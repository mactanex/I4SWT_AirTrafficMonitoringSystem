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

            //Unit Under Test
            var uut = new SeperationLogger(fakeFileWriter);

            //ExpectedString
            string expectedResult = "\r\nLog Entry: " + "14:30:40" + "\r\n" + "tags involved: " + "AU1234" + " : " + "UA3421\r\n";
           

            //act
            uut.LogSeperation(fakeSeperation);
            fakeFileWriter.Received().Write(expectedResult);
        }
        //assert



    }
}
