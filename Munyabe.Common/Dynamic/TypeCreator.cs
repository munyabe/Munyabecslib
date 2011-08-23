using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

namespace Munyabe.Common.Dynamic
{
    /// <summary>
    /// 実行時にクラスを動的に作成する機能を提供するクラスです。
    /// </summary>
    public static class TypeCreator
    {
        /// <summary>
        /// 動的アセンブリ内のモジュールです。
        /// </summary>
        private static ModuleBuilder _moduleBuilder = AppDomain.CurrentDomain
            .DefineDynamicAssembly(new AssemblyName { Name = ASSEMBLY_NAME }, AssemblyBuilderAccess.Run)
            .DefineDynamicModule(MODULE_NAME);

        /// <summary>
        /// <see cref="TypeCreator"/>で作成される型が宣言されるアセンブリ名です。
        /// </summary>
        public const string ASSEMBLY_NAME = "DynamicTypeAssembly";

        /// <summary>
        /// <see cref="TypeCreator"/>で作成される型が定義されているモジュール名です。
        /// </summary>
        public const string MODULE_NAME = "DynamicTypeModule";

        /// <summary>
        /// POCO (Plain Old CLR Objects) のクラスを作成します。
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="properties">作成するクラスのプロパティ</param>
        /// <param name="attributeBuilders">クラスに付与する属性のビルダー</param>
        /// <returns>作成したクラス</returns>
        public static Type CreateType(
            string className,
            IEnumerable<DynamicPropertyInfo> properties,
            params CustomAttributeBuilder[] attributeBuilders)
        {
            return CreateType(className, properties, null, attributeBuilders);
        }

        /// <summary>
        /// POCO (Plain Old CLR Objects) のクラスを作成します。
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="properties">作成するクラスのプロパティ</param>
        /// <param name="parentType">継承するクラス</param>
        /// <param name="attributeBuilders">クラスに付与する属性のビルダー</param>
        /// <returns>作成したクラス</returns>
        public static Type CreateType(
            string className,
            IEnumerable<DynamicPropertyInfo> properties,
            Type parentType,
            params CustomAttributeBuilder[] attributeBuilders)
        {
            var typeBuilder = _moduleBuilder.DefineType(
                className, TypeAttributes.Public | TypeAttributes.Class, parentType);

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

        /// <summary>
        /// プロパティの変更を通知する POCO (Plain Old CLR Objects) のクラスを作成します。
        /// </summary>
        /// <remarks>
        /// <typeparamref name="T"/>が<c>OnPropertyChanged(string)</c>を実装している必要があります。
        /// <paramref name="properties"/>のそれぞれの型はプリミティブ型、<see cref="string"/>、
        /// または比較演算子 == をオーバーロードしている型である必要があります。
        /// </remarks>
        /// <typeparam name="T">継承するクラスの型</typeparam>
        /// <param name="className">クラス名</param>
        /// <param name="properties">作成するクラスのプロパティ</param>
        /// <param name="attributeBuilders">クラスに付与する属性のビルダー</param>
        /// <returns>作成したクラス</returns>
        /// <exception cref="ArgumentException"><typeparamref name="T"/>が<c>OnPropertyChanged(string)</c>を実装していません。</exception>
        /// <exception cref="ArgumentException">指定されたプロパティの型が比較演算子 == を実装していません。</exception>
        public static Type CreateNotifiedType<T>(
            string className,
            IEnumerable<DynamicPropertyInfo> properties,
            params CustomAttributeBuilder[] attributeBuilders) where T : INotifyPropertyChanged
        {
            var parentType = typeof(T);
            var onPropertyChangedMethod = parentType.GetMethod(
                "OnPropertyChanged",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(string) },
                null);

            if (onPropertyChangedMethod == null)
            {
                throw new ArgumentException(
                    string.Format("The parent type [{0}] is not implement the [OnPropertyChanged] method.", parentType));
            }

            var typeBuilder = _moduleBuilder.DefineType(
                className, TypeAttributes.Public | TypeAttributes.Class, parentType);

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
                setMethodGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                setMethodGenerator.Emit(OpCodes.Ldarg_1);

                if (type.IsPrimitive == false && type != typeof(string))
                {
                    var equalsMethod = type.GetMethod("op_Equality", new[] { type, type });
                    if (equalsMethod != null)
                    {
                        setMethodGenerator.Emit(OpCodes.Call, equalsMethod);
                        setMethodGenerator.Emit(OpCodes.Ldc_I4_1);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format(
                            "The property [{0}] is invalid. The type [{1}] is not implement the [op_Equality] method.",
                            name, type));
                    }
                }

                setMethodGenerator.Emit(OpCodes.Ceq);
                var returnLabel = setMethodGenerator.DefineLabel();
                setMethodGenerator.Emit(OpCodes.Brtrue_S, returnLabel);
                setMethodGenerator.Emit(OpCodes.Ldarg_0);
                setMethodGenerator.Emit(OpCodes.Ldarg_1);
                setMethodGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                setMethodGenerator.Emit(OpCodes.Ldarg_0);
                setMethodGenerator.Emit(OpCodes.Ldstr, name);
                setMethodGenerator.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                setMethodGenerator.MarkLabel(returnLabel);
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