using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using ATMSystem.Misc;
using NSubstitute;
using NUnit.Framework;

namespace ATMSystem.Unit.Tests.MiscTests
{
    [TestFixture]
    public class SeperationLoggerUnitTest
    {
        [Test]
        public void LogSeperation_StringMatchesSeperationData()
        {
            //arrange
            var fakeFileWriter = Substitute.For<IFileWriter>();
            var fakeTrack1 = Substitute.For<ITrack>();
            fakeTrack1.Tag = "AU1234";
            var fakeTrack2 = Substitute.For<ITrack>();
            fakeTrack2.Tag = "UA3421"; 
            var fakeSeperation = Substitute.For<ISeperation>();
            fakeSeperation.TimeOfOccurence = "14:30:40";
            fakeSeperation.Track1 = fakeTrack1;
            fakeSeperation.Track2 = fakeTrack2;

            var fakeMonitor = Substitute.For<ISeperationMonitor>();

            //Unit Under Test
            var uut = new SeperationLogger(fakeFileWriter,fakeMonitor);

            //ExpectedString
            string expectedResult = "\r\nLog Entry: " + "14:30:40" + "\r\n" + "tags involved: " + "AU1234" + " : " + "UA3421\r\n";
           

            //act
            uut.LogSeperation(fakeSeperation);

            //assert
            fakeFileWriter.Received().Write(expectedResult);
        }

        [Test]
        public void SeperationHandler_StringMatchesSeperationData()
        {
            //arrange
            var fakeFileWriter = Substitute.For<IFileWriter>();
            var fakeTrack1 = Substitute.For<ITrack>();
            fakeTrack1.Tag = "LDS3F3";
            var fakeTrack2 = Substitute.For<ITrack>();
            fakeTrack2.Tag = "OU7543";
            var fakeSeperation = Substitute.For<ISeperation>();
            fakeSeperation.TimeOfOccurence = "18:30:40";
            fakeSeperation.Track1 = fakeTrack1;
            fakeSeperation.Track2 = fakeTrack2;

            var fakeMonitor = Substitute.For<ISeperationMonitor>();

            //Unit Under Test
            var uut = new SeperationLogger(fakeFileWriter, fakeMonitor);

            //ExpectedString
            string expectedResult = "\r\nLog Entry: " + fakeSeperation.TimeOfOccurence + "\r\n" + "tags involved: " + fakeSeperation.Track1.Tag + " : " + fakeSeperation.Track2.Tag+ "\r\n";


            //act
            uut.SeperationHandler(fakeSeperation, EventArgs.Empty);

            //assert
            
            fakeFileWriter.Received().Write(expectedResult);
        }


    }
}
