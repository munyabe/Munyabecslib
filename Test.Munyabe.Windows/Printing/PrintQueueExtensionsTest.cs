using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;
using Munyabe.Windows.Printing;

namespace Test.Munyabe.Windows.Printing
{
    /// <summary>
    /// <see cref="PrintQueueExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class PrintQueueExtensionsTest
    {
        [TestMethod]
        public void IsXpsDocumentWriterTest()
        {
            var server = new PrintServer();
            PrinterSettings.InstalledPrinters
                .Cast<string>()
                .Select(name => new PrintQueue(server, name))
                .ForEach(queue =>
                {
                    if (queue.QueueDriver.Name == "Microsoft XPS Document Writer")
                    {
                        Assert.IsTrue(queue.IsXpsDocumentWriter());
                    }
                    else
                    {
                        Assert.IsFalse(queue.IsXpsDocumentWriter());
                    }
                });
        }
    }
}
