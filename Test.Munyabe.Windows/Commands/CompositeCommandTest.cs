using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Windows.Commands;

namespace Test.Munyabe.Windows.Commands
{
    /// <summary>
    /// <see cref="CompositeCommand"/>と<see cref="CompositeCommand{T}"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class CompositeCommandTest
    {
        private const string HELLO = "Hello !! ";
        private const string MY_NAME = "Ichiro";
        private DelegateCommand _helloCommand;
        private DelegateCommand<string> _addNameCommand;
        private string _message;
        bool _canHelloExecute;
        bool _canAddNameExecute;

        [TestInitialize]
        public void ClassInitialize()
        {
            _helloCommand = new DelegateCommand(
                () =>
                {
                    _message += HELLO;
                },
                () => _canHelloExecute);

            _addNameCommand = new DelegateCommand<string>(
                param =>
                {
                    _message += param;
                },
                param => _canAddNameExecute);
        }

        [TestMethod]
        public void ExecuteCommandTest()
        {
            _message = null;

            var compositeCommand = new CompositeCommand();

            compositeCommand.RegisterCommand(_helloCommand);
            compositeCommand.Execute();
            Assert.AreEqual(HELLO, _message);

            _message = null;

            compositeCommand.RegisterCommand(_addNameCommand);
            compositeCommand.Execute(MY_NAME);
            Assert.AreEqual(HELLO + MY_NAME, _message);
        }

        [TestMethod]
        public void ExecuteAssignParameterTypeCommandTest()
        {
            _message = null;

            var compositeCommand = new CompositeCommand<string>();
            compositeCommand.RegisterCommand(_addNameCommand);
            compositeCommand.Execute(MY_NAME);
            Assert.AreEqual(MY_NAME, _message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotRegisterDifferentParameterTypeCommandTest()
        {
            var compositeCommand = new CompositeCommand<string>();
            compositeCommand.RegisterCommand(_helloCommand);
        }

        [TestMethod]
        public void CannotExecuteCommandTest()
        {
            _message = null;
            _canHelloExecute = true;
            _canAddNameExecute = false;

            var compositeCommand = new CompositeCommand();
            compositeCommand.RegisterCommand(_helloCommand);
            compositeCommand.RegisterCommand(_addNameCommand);

            if (compositeCommand.CanExecute(MY_NAME))
            {
                compositeCommand.Execute(MY_NAME);
            }
            Assert.IsNull(_message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotRegisterCompositeCommandInItselfTest()
        {
            var compositeCommand = new CompositeCommand();
            compositeCommand.RegisterCommand(_helloCommand);
            compositeCommand.RegisterCommand(compositeCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotRegisterSameCommandTwiceTest()
        {
            var compositeCommand = new CompositeCommand();
            compositeCommand.RegisterCommand(_helloCommand);
            compositeCommand.RegisterCommand(_helloCommand);
        }
    }
}