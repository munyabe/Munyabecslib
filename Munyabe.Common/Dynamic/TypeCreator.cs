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
        /// プロパティのアクセサーの属性です。
        /// </summary>
        private const MethodAttributes PROPERTY_ATTRIBUTES =
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

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

            properties.ForEach(property =>
            {
                var type = property.Type;
                var name = property.Name;
                var fieldBuilder = typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);

                var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);
                propertyBuilder.SetGetMethod(
                    CreateGetterBuilder(name, type, typeBuilder, fieldBuilder));
                propertyBuilder.SetSetMethod(
                    CreateSetterBuilder(name, type, typeBuilder, fieldBuilder));
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

            properties.ForEach(property =>
            {
                var type = property.Type;
                var name = property.Name;
                var fieldBuilder = typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);

                var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);
                propertyBuilder.SetGetMethod(
                    CreateGetterBuilder(name, type, typeBuilder, fieldBuilder));
                propertyBuilder.SetSetMethod(
                    CreateNotifedSetterBuilder(name, type, typeBuilder, fieldBuilder, onPropertyChangedMethod));
                property.AttributeBuilders.ForEach(propertyBuilder.SetCustomAttribute);
            });

            attributeBuilders.ForEach(typeBuilder.SetCustomAttribute);

            return typeBuilder.CreateType();
        }

        /// <summary>
        /// 自動実装プロパティのゲッターを作成します。
        /// </summary>
        private static MethodBuilder CreateGetterBuilder(string name, Type type, TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("get_" + name, PROPERTY_ATTRIBUTES, type, Type.EmptyTypes);
            var generator = methodBuilder.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, fieldBuilder);
            generator.Emit(OpCodes.Ret);
            return methodBuilder;
        }

        /// <summary>
        /// 自動実装プロパティのセッターを作成します。
        /// </summary>
        private static MethodBuilder CreateSetterBuilder(string name, Type type, TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("set_" + name, PROPERTY_ATTRIBUTES, null, new Type[] { type });
            var generator = methodBuilder.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, fieldBuilder);
            generator.Emit(OpCodes.Ret);
            return methodBuilder;
        }

        /// <summary>
        /// プロパティの変更を通知するセッターを作成します。
        /// </summary>
        private static MethodBuilder CreateNotifedSetterBuilder(string name, Type type, TypeBuilder typeBuilder, FieldBuilder fieldBuilder, MethodInfo onPropertyChangedMethod)
        {
            var methodBuilder = typeBuilder.DefineMethod("set_" + name, PROPERTY_ATTRIBUTES, null, new Type[] { type });
            var generator = methodBuilder.GetILGenerator();

            if (type.IsPrimitive || type == typeof(string))
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, fieldBuilder);
                generator.Emit(OpCodes.Ldarg_1);

                generator.Emit(OpCodes.Ceq);
            }
            else if (IsNullable(type))
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldflda, fieldBuilder);
                generator.Emit(OpCodes.Ldarg_1);

                generator.Emit(OpCodes.Box, type);
                generator.Emit(OpCodes.Constrained, type);

                var equalsMethod = type.GetMethod("Equals", new[] { typeof(object) });
                generator.Emit(OpCodes.Callvirt, equalsMethod);
            }
            else
            {
                var equalsMethod = type.GetMethod("op_Equality", new[] { type, type });
                if (equalsMethod != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, fieldBuilder);
                    generator.Emit(OpCodes.Ldarg_1);

                    generator.Emit(OpCodes.Call, equalsMethod);
                    generator.Emit(OpCodes.Ldc_I4_1);

                    generator.Emit(OpCodes.Ceq);
                }
                else
                {
                    throw new ArgumentException(string.Format(
                        "The property [{0}] is invalid. The type [{1}] is not implement the [op_Equality] method.",
                        name, type));
                }
            }

            var returnLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Brtrue_S, returnLabel);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, fieldBuilder);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldstr, name);
            generator.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
            generator.MarkLabel(returnLabel);
            generator.Emit(OpCodes.Ret);
            return methodBuilder;
        }

        /// <summary>
        /// <see cref="Nullable"/>かどうかを判定します。
        /// </summary>
        private static bool IsNullable(Type source)
        {
            return source.IsGenericType && source.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}