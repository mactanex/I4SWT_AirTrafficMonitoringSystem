using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Misc;
using NUnit.Framework;

namespace ATMSystem.Unit.Tests.MiscTests
{
    [TestFixture]
    public class FileWriterUnitTest
    {
        [Test]
        public void Write_FileExists()
        {
            //arrange
            var uut = new FileWriter();

            //act
            uut.Write("This is only a test");
            var  path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "log.txt";

            //assert
            Assert.IsTrue(File.Exists(path));
        }
    }
}
