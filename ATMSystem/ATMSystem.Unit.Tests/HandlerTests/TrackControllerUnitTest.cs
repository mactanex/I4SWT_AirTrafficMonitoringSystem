using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Handlers;
using ATMSystem.Interfaces;
using ATMSystem.Objects;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TransponderReceiver;

namespace ATMSystem.Unit.Tests.HandlerTests
{
    [TestFixture]
    class TrackControllerUnitTest
    {
        private IOutput _fakeOutput;
        private ITransponderDataConverter _fakeConverter;
        private ITransponderReceiver _fakeReceiver;
        private TrackController _uut;

        [SetUp]
        public void SetUp()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _fakeConverter = Substitute.For<ITransponderDataConverter>();
            _fakeReceiver = Substitute.For<ITransponderReceiver>();

            _uut = new TrackController(_fakeReceiver, _fakeConverter, _fakeOutput);
        }

        [TestCase(10000, 45000, 10000)]
        [TestCase(90000, 45000, 10000)]
        [TestCase(45000, 10000, 10000)]
        [TestCase(45000, 90000, 10000)]
        [TestCase(45000, 90000, 500)]
        [TestCase(45000, 90000, 20000)]
        public void DataHandler_NewTrackInBounds_TrackIsAdded(int x, int y, int altitude)
        {
            // Setup
            var inputStrings = new List<string>();
            inputStrings.Add("random");
            var args = new RawTransponderDataEventArgs(inputStrings);

            var track = Substitute.For<ITrack>();
            track.Tag = "TrackOne";
            track.CurrentPosition.x = x;
            track.CurrentPosition.y = y;
            track.CurrentAltitude = altitude;

            _fakeConverter.GetTrack(Arg.Any<string>()).Returns(track);

            // Act
            _uut.TransponderDataHandler(null, args);

            // Assert
            Assert.That(_uut.Tracks.ContainsKey("TrackOne"), Is.EqualTo(true));
        }

        [TestCase(5000, 45000, 10000)]
        [TestCase(9999, 45000, 10000)]
        [TestCase(90001, 45000, 10000)]
        [TestCase(45000, 5000, 10000)]
        [TestCase(45000, 9999, 10000)]
        [TestCase(45000, 90001, 10000)]
        [TestCase(45000, 45000, 499)]
        [TestCase(45000, 45000, 20001)]
        public void DataHandler_NewTrackOutOfBounds_TrackIsNotAdded(int x, int y, int altitude)
        {
            // Setup
            var inputStrings = new List<string>();
            inputStrings.Add("random");
            var args = new RawTransponderDataEventArgs(inputStrings);

            var track = Substitute.For<ITrack>();
            track.Tag = "TrackOne";
            track.CurrentPosition.x = x;
            track.CurrentPosition.y = y;
            track.CurrentAltitude = altitude;

            _fakeConverter.GetTrack(Arg.Any<string>()).Returns(track);

            // Act
            _uut.TransponderDataHandler(null, args);

            // Assert
            Assert.That(_uut.Tracks.ContainsKey("TrackOne"), Is.EqualTo(false));
        }

        [Test]
        public void DataHandler_TracksAdded_OutputWritesTracks()
        {

        }

        [Test]
        public void DataHandler_ExistingTrackInBounds_TrackIsUpdated()
        {

        }

        [Test]
        public void DataHandler_ExistingTrackOutOfBounds_TrackIsRemoved()
        {

        }


    }
}
