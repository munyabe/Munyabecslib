using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.VisualStudio;

namespace Test.Munyabe.VisualStudio
{
    /// <summary>
    /// <see cref="SolutionFileParser"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class SolutionFileParserTest : TestClassBase
    {
        [TestMethod]
        public void GetProjectFilesTest()
        {
            var solutionFilePath = GetTestDataPath("TestSolutionFile.sln");

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
