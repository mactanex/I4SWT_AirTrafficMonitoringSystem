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
        private IMapDrawer _fakeMapDrawer;
        private ITransponderDataConverter _fakeConverter;
        private ITransponderReceiver _fakeReceiver;
        private TrackController _uut;

        [SetUp]
        public void SetUp()
        {
            _fakeConverter = Substitute.For<ITransponderDataConverter>();
            _fakeReceiver = Substitute.For<ITransponderReceiver>();
            _fakeMapDrawer = Substitute.For<IMapDrawer>();

            _uut = new TrackController(_fakeReceiver, _fakeConverter, _fakeMapDrawer);
        }

        // X, Y, Z, RESULT
        [TestCase(9999, 45000, 10000, false)]
        [TestCase(10000, 45000, 10000, true)]
        [TestCase(90000, 45000, 10000, true)]
        [TestCase(90001, 90000, 10000, false)]
        [TestCase(45000, 9999, 10000, false)]
        [TestCase(45000, 10000, 10000, true)]
        [TestCase(45000, 90000, 10000, true)]
        [TestCase(45000, 90001, 10000, false)]
        [TestCase(45000, 45000, 499, false)]
        [TestCase(45000, 45000, 500, true)]
        [TestCase(45000, 45000, 20000, true)]
        [TestCase(45000, 45000, 20001, false)]
        public void DataHandler_NewTrackInOrOutOfBounds_AddedCorrectly(int x, int y, int altitude, bool result)
        {
            // Setup
            var inputStrings = new List<string>();
            inputStrings.Add("test");
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
            Assert.That(_uut.Tracks.ContainsKey("TrackOne"), Is.EqualTo(result));
        }

        [Test]
        public void DataHandler_TracksAdded_GeneratesAndDrawsMap()
        {
            // Setup
            var inputStrings = new List<string>
            {
                "test",
                "testTwo"
            };
            var args = new RawTransponderDataEventArgs(inputStrings);

            var track = Substitute.For<ITrack>();
            track.Tag = "TrackOne";
            track.CurrentPosition.x = 45000;
            track.CurrentPosition.y = 42000;
            track.CurrentAltitude = 10000;

            var trackTwo = Substitute.For<ITrack>();
            trackTwo.Tag = "TrackTwo";
            trackTwo.CurrentPosition.x = 90000;
            trackTwo.CurrentPosition.y = 32000;
            trackTwo.CurrentAltitude = 10000;

            _fakeConverter.GetTrack("test").Returns(track);
            _fakeConverter.GetTrack("testTwo").Returns(trackTwo);

            _fakeMapDrawer.GenerateMap(Arg.Any<List<ITrack>>()).Returns("Map");

            // Act
            _uut.TransponderDataHandler(null, args);

            // Assert
            Received.InOrder(() =>
            {
                _fakeMapDrawer.GenerateMap(Arg.Is<List<ITrack>>(l => l.Count == 2));
                _fakeMapDrawer.DrawMap(Arg.Is<string>(c => c != "" && c != null));
            });
        }

        // X, Y, Z, Nr of Calls
        [TestCase(9999, 45000, 10000, 0)]
        [TestCase(10000, 45000, 10000, 1)]
        [TestCase(90000, 45000, 10000, 1)]
        [TestCase(90001, 90000, 10000, 0)]
        [TestCase(45000, 9999, 10000, 0)]
        [TestCase(45000, 10000, 10000, 1)]
        [TestCase(45000, 90000, 10000, 1)]
        [TestCase(45000, 90001, 10000, 0)]
        [TestCase(45000, 45000, 499, 0)]
        [TestCase(45000, 45000, 500, 1)]
        [TestCase(45000, 45000, 20000, 1)]
        [TestCase(45000, 45000, 20001, 0)]
        public void DataHandler_ExistingTrackInOutOfBounds_CorrectTracksAreUpdated(int x, int y, int altitude, int nrOfCalls)
        {
            // Setup
            var inputStrings = new List<string>();
            inputStrings.Add("test");
            var args = new RawTransponderDataEventArgs(inputStrings);

            var track = Substitute.For<ITrack>();
            track.Tag = "TrackOne";
            track.CurrentPosition.x = 45000;
            track.CurrentPosition.y = 45000;
            track.CurrentAltitude = 10000;

            _fakeConverter.GetTrack(Arg.Any<string>()).Returns(track);

            // Act
            _uut.TransponderDataHandler(null, args);

            track.CurrentPosition.x = x;
            track.CurrentPosition.y = y;
            track.CurrentAltitude = altitude;

            _uut.TransponderDataHandler(null, args);

            // Assert
            track.Received(nrOfCalls).Update(Arg.Any<ICoordinate>(), Arg.Any<int>(), Arg.Any<DateTime>());
        }

        // X, Y, Z, Result
        [TestCase(9999, 45000, 10000, false)]
        [TestCase(10000, 45000, 10000, true)]
        [TestCase(90000, 45000, 10000, true)]
        [TestCase(90001, 90000, 10000, false)]
        [TestCase(45000, 9999, 10000, false)]
        [TestCase(45000, 10000, 10000, true)]
        [TestCase(45000, 90000, 10000, true)]
        [TestCase(45000, 90001, 10000, false)]
        [TestCase(45000, 45000, 499, false)]
        [TestCase(45000, 45000, 500, true)]
        [TestCase(45000, 45000, 20000, true)]
        [TestCase(45000, 45000, 20001, false)]
        public void DataHandler_ExistingTrackInOrOutOfBounds_CorrectTracksRemoved(int x, int y, int altitude, bool result)
        {
            // Setup
            var inputStrings = new List<string>();
            inputStrings.Add("test");
            var args = new RawTransponderDataEventArgs(inputStrings);

            var track = Substitute.For<ITrack>();
            track.Tag = "TrackOne";
            track.CurrentPosition.x = 45000;
            track.CurrentPosition.y = 45000;
            track.CurrentAltitude = 10000;

            _fakeConverter.GetTrack(Arg.Any<string>()).Returns(track);

            // Act
            _uut.TransponderDataHandler(null, args);

            track.CurrentPosition.x = x;
            track.CurrentPosition.y = y;
            track.CurrentAltitude = altitude;

            _uut.TransponderDataHandler(null, args);

            // Assert
            Assert.That(_uut.Tracks.ContainsKey("TrackOne"), Is.EqualTo(result));
        }

        [Test]
        public void DataHandler_TracksAdded_TracksUpdatedInvoked()
        {
            // Setup
            var inputStrings = new List<string>
            {
                "test",
                "testTwo"
            };
            var args = new RawTransponderDataEventArgs(inputStrings);

            var track = Substitute.For<ITrack>();
            track.Tag = "TrackOne";
            track.CurrentPosition.x = 45000;
            track.CurrentPosition.y = 42000;
            track.CurrentAltitude = 10000;

            var trackTwo = Substitute.For<ITrack>();
            trackTwo.Tag = "TrackTwo";
            trackTwo.CurrentPosition.x = 90000;
            trackTwo.CurrentPosition.y = 32000;
            trackTwo.CurrentAltitude = 10000;

            _fakeConverter.GetTrack("test").Returns(track);
            _fakeConverter.GetTrack("testTwo").Returns(trackTwo);

            int called = 0;
            _uut.OnTrackUpdated += (s, a) => { called++; };

            // Act
            _uut.TransponderDataHandler(null, args);

            // Assert
            Assert.That(called, Is.EqualTo(2));
        }

        [Test]
        public void DataHandler_TracksUpdated_TracksUpdatedInvoked()
        {
            // Setup
            var inputStrings = new List<string>
            {
                "test",
            };
            var args = new RawTransponderDataEventArgs(inputStrings);

            var track = Substitute.For<ITrack>();
            track.Tag = "TrackOne";
            track.CurrentPosition.x = 45000;
            track.CurrentPosition.y = 42000;
            track.CurrentAltitude = 10000;

            _fakeConverter.GetTrack("test").Returns(track);

            _uut.TransponderDataHandler(null, args);

            int called = 0;
            _uut.OnTrackUpdated += (s, a) => { called++; };

            // Act
            _uut.TransponderDataHandler(null, args);

            // Assert
            Assert.That(called, Is.EqualTo(1));
        }



    }
}
