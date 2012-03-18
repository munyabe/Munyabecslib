using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.VisualStudio;

namespace Test.Munyabe.VisualStudio
{
    /// <summary>
    /// <see cref="ProjectFileParser"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class ProjectFileParserTest : TestClassBase
    {
        [TestMethod]
        public void GetFilesTest()
        {
            var projectFilePath = GetTestDataPath("TestProjectFile.csproj");

            var csFiles = ProjectFileParser.GetFiles(projectFilePath, FileTypeFlags.CSharp).ToList();
            Assert.AreEqual(6, csFiles.Count);
            Assert.AreEqual("DelegateCommandTest.cs", Path.GetFileName(csFiles[0]));
            Assert.AreEqual("AssemblyInfo.cs", Path.GetFileName(csFiles[1]));
            Assert.AreEqual("UIElementUtilTest.cs", Path.GetFileName(csFiles[2]));
            Assert.AreEqual("Resources.Designer.cs", Path.GetFileName(csFiles[3]));
            Assert.AreEqual("TestSubView.xaml.cs", Path.GetFileName(csFiles[4]));
            Assert.AreEqual("TestView.xaml.cs", Path.GetFileName(csFiles[5]));

            var relativeCsPaths = ProjectFileParser.GetFiles(projectFilePath, FileTypeFlags.CSharp, false).ToList();
            Assert.AreEqual(6, relativeCsPaths.Count);
            Assert.AreEqual(@"TestView\TestView.xaml.cs", relativeCsPaths[5]);

            var xamlFiles = ProjectFileParser.GetFiles(projectFilePath, FileTypeFlags.Xaml, false).ToList();
            Assert.AreEqual(2, xamlFiles.Count);
            Assert.AreEqual(@"TestView\TestSubView.xaml", xamlFiles[0]);
            Assert.AreEqual(@"TestView\TestView.xaml", xamlFiles[1]);

            var resxFiles = ProjectFileParser.GetFiles(projectFilePath, FileTypeFlags.ResX, false).ToList();
            Assert.AreEqual(2, resxFiles.Count);
            Assert.AreEqual(@"Properties\Resources.resx", resxFiles[0]);
            Assert.AreEqual(@"Properties\Messages.resx", resxFiles[1]);

            var csAndXamlFiles = ProjectFileParser.GetFiles(projectFilePath, FileTypeFlags.Xaml | FileTypeFlags.CSharp);
            Assert.AreEqual(8, csAndXamlFiles.Count());

            var noneFiles = ProjectFileParser.GetFiles(projectFilePath, FileTypeFlags.None);
            Assert.AreEqual(0, noneFiles.Count());
        }
    }
}
