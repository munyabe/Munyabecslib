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

            var absolutePaths = SolutionFileParser.GetProjectFiles(solutionFilePath).ToList();
            Assert.AreEqual(6, absolutePaths.Count);
            Assert.AreEqual("Munyabe.Common.csproj", Path.GetFileName(absolutePaths[0]));
            Assert.AreEqual("Test.Munyabe.Common.csproj", Path.GetFileName(absolutePaths[1]));
            Assert.AreEqual("Munyabe.Windows.csproj", Path.GetFileName(absolutePaths[2]));
            Assert.AreEqual("Test.Munyabe.Windows.csproj", Path.GetFileName(absolutePaths[3]));
            Assert.AreEqual("Munyabe.VisualStudio.csproj", Path.GetFileName(absolutePaths[4]));
            Assert.AreEqual("Test.Munyabe.VisualStudio.csproj", Path.GetFileName(absolutePaths[5]));

            var relativePaths = SolutionFileParser.GetProjectFiles(solutionFilePath, false).ToList();
            Assert.AreEqual(6, relativePaths.Count);
            Assert.AreEqual(@"Munyabe.Common\Munyabe.Common.csproj", relativePaths[0]);
        }
    }
}
