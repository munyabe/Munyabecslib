using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Windows.Commands;

namespace Test.Munyabe.Windows.Commands
{
    /// <summary>
    /// <see cref="DelegateCommand"/>と<see cref="DelegateCommand{T}"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class DelegateCommandTest
    {
        private const string HELLO = "Hello !! ";
        private const string MY_NAME = "Ichiro";
        private DelegateCommand _helloCommand;
        private DelegateCommand<string> _helloNameCommand;
        private string _message;
        private bool _canHello;

        [TestInitialize]
        public void ClassInitialize()
        {
            _helloCommand = new DelegateCommand(
                () =>
                {
                    _message = HELLO;
                },
                () => _canHello);

            _helloNameCommand = new DelegateCommand<string>(
                param =>
                {
                    _message = HELLO + param;
                },
                param => _canHello);
        }

        [TestMethod]
        public void ExecuteCommandTest()
        {
            _message = null;

            _helloCommand.Execute();
            Assert.AreEqual(HELLO, _message);

            _helloNameCommand.Execute(MY_NAME);
            Assert.AreEqual(HELLO + MY_NAME, _message);
        }

        [TestMethod]
        public void CannotExecuteCommandTest()
        {
            _message = null;
            _canHello = false;

            if (_helloCommand.CanExecute())
            {
                _helloCommand.Execute();
            }
            Assert.IsNull(_message);

            if (_helloNameCommand.CanExecute(MY_NAME))
            {
                _helloNameCommand.Execute(MY_NAME);
            }
            Assert.IsNull(_message);
        }
    }
}