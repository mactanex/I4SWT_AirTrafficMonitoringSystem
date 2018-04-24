using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATMSystem.Handlers;
using ATMSystem.Interfaces;
using ATMSystem.Objects;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using TransponderReceiver;

namespace ATMSystem.Unit.Tests.HandlerTests
{
    [TestFixture]
    public class SeparationMonitorUnitTest
    {
        private ISeparationMonitor _uut;
        private ITrackController _fakeTrackController;
       
        private int nEventsRaised;

        [SetUp]
        public void SetUp()
        {

            _fakeTrackController = Substitute.For<ITrackController>();

            _uut = new SeparationMonitor(_fakeTrackController);
            _uut.OnSeparationEvent += delegate(object sender, SeparationEventArgs e) { nEventsRaised++; };


            nEventsRaised = 0;
        }

        // Altitude1, X1, Y1, altitude2, x2, y2      nEventsRaised
        [TestCase(2400, 40000, 40000, 2699, 40000, 40000, 1)] //Delta Altitude testing = 299 
        [TestCase(2400, 40000, 40000, 2701, 40000, 40000, 0)] //Delta Altitude testing = 301
        [TestCase(2400, 40000, 40000, 2700, 40000, 40000, 0)] //Delta Altitude testing = 300
        [TestCase(2400, 20000, 39999, 2400, 20000, 35000, 1)] //Delta Position testing = 4999
        [TestCase(2400, 20000, 40000, 2400, 20000, 35000, 0)] //Delta Position testing = 5000
        [TestCase(2400, 40000, 40001, 2400, 20000, 35000, 0)] //Delta Position testing = 5001
        public void TrackDataHandler_AddsTwoTracks_SeperationEventRaisedCorrectly(int altitude1, int x1, int y1, int altitude2,
            int x2, int y2, int nrEventsRaised)
        {
            // Arrange
            var trackOne = Substitute.For<ITrack>();
            trackOne.Tag = "HelloTag";
            trackOne.CurrentAltitude = altitude1;
            trackOne.CurrentPosition = new Coordinate() {x = x1, y = y1};

            var trackTwo = Substitute.For<ITrack>();
            trackTwo.Tag = "GoodbyeTag";
            trackTwo.CurrentAltitude = altitude2;
            trackTwo.CurrentPosition = new Coordinate() {x = x2, y = y2};

            var returnedTracks = new Dictionary<string, ITrack>();
            returnedTracks.Add(trackOne.Tag, trackOne);
            returnedTracks.Add(trackTwo.Tag, trackTwo);

            _fakeTrackController.Tracks.Returns(returnedTracks);

            // Act

            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackOne.Tag));
            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackTwo.Tag));

            // Assert
            Assert.That(nEventsRaised == nrEventsRaised, Is.EqualTo(true));
        }

        [Test]
        public void TrackDataHandler_AddTwoTracksInConflictAndUpdateOne_ConflictIsResolved_SeperationIsRemoved()
        {
            // Arrange (Conflicting flights)
            var trackOne = Substitute.For<ITrack>();
            trackOne.Tag = "HelloTag";
            trackOne.CurrentAltitude = 500;
            trackOne.CurrentPosition = new Coordinate() { x = 15000, y = 25000 };

            var trackTwo = Substitute.For<ITrack>();
            trackTwo.Tag = "GoodbyeTag";
            trackTwo.CurrentAltitude = 799;
            trackTwo.CurrentPosition = new Coordinate() { x = 14500, y = 23900 };

            var returnedTracks = new Dictionary<string, ITrack>();
            returnedTracks.Add(trackOne.Tag, trackOne);
            returnedTracks.Add(trackTwo.Tag, trackTwo);

            _fakeTrackController.Tracks.Returns(returnedTracks);

            // Act

            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackOne.Tag));
            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackTwo.Tag));

            // Resolve the conflict
            trackTwo.CurrentAltitude = 15000;

            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackTwo.Tag));

            // Assert
            Assert.That(_uut.Separations.Count == 0, Is.EqualTo(true));
        }

        [Test]
        public void TrackDataHandler_AddTwoTracksInConflictAndUpdateOne_ConflictIsNotResolved_SeperationStays()
        {
            // Arrange (Conflicting flights)
            var trackOne = Substitute.For<ITrack>();
            trackOne.Tag = "HelloTag";
            trackOne.CurrentAltitude = 500;
            trackOne.CurrentPosition = new Coordinate() { x = 15000, y = 25000 };

            var trackTwo = Substitute.For<ITrack>();
            trackTwo.Tag = "GoodbyeTag";
            trackTwo.CurrentAltitude = 799;
            trackTwo.CurrentPosition = new Coordinate() { x = 14500, y = 23900 };

            var returnedTracks = new Dictionary<string, ITrack>();
            returnedTracks.Add(trackOne.Tag, trackOne);
            returnedTracks.Add(trackTwo.Tag, trackTwo);

            _fakeTrackController.Tracks.Returns(returnedTracks);

            // Act

            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackOne.Tag));

            // Still in conflict
            trackTwo.CurrentAltitude = 600;

            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackTwo.Tag));

            // Assert
            Assert.That(_uut.Separations.Count == 1, Is.EqualTo(true));
        }

        [Test]
        public void TrackDataHandler_AddTwoTracksInConflictAndAddOneMoreInConflictWithBoth_SeperationsCountIsThree()
        {
            // Arrange (Conflicting flights)
            var trackOne = Substitute.For<ITrack>();
            trackOne.Tag = "HelloTag";
            trackOne.CurrentAltitude = 500;
            trackOne.CurrentPosition = new Coordinate() { x = 15000, y = 25000 };

            var trackTwo = Substitute.For<ITrack>();
            trackTwo.Tag = "GoodbyeTag";
            trackTwo.CurrentAltitude = 799;
            trackTwo.CurrentPosition = new Coordinate() { x = 14500, y = 23900 };

            var returnedTracks = new Dictionary<string, ITrack>();
            returnedTracks.Add(trackOne.Tag, trackOne);
            returnedTracks.Add(trackTwo.Tag, trackTwo);

            _fakeTrackController.Tracks.Returns(returnedTracks);

            // Act
            
            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackTwo.Tag));

            var trackThree = Substitute.For<ITrack>();
            trackThree.Tag = "GoodmorningTag";
            trackThree.CurrentAltitude = 650;
            trackThree.CurrentPosition = new Coordinate() { x = 16000, y = 24100 };

            returnedTracks.Add(trackThree.Tag, trackThree);
            _fakeTrackController.OnTrackUpdated += Raise.EventWith(null, new TrackControllerEventArgs(trackThree.Tag));

            // Assert
            Assert.That(_uut.Separations.Count, Is.EqualTo(3));
        }




    }
}
