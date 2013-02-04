using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common.ComponentModel.DataAnnotations;

namespace Test.Munyabe.Common.ComponentModel.DataAnnotations
{
    /// <summary>
    /// <see cref="ValidationAttributeBuilder"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class ValidationAttributeBuilderTest
    {
        private const string ERROR_MESSAGE = "This is error.";

        [TestMethod]
        public void CreateDisplayBuilderTest()
        {
            var builder = ValidationAttributeBuilder.CreateDisplayBuilder("Hoge");
            var attribute = CreateAttribute(builder) as DisplayAttribute;

            Assert.IsNotNull(attribute);
            Assert.AreEqual("Hoge", attribute.Name);
        }

        [TestMethod]
        public void CreateRangeBuilderByIntTest()
        {
            var builder = ValidationAttributeBuilder.CreateRangeBuilder(3, 7, ERROR_MESSAGE);
            var attribute = CreateAttribute(builder) as RangeAttribute;

            Assert.IsNotNull(attribute);
            Assert.AreEqual(ERROR_MESSAGE, attribute.ErrorMessage);
            Assert.AreEqual(3, attribute.Minimum);
            Assert.AreEqual(7, attribute.Maximum);
        }

        [TestMethod]
        public void CreateRangeBuilderTest()
        {
            var minDate = new DateTime(1900, 1, 1).ToString();
            var maxDate = new DateTime(2079, 6, 6).ToString();

            var builder = ValidationAttributeBuilder.CreateRangeBuilder(typeof(DateTime), minDate, maxDate, ERROR_MESSAGE);
            var attribute = CreateAttribute(builder) as RangeAttribute;

            Assert.IsNotNull(attribute);
            Assert.AreEqual(ERROR_MESSAGE, attribute.ErrorMessage);
            Assert.AreEqual(minDate, attribute.Minimum);
            Assert.AreEqual(maxDate, attribute.Maximum);
        }

        [TestMethod]
        public void CreateRegularExpressionBuilderTest()
        {
            var pattern = @"[^\s]+";
            var builder = ValidationAttributeBuilder.CreateRegularExpressionBuilder(pattern, ERROR_MESSAGE);
            var attribute = CreateAttribute(builder) as RegularExpressionAttribute;

            Assert.IsNotNull(attribute);
            Assert.AreEqual(ERROR_MESSAGE, attribute.ErrorMessage);
            Assert.AreEqual(pattern, attribute.Pattern);
        }

        [TestMethod]
        public void CreateRequiredBuilderTest()
        {
            var builder = ValidationAttributeBuilder.CreateRequiredBuilder(ERROR_MESSAGE);
            var attribute = CreateAttribute(builder) as RequiredAttribute;

            Assert.IsNotNull(attribute);
            Assert.AreEqual(ERROR_MESSAGE, attribute.ErrorMessage);
        }

        [TestMethod]
        public void CreateStringLengthBuilderTest()
        {
            var builder = ValidationAttributeBuilder.CreateStringLengthBuilder(3, 7, ERROR_MESSAGE);
            var attribute = CreateAttribute(builder) as StringLengthAttribute;

            Assert.IsNotNull(attribute);
            Assert.AreEqual(ERROR_MESSAGE, attribute.ErrorMessage);
            Assert.AreEqual(3, attribute.MinimumLength);
            Assert.AreEqual(7, attribute.MaximumLength);
        }

        /// <summary>
        /// 指定されたビルダーを利用し属性のインスタンスを作成します。
        /// </summary>
        private Attribute CreateAttribute(CustomAttributeBuilder attributeBuilder)
        {
            var assemblyName = new AssemblyName("TestAssembly");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.Run,
                new CustomAttributeBuilder[] { attributeBuilder });

            var type = assemblyBuilder.DefineDynamicModule(assemblyName.Name)
                .DefineType("TestType", TypeAttributes.Public)
                .CreateType();

            return assemblyBuilder.GetCustomAttributes(true).OfType<Attribute>().FirstOrDefault();
        }
    }
}
