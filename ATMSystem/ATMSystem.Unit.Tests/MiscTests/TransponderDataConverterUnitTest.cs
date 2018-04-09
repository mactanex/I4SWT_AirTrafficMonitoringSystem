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
        public void GetTrack_RawDataInputIsCorrect_TimeStampMatches()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            string rawInput = "ATR423;39045;12932;14000;20151006213456789";

            DateTime TARGET_DATETIME = new DateTime(2015, 10, 6, 21, 34, 56, 789);
            var RESULT_DATETIME = uut.GetTrack(rawInput).LastSeen;

            Assert.That(RESULT_DATETIME.CompareTo(TARGET_DATETIME), Is.EqualTo(0));
        }

        [Test]
        public void GetTrack_RawDataFormatHasTooManyValues_ReturnsNull()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            // Wrong input
            string rawInput = "ThisIs;1400;32200;023;20151006213456789;51000";

            var TARGET_TRACK = uut.GetTrack(rawInput);

            Assert.That(TARGET_TRACK == null, Is.EqualTo(true));
        }

        [Test]
        public void GetTrack_RawDataFormatHasWrongValues_ReturnsNull()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            // Wrong input
            string rawInput = "ThisIs;1400;A;023;20151006213456789";

            var TARGET_TRACK = uut.GetTrack(rawInput);

            Assert.That(TARGET_TRACK == null, Is.EqualTo(true));
        }

        [Test]
        public void GetTrack_RawDataInputIsCorrect_TrackHasGivenCoordinate()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            string rawInput = "ATR423;39045;12932;14000;20151006213456789";

            var TARGET_COORDINATE = new Coordinate() {x = 39045, y = 12932};

            var RESULT_COORDINATE = uut.GetTrack(rawInput)?.CurrentPosition;

            Assert.That(RESULT_COORDINATE?.x == TARGET_COORDINATE.x, Is.EqualTo(true));
            Assert.That(RESULT_COORDINATE?.y == TARGET_COORDINATE.y, Is.EqualTo(true));
        }

        [Test]
        public void GetTrack_RawDataInputIsCorrect_TrackHasGivenAltitude()
        {
            TransponderDataConverter uut = new TransponderDataConverter();

            string rawInput = "ATR423;39045;12932;14000;20151006213456789";

            var TARGET_ALTITUDE = 14000;
            var RESULT_ALTITUDE = uut.GetTrack(rawInput)?.CurrentAltitude;

            Assert.That(RESULT_ALTITUDE == TARGET_ALTITUDE, Is.EqualTo(true));
        }
    }
}