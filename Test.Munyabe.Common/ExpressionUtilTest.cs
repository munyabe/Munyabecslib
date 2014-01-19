using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="ExpressionUtil"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class ExpressionUtilTest
    {
        /// <summary>
        /// テストで使用するクラスです。
        /// </summary>
        public class ProjectBase
        {
            public string Name { get; set; }
        }

        /// <summary>
        /// テストで使用するクラスです。
        /// </summary>
        public class ConcreteProject : ProjectBase
        {
        }

        /// <summary>
        /// テストで使用するプロパティです。
        /// </summary>
        public ProjectBase Project { get; set; }

        [TestMethod]
        public void CreateGetterTest()
        {
            var now = DateTime.Now;
            var dateProperty = typeof(DateTime).GetProperty("Day");
            var dateGetter = ExpressionUtil.CreateGetter(dateProperty).Compile();
            Assert.AreEqual(now.Day, dateGetter(now));

            var project = new ConcreteProject { Name = "Hoge" };
            var nameProperty = project.GetType().GetProperty("Name");
            var getter = ExpressionUtil.CreateGetter(nameProperty).Compile();
            Assert.AreEqual("Hoge", getter(project));
        }

        [TestMethod]
        public void CreateGetterWithGenericTest()
        {
            var now = DateTime.Now;
            var dateProperty = typeof(DateTime).GetProperty("Day");
            var dateGetter = ExpressionUtil.CreateGetter<DateTime, int>(dateProperty).Compile();
            Assert.AreEqual(now.Day, dateGetter(now));

            var concrete = new ConcreteProject { Name = "Hoge" };
            var baseProperty = typeof(ProjectBase).GetProperty("Name");
            var basePropertySetter = ExpressionUtil.CreateGetter<ConcreteProject, string>(baseProperty).Compile();
            Assert.AreEqual("Hoge", basePropertySetter(concrete));
        }

        [TestMethod]
        public void CreateSetterTest()
        {
            var builder = new StringBuilder(10);
            var capacityProperty = typeof(StringBuilder).GetProperty("Capacity");
            var capacitySetter = ExpressionUtil.CreateSetter(capacityProperty).Compile();
            capacitySetter(builder, 20);
            Assert.AreEqual(20, builder.Capacity);

            var project = new ConcreteProject { Name = "Hoge" };
            var nameProperty = project.GetType().GetProperty("Name");
            var setter = ExpressionUtil.CreateSetter(nameProperty).Compile();
            setter(project, "Fuga");
            Assert.AreEqual("Fuga", project.Name);
        }

        [TestMethod]
        public void CreateSetterWithGenericTest()
        {
            var builder = new StringBuilder(10);
            var capacityProperty = typeof(StringBuilder).GetProperty("Capacity");
            var capacitySetter = ExpressionUtil.CreateSetter<StringBuilder, int>(capacityProperty).Compile();
            capacitySetter(builder, 20);
            Assert.AreEqual(20, builder.Capacity);

            var concrete = new ConcreteProject { Name = "Hoge" };
            var baseProperty = typeof(ProjectBase).GetProperty("Name");
            var basePropertySetter = ExpressionUtil.CreateSetter<ConcreteProject, string>(baseProperty).Compile();
            basePropertySetter(concrete, "Fuga");
            Assert.AreEqual("Fuga", concrete.Name);

            Project = new ProjectBase { Name = "Hoge" };
            var testProperty = typeof(ExpressionUtilTest).GetProperty("Project");
            var testPropertySetter = ExpressionUtil.CreateSetter<ExpressionUtilTest, ConcreteProject>(testProperty).Compile();
            testPropertySetter(this, new ConcreteProject { Name = "Fuga" });
            Assert.AreEqual("Fuga", Project.Name);
        }

        [TestMethod]
        public void GetMemberNameTest()
        {
            Assert.AreEqual("Project", ExpressionUtil.GetMemberName(() => Project));
            Assert.AreEqual("Empty", ExpressionUtil.GetMemberName(() => string.Empty));

            DateTime? now = DateTime.Now;
            Assert.AreEqual("Date", ExpressionUtil.GetMemberName(() => now.Value.Date));
        }

        [TestMethod]
        public void GetMemberNameWithInstanceTest()
        {
            Assert.AreEqual("Length", ExpressionUtil.GetMemberName<string, int>(dummy => dummy.Length));
        }

        [TestMethod]
        public void GetMethodNameTest()
        {
            Assert.AreEqual("GetHashCode", ExpressionUtil.GetMethodName(() => GetHashCode()));

            var hoge = "Hoge";
            Assert.AreEqual("GetType", ExpressionUtil.GetMethodName(() => hoge.GetType()));
        }
    }
}