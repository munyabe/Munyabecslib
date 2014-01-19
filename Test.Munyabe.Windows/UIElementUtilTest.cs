using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common.Algorithms;
using Munyabe.Windows;
using Test.Munyabe.Windows.TestView;

namespace Test.Munyabe.Windows
{
    /// <summary>
    /// <see cref="UIElementUtil"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class UIElementUtilTest
    {
        private Visual _mainGrid;

        [TestInitialize]
        public void Initialize()
        {
            var view = new UIElementUtilTestView();
            _mainGrid = view.MainGrid;
        }

        [TestMethod]
        public void FindLogicalChildrenTest()
        {
            Assert.IsFalse(UIElementUtil.FindLogicalChildren<Window>(_mainGrid).Any());

            var actual = UIElementUtil.FindLogicalChildren<TextBox>(_mainGrid);
            var expected = new List<string> { "Suzuki", "SubView", "Ichiro" };

            CollectionAssert.AreEqual(expected, actual.Select(each => each.Text).ToList());
        }

        [TestMethod]
        public void FindLogicalChildrenDepthSearchAllTest()
        {
            var actual = UIElementUtil.FindLogicalChildren<DependencyObject>(_mainGrid);
            var expected = new List<Type>
            {
                typeof(StackPanel),
                typeof(TextBlock),
                typeof(TextBox),
                typeof(ContentControl),
                typeof(UIElementUtilTestSubView),
                typeof(Grid),
                typeof(TextBox),
                typeof(StackPanel),
                typeof(TextBlock),
                typeof(TextBox)
            };

            CollectionAssert.AreEqual(expected, actual.Select(each => each.GetType()).ToList());
        }

        [TestMethod]
        public void FindLogicalChildrenBreadthSearchAllTest()
        {
            var actual = UIElementUtil.FindLogicalChildren<DependencyObject>(_mainGrid, GraphSearchAlgorithm.BreadthFirstSearch);
            var expected = new List<Type>
            {
                typeof(StackPanel),
                typeof(StackPanel),
                typeof(TextBlock),
                typeof(TextBox),
                typeof(ContentControl),
                typeof(TextBlock),
                typeof(TextBox),
                typeof(UIElementUtilTestSubView),
                typeof(Grid),
                typeof(TextBox),
            };

            CollectionAssert.AreEqual(expected, actual.Select(each => each.GetType()).ToList());
        }

        [TestMethod]
        public void FindVisualChildrenTest()
        {
            Assert.IsFalse(UIElementUtil.FindVisualChildren<Window>(_mainGrid).Any());

            var actual = UIElementUtil.FindVisualChildren<TextBox>(_mainGrid);
            var expected = new List<string> { "Suzuki", "Ichiro" };

            CollectionAssert.AreEqual(expected, actual.Select(each => each.Text).ToList());
        }

        [TestMethod]
        public void FindVisualChildrenDepthSearchAllTest()
        {
            var actual = UIElementUtil.FindVisualChildren<Visual>(_mainGrid);
            var expected = new List<Type>
            {
                typeof(StackPanel),
                typeof(TextBlock),
                typeof(TextBox),
                typeof(ContentControl),
                typeof(StackPanel),
                typeof(TextBlock),
                typeof(TextBox)
            };

            CollectionAssert.AreEqual(expected, actual.Select(each => each.GetType()).ToList());
        }

        [TestMethod]
        public void FindVisualChildrenBreadthSearchAllTest()
        {
            var actual = UIElementUtil.FindVisualChildren<Visual>(_mainGrid, GraphSearchAlgorithm.BreadthFirstSearch);
            var expected = new List<Type>
            {
                typeof(StackPanel),
                typeof(StackPanel),
                typeof(TextBlock),
                typeof(TextBox),
                typeof(ContentControl),
                typeof(TextBlock),
                typeof(TextBox)
            };

            CollectionAssert.AreEqual(expected, actual.Select(each => each.GetType()).ToList());
        }

        [TestMethod]
        public void GetVisualTreeChildrenTest()
        {
            var actual = UIElementUtil.GetVisualTreeChildren(_mainGrid);
            var expected = new List<Type> { typeof(StackPanel), typeof(StackPanel) };

            CollectionAssert.AreEqual(expected, actual.Select(each => each.GetType()).ToList());
        }
    }
}