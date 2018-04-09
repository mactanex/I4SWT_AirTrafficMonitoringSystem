using System;
using ATMSystem.Interfaces;
using ATMSystem.Misc;
using NUnit.Framework;
using ATMSystem.Objects;

namespace ATMSystem.Unit.Tests.MiscTests
{
    [TestFixture]
    public class TransponderDataConverterUnitTest
    {

        [Test]
        public void GetTrack_RawDataIsCorrect_TimeStampMatches()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            // Correct input
            string correct_rawdata = "ATR423;39045;12932;14000;20151006213456789";

            DateTime TARGET_DATETIME = new DateTime(2015, 10, 6, 21, 34, 56, 789);
            var RESULT_DATETIME = uut.GetTrack(correct_rawdata).LastSeen;

            Assert.That(RESULT_DATETIME.CompareTo(TARGET_DATETIME), Is.EqualTo(0));
        }

        [Test]
        public void GetTrack_RawDataHasTooManyValues_ReturnsNull()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            // Wrong input
            string wrong_rawdata = "ATR423;A;12932;14000;20151006213456789;51000";

            var TARGET_TRACK = uut.GetTrack(wrong_rawdata);

            Assert.That(TARGET_TRACK == null, Is.EqualTo(true));
        }

        // Wrong input
        [TestCase("ATR423;A;12932;14000;20151006213456789")]
        [TestCase("ATR423;39045;A;14000;20151006213456789")]
        public void GetTrack_RawDataHasWrongFormatPositionValue_ReturnsNull(string wrong_rawdata)
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            var TARGET_TRACK = uut.GetTrack(wrong_rawdata);

            Assert.That(TARGET_TRACK == null, Is.EqualTo(true));
        }

        [Test]
        public void GetTrack_RawDataHasWrongFormatTimeStamp_ReturnsNull()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            // Wrong input
            string wrong_rawdata = "ATR423;39045;12932;14000;20151006213A56789";

            var TARGET_TRACK = uut.GetTrack(wrong_rawdata);

            Assert.That(TARGET_TRACK == null, Is.EqualTo(true));
        }

        [Test]
        public void GetTrack_RawDataIsCorrect_TrackHasGivenCoordinate()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            // Correct input
            string correct_rawdata = "ATR423;39045;12932;14000;20151006213456789";

            var TARGET_COORDINATE = new Coordinate() {x = 39045, y = 12932};

            var RESULT_COORDINATE = uut.GetTrack(correct_rawdata)?.CurrentPosition;

            Assert.That(RESULT_COORDINATE?.x == TARGET_COORDINATE.x, Is.EqualTo(true));
            Assert.That(RESULT_COORDINATE?.y == TARGET_COORDINATE.y, Is.EqualTo(true));
        }

        [Test]
        public void GetTrack_RawDataIsCorrect_TrackHasGivenAltitude()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            // Correct input
            string correct_rawdata = "ATR423;39045;12932;14000;20151006213456789";

            var TARGET_ALTITUDE = 14000;
            var RESULT_ALTITUDE = uut.GetTrack(correct_rawdata)?.CurrentAltitude;

            Assert.That(RESULT_ALTITUDE == TARGET_ALTITUDE, Is.EqualTo(true));
        }
    }
}