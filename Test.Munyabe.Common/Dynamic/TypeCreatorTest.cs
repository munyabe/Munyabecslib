using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common.ComponentModel;
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
        [TestMethod]
        public void CreateIntsanceTest()
        {
            var dynamicType = TypeCreator.CreateType(
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

            var type = data.GetType();
            Assert.AreEqual("Person", type.FullName);

            var properties = type.GetProperties();
            Assert.AreEqual("FirstName", properties[0].Name);
            Assert.AreEqual("LastName", properties[1].Name);
            Assert.AreEqual("Age", properties[2].Name);

            dynamic person = data;
            person.FirstName = "鈴木";
            person.Age = 1;
            Assert.AreEqual("鈴木", person.FirstName);
            Assert.AreEqual(1, person.Age);

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

        [TestMethod]
        public void CreateConcreteIntsanceTest()
        {
            var dynamicType = TypeCreator.CreateType(
                "ConcretePerson",
                new[] { new DynamicPropertyInfo("Name", typeof(string)) },
                typeof(ParentClass));

            Assert.IsInstanceOfType(Activator.CreateInstance(dynamicType), typeof(ParentClass));
        }

        [TestMethod]
        public void CreateNotifiedTypeTest()
        {
            var createProperties = new[]
                {
                    new DynamicPropertyInfo("Name", typeof(string)),
                    new DynamicPropertyInfo("Age", typeof(int)),
                    new DynamicPropertyInfo("Number", typeof(int?)),
                    new DynamicPropertyInfo("Birthday", typeof(DateTime)),
                    new DynamicPropertyInfo("IsMale", typeof(bool)),
                };

            var dynamicType = TypeCreator.CreateNotifiedType<NotifyPropertyChangedBase>(
                "NotifiedPerson", createProperties);

            var data = Activator.CreateInstance(dynamicType) as INotifyPropertyChanged;
            Assert.IsNotNull(data);

            var type = data.GetType();
            Assert.AreEqual("NotifiedPerson", type.FullName);

            var personProperties = type.GetProperties();
            Assert.IsTrue(personProperties.Select(prop => prop.Name).SequenceEqual(
                personProperties.Select(prop => prop.Name)));

            var propertyChangedCounts = createProperties.ToDictionary(prop => prop.Name, prop => 0);

            data.PropertyChanged += (sender, e) => propertyChangedCounts[e.PropertyName]++;
            dynamic person = data;

            person.Name = "鈴木";
            person.Age = 36;
            person.Number = 51;
            person.Birthday = new DateTime(1973, 10, 22);
            person.IsMale = true;

            Assert.AreEqual("鈴木", person.Name);
            Assert.AreEqual(36, person.Age);
            Assert.AreEqual(51, person.Number);
            Assert.AreEqual(new DateTime(1973, 10, 22), person.Birthday);
            Assert.AreEqual(true, person.IsMale);

            Assert.AreEqual(1, propertyChangedCounts["Name"]);
            Assert.AreEqual(1, propertyChangedCounts["Age"]);
            Assert.AreEqual(1, propertyChangedCounts["Number"]);
            Assert.AreEqual(1, propertyChangedCounts["Birthday"]);
            Assert.AreEqual(1, propertyChangedCounts["IsMale"]);

            person.Name = "鈴木";
            person.Age = 37;
            person.Number = 52;
            person.Birthday = new DateTime(1973, 10, 22);
            person.IsMale = true;

            Assert.AreEqual(1, propertyChangedCounts["Name"]);
            Assert.AreEqual(2, propertyChangedCounts["Age"]);
            Assert.AreEqual(2, propertyChangedCounts["Number"]);
            Assert.AreEqual(1, propertyChangedCounts["Birthday"]);
            Assert.AreEqual(1, propertyChangedCounts["IsMale"]);

            person.Name = "suzuki";
            person.Age = 37;
            person.Number = 52;
            person.Birthday = new DateTime(1974, 10, 22);
            person.IsMale = false;

            Assert.AreEqual(2, propertyChangedCounts["Name"]);
            Assert.AreEqual(2, propertyChangedCounts["Age"]);
            Assert.AreEqual(2, propertyChangedCounts["Number"]);
            Assert.AreEqual(2, propertyChangedCounts["Birthday"]);
            Assert.AreEqual(2, propertyChangedCounts["IsMale"]);
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