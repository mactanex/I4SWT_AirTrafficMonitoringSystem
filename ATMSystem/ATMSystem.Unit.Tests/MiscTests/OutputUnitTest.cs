using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using NUnit.Framework;
using ATMSystem.Misc;
using NSubstitute;

namespace ATMSystem.Unit.Tests.MiscTests
{
    [TestFixture]
    public class OutputUnitTest
    {
        [SetUp]
        public void SetUpConsole()
        {
            //this needs to run in order to perform independent tests.
            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
        }

        [Test]
        public void WriteToOutputCalled_ConsoleOutputsCorrect()
        {
            //using stringwriter we redirect the Console, so we can assert on it
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                //arrange
                var uut = new Output();

                var fakeCoord = Substitute.For<ICoordinate>();
                fakeCoord.x = 3;
                fakeCoord.y = 6;

                var fakeTrack = Substitute.For<ITrack>();
                fakeTrack.Tag = "HY7654";
                fakeTrack.CurrentPosition = fakeCoord;
                fakeTrack.CurrentAltitude = 500;
                fakeTrack.CurrentHorizontalVelocity = 23;
                fakeTrack.CurrentCompassCourse = 45;
                

                //act
                uut.WriteToOutput(fakeTrack);

                //assert
                string expected = "Tag: HY7654\r\n" + "Current Position: (3,6) meters\r\n" + "Current Altitude: 500 meters\r\n" +
                                  "Current Horizontal Velocity: 23 m/s\r\n" + "Current Compass Course: 45 degrees\r\n";
               
                Assert.AreEqual(expected, sw.ToString());
            }
        }

    }
}
