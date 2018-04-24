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
        public void LogSeperation_Conflicting_StringMatchesSeperationData()
        {
            //arrange
            var fakeFileWriter = Substitute.For<IFileWriter>();
            var fakeTrack1 = Substitute.For<ITrack>();
            fakeTrack1.Tag = "AU1234";
            var fakeTrack2 = Substitute.For<ITrack>();
            fakeTrack2.Tag = "UA3421"; 
            var fakeSeperation = Substitute.For<ISeperation>();
            fakeSeperation.TimeOfOccurence = "14:30:40";
            fakeSeperation.TrackOne.Tag = fakeTrack1.Tag;
            fakeSeperation.TrackTwo.Tag = fakeTrack2.Tag;
            fakeSeperation.ConflictingSeperation = true;

            var fakeMonitor = Substitute.For<ISeperationMonitor>();

            //Unit Under Test
            var uut = new SeperationLogger(fakeFileWriter,fakeMonitor);

            //ExpectedString
            string expectedResult = "\r\nLog Entry: " + "14:30:40" + "\r\n" + "Tags involved: " + "AU1234" + " : " + "UA3421\r\nAre conflicting!\r\n";
           

            //act
            uut.LogSeperation(fakeSeperation);

            //assert
            fakeFileWriter.Received().Write(expectedResult);
        }

        [Test]
        public void LogSeperation_NotConflicting_StringMatchesSeperationData()
        {
            //arrange
            var fakeFileWriter = Substitute.For<IFileWriter>();
            var fakeTrack1 = Substitute.For<ITrack>();
            fakeTrack1.Tag = "AU1234";
            var fakeTrack2 = Substitute.For<ITrack>();
            fakeTrack2.Tag = "UA3421";
            var fakeSeperation = Substitute.For<ISeperation>();
            fakeSeperation.TimeOfOccurence = "14:30:40";
            fakeSeperation.TrackOne.Tag = fakeTrack1.Tag;
            fakeSeperation.TrackTwo.Tag = fakeTrack2.Tag;
            fakeSeperation.ConflictingSeperation = false;

            var fakeMonitor = Substitute.For<ISeperationMonitor>();

            //Unit Under Test
            var uut = new SeperationLogger(fakeFileWriter, fakeMonitor);

            //ExpectedString
            string expectedResult = "\r\nLog Entry: " + "14:30:40" + "\r\n" + "Tags involved: " + "AU1234" + " : " + "UA3421\r\nAre no longer conflicting!\r\n";


            //act
            uut.LogSeperation(fakeSeperation);

            //assert
            fakeFileWriter.Received().Write(expectedResult);
        }

        [Test]
        public void SeperationHandler_Conflicting_StringMatchesSeperationData()
        {
            //arrange
            var fakeFileWriter = Substitute.For<IFileWriter>();
            var fakeTrack1 = Substitute.For<ITrack>();
            fakeTrack1.Tag = "LDS3F3";
            var fakeTrack2 = Substitute.For<ITrack>();
            fakeTrack2.Tag = "OU7543";
            var fakeSeperation = Substitute.For<ISeperation>();
            fakeSeperation.TimeOfOccurence = "18:30:40";
            fakeSeperation.TrackOne.Tag = fakeTrack1.Tag;
            fakeSeperation.TrackTwo.Tag = fakeTrack2.Tag;
            fakeSeperation.ConflictingSeperation = true;

            var fakeMonitor = Substitute.For<ISeperationMonitor>();

            //Unit Under Test
            var uut = new SeperationLogger(fakeFileWriter, fakeMonitor);

            //ExpectedString
            string expectedResult = "\r\nLog Entry: " + fakeSeperation.TimeOfOccurence + "\r\n" + "Tags involved: " + fakeSeperation.TrackOne.Tag + " : " + fakeSeperation.TrackTwo.Tag + "\r\nAre conflicting!\r\n";


            //act
            uut.SeperationHandler(null, new SeperationEventArgs(fakeSeperation));

            //assert
            
            fakeFileWriter.Received().Write(expectedResult);
        }


    }
}
