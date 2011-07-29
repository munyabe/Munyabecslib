using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.VisualStudio;

namespace Test.Munyabe.VisualStudio
{
    /// <summary>
    /// <see cref="SolutionFileParser"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class SolutionFileParserTest
    {
        private string _testDataDir;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            var testDataDirName = "TestData";
            var sourceDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var sourceTestDataDir = Path.Combine(sourceDir, testDataDirName);
            _testDataDir = Path.Combine(TestContext.DeploymentDirectory, testDataDirName);

            Directory.Move(sourceTestDataDir, _testDataDir);
        }

        [TestMethod]
        public void GetProjectFilesTest()
        {
            var solutionFilePath = Path.Combine(_testDataDir, "TestSolutionFile.sln");

            var absolutePaths = SolutionFileParser.GetProjectFiles(solutionFilePath);
            Assert.AreEqual(6, absolutePaths.Count());

            var relativePaths = SolutionFileParser.GetProjectFiles(solutionFilePath, false);
            Assert.AreEqual(6, relativePaths.Count());
        }
    }
}
