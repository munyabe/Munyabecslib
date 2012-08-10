using System.IO;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Munyabe.VisualStudio
{
    /// <summary>
    /// テストクラスの基底クラスです。
    /// </summary>
    [TestClass]
    public abstract class TestClassBase
    {
        /// <summary>
        /// テストデータのディレクトリー名です。
        /// </summary>
        private const string TEST_DATA_DIR_NAME = "TestData";

        /// <summary>
        /// テストデータのディレクトリーの絶対パスです。
        /// </summary>
        private string _testDataDir;

        /// <summary>
        /// 現在のテストの実行についての情報および機能を提供するテストコンテキストを取得または設定します。
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// テストクラスを初期化します。
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var sourceDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var sourceTestDataDir = Path.Combine(sourceDir, TEST_DATA_DIR_NAME);
            _testDataDir = Path.Combine(TestContext.DeploymentDirectory, TEST_DATA_DIR_NAME);

            if (Directory.Exists(sourceTestDataDir) && Directory.Exists(_testDataDir) == false)
            {
                FileSystem.CopyDirectory(sourceTestDataDir, _testDataDir);
            }
        }

        /// <summary>
        /// 指定したテストデータの絶対パスを取得します。
        /// </summary>
        /// <param name="fileName">ストデータのファイル名</param>
        /// <returns>テストデータの絶対パス</returns>
        protected string GetTestDataPath(string fileName)
        {
            return Path.Combine(_testDataDir, fileName);
        }
    }
}
