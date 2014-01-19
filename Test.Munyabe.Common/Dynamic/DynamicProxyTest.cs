using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common.Dynamic;

namespace Test.Munyabe.Common.Dynamic
{
    /// <summary>
    /// <see cref="DynamicProxy"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class DynamicProxyTest
    {
        private DynamicProxy _dynamicProxy = new DynamicProxy();

        private Person _person = new Person { Name = "Ichiro", Age = 37 };

        [TestInitialize]
        public void Initialize()
        {
            _dynamicProxy.RegisterModel(_person);
        }

        [TestMethod]
        public void PropertyNamesTest()
        {
            Assert.AreEqual(2, _dynamicProxy.PropertyNames.Count);
            Assert.IsTrue(_dynamicProxy.PropertyNames.Contains("Name"));
            Assert.IsTrue(_dynamicProxy.PropertyNames.Contains("Age"));
            Assert.IsFalse(_dynamicProxy.PropertyNames.Contains("Hoge"));
        }

        [TestMethod]
        public void ValidatedPropertyNamesTest()
        {
            Assert.AreEqual(1, _dynamicProxy.ValidatedPropertyNames.Count);
            Assert.IsTrue(_dynamicProxy.ValidatedPropertyNames.Contains("Name"));
            Assert.IsFalse(_dynamicProxy.ValidatedPropertyNames.Contains("Age"));
        }

        [TestMethod]
        public void GetMemberTest()
        {
            Assert.AreEqual(_person.Name, _dynamicProxy.GetMember("Name"));
            Assert.AreEqual(_person.Age, _dynamicProxy.GetMember("Age"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetMemberErrorTest()
        {
            _dynamicProxy.GetMember("Hoge");
        }

        [TestMethod]
        public void GetSourceModelTest()
        {
            var team = new Team { TeamName = "Mariners" };
            _dynamicProxy.RegisterModel(team);

            var sourcePerson = _dynamicProxy.GetSourceModel("Name");
            var sourceTeam = _dynamicProxy.GetSourceModel("TeamName");
            var dummy = _dynamicProxy.GetSourceModel("Hoge");

            Assert.AreSame(_person, sourcePerson);
            Assert.AreSame(team, sourceTeam);
            Assert.IsNull(dummy);
        }

        [TestMethod]
        public void RegisterModelTest()
        {
            var person = new Person { Name = "Matsui", Age = 36 };
            _dynamicProxy.RegisterModel(person);

            dynamic dynamicProxy = _dynamicProxy; ;

            Assert.AreEqual("Matsui", dynamicProxy.Name);
            Assert.AreEqual(36, dynamicProxy.Age);

            dynamicProxy.Name = "Matsuzaka";
            dynamicProxy.Age = 30;

            Assert.AreEqual("Matsuzaka", person.Name);
            Assert.AreEqual(30, person.Age);
        }

        [TestMethod]
        public void SetMemberTest()
        {
            _dynamicProxy.SetMember("Name", "Matsui");
            _dynamicProxy.SetMember("Age", 36);

            Assert.AreEqual("Matsui", _dynamicProxy.GetMember("Name"));
            Assert.AreEqual(36, _dynamicProxy.GetMember("Age"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetMemberErrorTest()
        {
            _dynamicProxy.SetMember("Hoge", null);
        }

        [TestMethod]
        public void TryGetMemberTest()
        {
            dynamic dynamicProxy = _dynamicProxy; ;
            Assert.AreEqual(_person.Name, dynamicProxy.Name);
            Assert.AreEqual(_person.Age, dynamicProxy.Age);
        }

        [TestMethod]
        public void TrySetMemberTest()
        {
            dynamic dynamicProxy = _dynamicProxy; ;
            dynamicProxy.Name = "Matsui";
            dynamicProxy.Age = 36;

            Assert.AreEqual("Matsui", dynamicProxy.Name);
            Assert.AreEqual(36, dynamicProxy.Age);
        }

        private class Person
        {
            [Required]
            public string Name { get; set; }

            public int Age { get; set; }
        }

        private class Team
        {
            public string TeamName { get; set; }
        }
    }
}