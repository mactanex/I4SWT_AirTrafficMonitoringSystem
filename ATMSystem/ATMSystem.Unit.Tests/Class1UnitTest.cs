using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ATMSystem.Unit.Tests
{
    [TestFixture]
    public class Class1UnitTest
    {
        
        [Test]
        public void Add2And2Returns4()
        {
            Class1 uut = new Class1();
            var result = uut.Add(2, 2);
            Assert.That(result,Is.EqualTo(4));
        }

    }
}
