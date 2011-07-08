using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common.DataAnnotations.DataAnnotations;
using Munyabe.Common.Dynamic;

namespace Test.Munyabe.Common.Dynamic
{
    /// <summary>
    /// <see cref="TypeCreator"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public sealed class TypeCreatorTest
    {
        /// <summary>
        /// 動的クラスのインスタンスを作成し、指定の属性・プロパティが存在することを確認します。
        /// </summary>
        [TestMethod]
        public void CreateIntsanceTest()
        {
            var dynamicType = TypeCreator.CreateDynamicType(
                "Person",
                new[]
                {
                    new DynamicPropertyInfo("FirstName", typeof(string),
                        ValidationAttributeBuilder.CreateRequiredBuilder("FirstNameは必須です")),
                    new DynamicPropertyInfo("LastName", typeof(string),
                        ValidationAttributeBuilder.CreateStringLengthBuilder(0, 5, "LastNameは5文字以内です。"),
                        ValidationAttributeBuilder.CreateRegularExpressionBuilder("^[1-9]$", "エラーです。")),
                    new DynamicPropertyInfo("Age", typeof(int)),
                },
                new CustomAttributeBuilder(
                    typeof(TestClassAttribute).GetConstructor(Type.EmptyTypes), new object[] { }));

            var data = Activator.CreateInstance(dynamicType);

            dynamic person = data;
            person.FirstName = "鈴木";
            person.Age = 1;
            Assert.AreEqual("鈴木", person.FirstName);
            Assert.AreEqual(1, person.Age);

            var type = data.GetType();
            Assert.AreEqual("Person", type.FullName);

            var properties = type.GetProperties();
            Assert.AreEqual("FirstName", properties[0].Name);
            Assert.AreEqual("LastName", properties[1].Name);
            Assert.AreEqual("Age", properties[2].Name);

            var classAtt = data.GetType().GetCustomAttributes(false);
            Assert.IsInstanceOfType(classAtt.Single(), typeof(TestClassAttribute));

            var firstNameAtt = Attribute.GetCustomAttributes(properties[0]);
            Assert.IsInstanceOfType(firstNameAtt.Single(), typeof(RequiredAttribute));

            var lastNameAtt = Attribute.GetCustomAttributes(properties[1]);
            Assert.AreEqual(2, lastNameAtt.Count());
            Assert.IsInstanceOfType(lastNameAtt.First(), typeof(StringLengthAttribute));
            Assert.IsInstanceOfType(lastNameAtt.Skip(1).First(), typeof(RegularExpressionAttribute));

            var ageAtt = Attribute.GetCustomAttributes(properties[2]);
            Assert.IsFalse(ageAtt.Any());
        }

        /// <summary>
        /// 指定したクラスを継承した動的クラスを作成できることを確認します。
        /// </summary>
        [TestMethod]
        public void CreateConcreteIntsanceTest()
        {
            var dynamicType = TypeCreator.CreateDynamicType(
                "Person",
                new[] { new DynamicPropertyInfo("Name", typeof(string)) },
                typeof(ParentClass));

            Assert.IsInstanceOfType(Activator.CreateInstance(dynamicType), typeof(ParentClass));
        }

        /// <summary>
        /// <see cref="CreateConcreteIntsanceTest"/>で作成するクラスの抽象クラスです。
        /// </summary>
        public abstract class ParentClass
        {
            public int Id { get; set; }
        }
    }
}