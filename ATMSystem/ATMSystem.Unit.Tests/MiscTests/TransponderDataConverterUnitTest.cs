using System;
using ATMSystem.Misc;
using NUnit.Framework;
using ATMSystem.Objects;

namespace ATMSystem.Unit.Tests.MiscTests
{
    [TestFixture]
    public class TransponderDataConverterUnitTest
    {
        private TransponderDataConverter _uut;

        [SetUp]
        public void SetUp()
        {
            _uut = new TransponderDataConverter();
        }

        [Test]
        public void GetTimeStamp_RawDataInputIsCorrect_TimeStampReturnedCorrectly()
        {
            string rawInput = "ATR423;39045;12932;14000;20151006213456789";
            DateTime target = new DateTime(2015, 10, 6, 21, 34, 56, 789);
            DateTime result = _uut.GetTimeStamp(rawInput);
            Assert.That(result.CompareTo(target), Is.EqualTo(0));
        }

        [Test]
        public void GetTimeStamp_RawDataInputIsIncorrect_ThrowsArgumentException()
        {
            // Wrong input
            string rawInput = "ThisIs;A;412376;023;;;11;;2da;input";

            Assert.Throws<ArgumentException>(() =>
            {
                var result = _uut.GetTimeStamp(rawInput);
            });
        }

        [Test]
        public void GetTrack_RawDataInputIsCorrect_ReturnedTrackIsCorrect()
        {
            string rawInput = "ATR423;39045;12932;14000;20151006213456789";

            var target = new Track("ATR423")
            {
                CurrentAltitude = 14000,
                CurrentPosition = new Coordinate() { x = 39045, y = 12932}
            };

            var result = _uut.GetTrack(rawInput);

            Assert.That(result == target, Is.EqualTo(true));
        }
    }
}