using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Munyabe.Common.Dynamic
{
    /// <summary>
    /// 実行時に動的クラスを作成する機能を提供するクラスです。
    /// </summary>
    public static class TypeCreator
    {
        /// <summary>
        /// <see cref="TypeCreator"/>で作成される型が宣言されるアセンブリ名です。
        /// </summary>
        public const string ASSEMBLY_NAME = "DynamicTypeAssembly";

        /// <summary>
        /// <see cref="TypeCreator"/>で作成される型が定義されているモジュール名です。
        /// </summary>
        public const string MODULE_NAME = "DynamicTypeModule";

        /// <summary>
        /// 動的クラスを作成します。
        /// <remarks>POCO (Plain Old CLR Objects) Entityを作成します。</remarks>
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="properties">追加するプロパティ</param>
        /// <param name="attributeBuilders">クラスに付与する属性の<c>Builder</c></param>
        /// <returns>作成したクラス</returns>
        public static Type CreateDynamicType(
            string className,
            IEnumerable<DynamicPropertyInfo> properties,
            params CustomAttributeBuilder[] attributeBuilders)
        {
            return CreateDynamicType(className, properties, null, attributeBuilders);
        }

        /// <summary>
        /// 動的クラスを作成します。
        /// <remarks>POCO (Plain Old CLR Objects) Entityを作成します。</remarks>
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="properties">追加するプロパティ</param>
        /// <param name="parentType">継承するクラス</param>
        /// <param name="attributeBuilders">クラスに付与する属性の<c>Builder</c></param>
        /// <returns>作成したクラス</returns>
        public static Type CreateDynamicType(
            string className,
            IEnumerable<DynamicPropertyInfo> properties,
            Type parentType,
            params CustomAttributeBuilder[] attributeBuilders)
        {
            var assemblyName = new AssemblyName { Name = ASSEMBLY_NAME };
            var typeBuilder = AppDomain.CurrentDomain
                .DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run)
                .DefineDynamicModule(MODULE_NAME)
                .DefineType(className, TypeAttributes.Public | TypeAttributes.Class, parentType);

            var propAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            properties.ForEach(property =>
            {
                var type = property.Type;
                var name = property.Name;
                var fieldBuilder = typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);

                var getMethodBuilder = typeBuilder.DefineMethod("get_" + name, propAttributes, type, Type.EmptyTypes);
                var getMethodGenerator = getMethodBuilder.GetILGenerator();
                getMethodGenerator.Emit(OpCodes.Ldarg_0);
                getMethodGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                getMethodGenerator.Emit(OpCodes.Ret);

                var setMethodBuilder = typeBuilder.DefineMethod("set_" + name, propAttributes, null, new Type[] { type });
                var setMethodGenerator = setMethodBuilder.GetILGenerator();
                setMethodGenerator.Emit(OpCodes.Ldarg_0);
                setMethodGenerator.Emit(OpCodes.Ldarg_1);
                setMethodGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                setMethodGenerator.Emit(OpCodes.Ret);

                var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);
                propertyBuilder.SetGetMethod(getMethodBuilder);
                propertyBuilder.SetSetMethod(setMethodBuilder);

                property.AttributeBuilders.ForEach(propertyBuilder.SetCustomAttribute);
            });

            attributeBuilders.ForEach(typeBuilder.SetCustomAttribute);

            return typeBuilder.CreateType();
        }
    }
}