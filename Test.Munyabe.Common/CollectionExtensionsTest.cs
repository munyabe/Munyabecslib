using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="CollectionExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class CollectionExtensionsTest
    {
        ObservableCollection<string> _collection = new ObservableCollection<string> { "a", "b", "c", "d", "e" };

        [TestMethod]
        public void RemoveOutRangeToEndTest()
        {
            _collection.RemoveOutRange(3);

            var expected = new string[] { "a", "b", "c" };
            CollectionAssert.AreEqual(expected, _collection);
        }

        [TestMethod]
        public void RemoveOutRangeTest()
        {
            _collection.RemoveOutRange(1, 3);

            var expected = new string[] { "b", "c", "d" };
            CollectionAssert.AreEqual(expected, _collection);
        }

        [TestMethod]
        public void RemoveRangeTest()
        {
            _collection.RemoveRange(1, 3);

            var expected = new string[] { "a", "e" };
            CollectionAssert.AreEqual(expected, _collection);
        }
    }
}