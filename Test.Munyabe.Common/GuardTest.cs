using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="Guard"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class GuardTest
    {
        [TestMethod]
        public void ArgumentIsPositiveTest()
        {
            Guard.ArgumentIsPositive(1, "argName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentIsPositiveThrowExceptionTest()
        {
            Guard.ArgumentIsPositive(0, "argName");
        }

        [TestMethod]
        public void ArgumentNotNullTest()
        {
            Guard.ArgumentNotNull(string.Empty, "argName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNullThrowExceptionTest()
        {
            string nullValue = null;
            Guard.ArgumentNotNull(nullValue, "argName");
        }

        [TestMethod]
        public void ArgumentNotNullOrEmptyTest()
        {
            Guard.ArgumentNotNullOrEmpty("Hoge", "argName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNullOrEmptyThrowNullExceptionTest()
        {
            Guard.ArgumentNotNullOrEmpty(null, "argName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentNotNullOrEmptyThrowEmptyExceptionTest()
        {
            Guard.ArgumentNotNullOrEmpty(string.Empty, "argName");
        }

        [TestMethod]
        public void InstanceIsAssignableTest()
        {
            var attribute = new TestClassAttribute();
            Guard.InstanceIsAssignable(typeof(TestClassAttribute), attribute, "attribute");
            Guard.InstanceIsAssignable(typeof(Attribute), attribute, "attribute");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InstanceIsAssignableThrowExceptionTest()
        {
            var assignment = "Hoge";
            Guard.InstanceIsAssignable(typeof(Attribute), assignment, "assignment");
        }

        [TestMethod]
        public void TypeIsAssignableTest()
        {
            Guard.TypeIsAssignable(typeof(Attribute), typeof(TestClassAttribute), "argName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TypeIsAssignableThrowExceptionTest()
        {
            Guard.TypeIsAssignable(typeof(Attribute), typeof(string), "argName");
        }

        [TestMethod]
        public void TypeIsEqualTest()
        {
            Guard.TypeIsEqual(typeof(Attribute), typeof(Attribute), "argName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TypeIsEqualThrowExceptionTest()
        {
            Guard.TypeIsEqual(typeof(Attribute), typeof(TestClassAttribute), "argName");
        }

        [TestMethod]
        public void TypeNotGenericTest()
        {
            Guard.TypeNotGeneric(typeof(string), "argName");
            Guard.TypeNotGeneric(typeof(Nullable), "argName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TypeNotGenericThrowExceptionTest()
        {
            Guard.TypeNotGeneric(typeof(Nullable<>), "argName");
        }
    }
}