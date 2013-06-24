using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;
using Munyabe.Common.Algorithms;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="IEnumerableExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class IEnumerableExtensionsTest
    {
        [TestMethod]
        public void CompareCountTest()
        {
            var count9 = Enumerable.Range(0, 9);
            var count10A = Enumerable.Range(0, 10);
            var count10B = Enumerable.Range(0, 10);
            var count11 = Enumerable.Range(0, 11);

            Assert.AreEqual(0, count10A.CompareCount(count10A));
            Assert.AreEqual(0, count10A.CompareCount(count10B));

            Assert.AreEqual(1, count11.CompareCount(count10A));
            Assert.AreEqual(-1, count9.CompareCount(count10A));

            Assert.AreEqual(1, count9.CompareCount(null));
        }

        [TestMethod]
        public void CompareCountByIntTest()
        {
            var count9 = Enumerable.Range(0, 9);
            var count10 = Enumerable.Range(0, 10);
            var count11 = Enumerable.Range(0, 11);

            Assert.AreEqual(0, count9.CompareCount(9));
            Assert.AreEqual(0, count10.CompareCount(10));
            Assert.AreEqual(0, Enumerable.Empty<int>().CompareCount(0));

            Assert.AreEqual(1, count11.CompareCount(10));
            Assert.AreEqual(-1, count9.CompareCount(10));

            Assert.AreEqual(1, count9.CompareCount(-1));
        }

        [TestMethod]
        public void ConcatTest()
        {
            var actual1 = Enumerable.Range(0, 9).Concat(9); ;

            Assert.AreEqual(10, actual1.Count());
            Assert.AreEqual(9, actual1.Last());

            var actual2 = Enumerable.Empty<int>().Concat(7);

            Assert.AreEqual(1, actual2.Count());
            Assert.AreEqual(7, actual2.First());
        }

        [TestMethod]
        public void DistinctTest()
        {
            var actual1 = new[]
            {
                new Person { Name = "A", Age = 1 },
                new Person { Name = "B", Age = 2 },
                new Person { Name = "C", Age = 3 },
                new Person { Name = "D", Age = 3 },
                new Person { Name = "E", Age = 2 }
            }
            .Distinct(person => person.Age)
            .Select(person => person.Name);

            var expected1 = new[] { "A", "B", "C" };
            Assert.IsTrue(actual1.SequenceEqual(expected1));

            var actual2 = new[]
            {
                new Person { Name = "A", Age = 1 },
                new Person { Name = null, Age = 2 },
                new Person { Name = "A", Age = 3 },
                new Person { Name = null, Age = 4 },
                new Person { Name = null, Age = 5 }
            }
            .Distinct(person => person.Name)
            .Select(person => person.Age);

            var expected2 = new[] { 1, 2 };
            Assert.IsTrue(actual2.SequenceEqual(expected2));
        }

        [TestMethod]
        public void ForEachTest()
        {
            var sum = 0;
            Enumerable.Range(1, 10).ForEach(each => sum += each);

            Assert.AreEqual(55, sum);
        }

        [TestMethod]
        public void ForEachWithIndexTest()
        {
            var sum = 0;
            Enumerable.Range(1, 10).ForEach((each, i) => sum += i);

            Assert.AreEqual(45, sum);
        }

        [TestMethod]
        public void IsCountTest()
        {
            Assert.IsTrue(Enumerable.Range(0, 7).IsCount(7));

            Assert.IsFalse(Enumerable.Range(0, 7).IsCount(6));
            Assert.IsFalse(Enumerable.Range(0, 7).IsCount(8));
            Assert.IsFalse(Enumerable.Range(0, 7).IsCount(0));
            Assert.IsFalse(Enumerable.Range(0, 7).IsCount(-1));
        }

        [TestMethod]
        public void IsOverlappedTest()
        {
            var overlappedNums = new[] { 1, 2, 2, 3 };
            Assert.IsTrue(overlappedNums.IsOverlapped());

            var nonOverlappedNums = new[] { 1, 2, 3, 4 };
            Assert.IsFalse(nonOverlappedNums.IsOverlapped());

            var overlappedStrings = new[] { "A", "B", "C", "A" };
            Assert.IsTrue(overlappedStrings.IsOverlapped());

            var nonOverlappedStrings = new[] { "A", "B", "C", "D" };
            Assert.IsFalse(nonOverlappedStrings.IsOverlapped());

            int overlappedNum;
            Assert.IsTrue(overlappedNums.IsOverlapped(out overlappedNum));
            Assert.AreEqual(2, overlappedNum);

            int dummyNum;
            Assert.IsFalse(nonOverlappedNums.IsOverlapped(out dummyNum));
            Assert.AreEqual(0, dummyNum);

            string overlappedString;
            Assert.IsTrue(overlappedStrings.IsOverlapped(out overlappedString));
            Assert.AreEqual("A", overlappedString);

            string dummyString;
            Assert.IsFalse(nonOverlappedStrings.IsOverlapped(out dummyString));
            Assert.IsNull(dummyString);
        }

        [TestMethod]
        public void IsSingleTest()
        {
            Assert.IsTrue(Enumerable.Range(0, 1).IsSingle());
            Assert.IsFalse(Enumerable.Range(0, 2).IsSingle());

            Assert.IsTrue(Enumerable.Range(1, 9).IsSingle(num => num % 5 == 0));
            Assert.IsFalse(Enumerable.Range(1, 10).IsSingle(num => num % 5 == 0));
            Assert.IsFalse(Enumerable.Range(1, 10).IsSingle(num => num % 11 == 0));
        }

        [TestMethod]
        public void IsSingleKindTest()
        {
            Assert.IsTrue(new[] { 1 }.IsSingleKind());
            Assert.IsTrue(new[] { 2, 2 }.IsSingleKind());

            Assert.IsFalse(new[] { 1, 2 }.IsSingleKind());
            Assert.IsFalse(new[] { 1, 1, 2 }.IsSingleKind());
            Assert.IsFalse(Enumerable.Empty<int>().IsSingleKind());

            Assert.IsTrue(new string[] { null }.IsSingleKind());
            Assert.IsTrue(new string[] { null, null }.IsSingleKind());
            Assert.IsFalse(new[] { null, "a" }.IsSingleKind());
        }

        [TestMethod]
        public void OverlappedTest()
        {
            var actual1 = new[] { 1, 2, 3, 2, 3 }.Overlapped();
            var expected1 = new[] { 2, 3 };

            Assert.IsTrue(actual1.SequenceEqual(expected1));

            var actual2 = new[] { 1, 2, 3, 2, 3, 3 }.Overlapped();
            var expected2 = new[] { 2, 3 };

            Assert.IsTrue(actual2.SequenceEqual(expected2));

            var actual3 = new[] { 1, 2, 3, 4, 5 }.Overlapped();
            var expected3 = Enumerable.Empty<int>();

            Assert.IsTrue(actual3.SequenceEqual(expected3));
        }

        [TestMethod]
        public void RecursiveTest()
        {
            var testTree = new TestTree("Parent");

            var child1 = new TestTree("Child_1");
            child1.Children.AddRange(new[]
                {
                    new TestTree("Child_1-a"),
                    new TestTree("Child_1-b"),
                });
            var child2 = new TestTree("Child_2");
            child2.Children.AddRange(new[]
                {
                    new TestTree("Child_2-a"),
                    new TestTree("Child_2-b"),
                    new TestTree("Child_2-c"),
                });
            var child3 = new TestTree("Child_3");
            child3.Children.AddRange(new[]
                {
                    new TestTree("Child_3-a"),
                    new TestTree("Child_3-b"),
                });

            var child3c = new TestTree("Child_3-c");
            child3c.Children.AddRange(new[]
                {
                    new TestTree("Child_3-c-1"),
                    new TestTree("Child_3-c-2"),
                    new TestTree("Child_3-c-3"),
                });

            child3.Children.Add(child3c);
            child3.Children.Add(new TestTree("Child_3-d"));

            testTree.Children.Add(child1);
            testTree.Children.Add(child2);
            testTree.Children.Add(child3);

            var depthFirstSearchActual = testTree
                .Recursive(each => each.Children)
                .Select(tree => tree.Value);

            var depthFirstSearchExpected = new[]
            {
                "Child_1", "Child_1-a", "Child_1-b",
                "Child_2", "Child_2-a", "Child_2-b", "Child_2-c",
                "Child_3", "Child_3-a", "Child_3-b", "Child_3-c",
                "Child_3-c-1", "Child_3-c-2", "Child_3-c-3",
                "Child_3-d"
            };

            Assert.IsTrue(depthFirstSearchActual.SequenceEqual(depthFirstSearchExpected));

            var breadthFirstSearchActual = testTree
                .Recursive(each => each.Children, GraphSearchAlgorithm.BreadthFirstSearch)
                .Select(tree => tree.Value);

            var breadthFirstSearchExpected = new[]
            {
                "Child_1", "Child_2", "Child_3",
                "Child_1-a", "Child_1-b", "Child_2-a", "Child_2-b", "Child_2-c", "Child_3-a", "Child_3-b", "Child_3-c", "Child_3-d",
                "Child_3-c-1", "Child_3-c-2", "Child_3-c-3",
            };

            Assert.IsTrue(breadthFirstSearchActual.SequenceEqual(breadthFirstSearchExpected));
        }

        [TestMethod]
        public void TakeDoWhileTest()
        {
            var testArray = new[] { 9, 3, 7, 2, 5, 4, 1, 8, 6 };
            var emptyArray = new int[] { };

            CollectionAssert.AreEqual(new[] { 9, 3, 7 }, testArray.TakeWhile(x => 2 < x).ToArray());
            CollectionAssert.AreEqual(new[] { 9, 3, 7, 2 }, testArray.TakeDoWhile(x => 2 < x).ToArray());

            CollectionAssert.AreEqual(emptyArray, testArray.TakeWhile(x => false).ToArray());
            CollectionAssert.AreEqual(new[] { 9 }, testArray.TakeDoWhile(x => false).ToArray());

            Assert.IsFalse(emptyArray.TakeWhile(x => true).Any());
            Assert.IsFalse(emptyArray.TakeDoWhile(x => true).Any());
        }

        [TestMethod]
        public void ToHashSetTest()
        {
            var actual1 = new[] { 1, 2, 3, 2 }.ToHashSet();
            var expected1 = new HashSet<int>(new[] { 1, 2, 3 });

            Assert.IsTrue(actual1.SequenceEqual(expected1));
        }

        /// <summary>
        /// <see cref="DistinctTest"/>で使用するDTOです。
        /// </summary>
        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public override string ToString()
            {
                return string.Format("{0} : {1}", Name, Age);
            }
        }

        /// <summary>
        /// <see cref="RecursiveTest"/>で使用するDTOです。
        /// </summary>
        private class TestTree : IEnumerable<TestTree>
        {
            public List<TestTree> Children { get; private set; }

            public string Value { get; set; }

            public TestTree(string value)
            {
                Value = value;
                Children = new List<TestTree>();
            }

            public IEnumerator<TestTree> GetEnumerator()
            {
                return Children.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}