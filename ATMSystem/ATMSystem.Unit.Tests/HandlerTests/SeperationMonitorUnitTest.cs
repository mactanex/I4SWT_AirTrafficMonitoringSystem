using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATMSystem.Handlers;
using ATMSystem.Interfaces;
using NSubstitute;
using NUnit.Framework;
using TransponderReceiver;

namespace ATMSystem.Unit.Tests.HandlerTests
{
    [TestFixture]
    public class SeperationMonitorUnitTest
    {
        private ISeperationMonitor _uut;
        private ITrackController _fakeTrackController;
        private ISeperationLogger _fakeSeperationLogger;

        private int nEventsRaised;

        public class FakeTrackController : ITrackController
        {
            public event EventHandler OnTrackUpdated;
            public void TransponderDataHandler(object obj, EventArgs args)
            {
                var track = obj as ITrack;
                if (_tracks.ContainsKey(track.Tag))
                    _tracks[track.Tag] = track;
                else
                    _tracks.Add(track.Tag, track);

                OnTrackUpdated.Invoke(track, EventArgs.Empty);
            }

            private readonly Dictionary<string, ITrack> _tracks = new Dictionary<string, ITrack>();
            public IReadOnlyDictionary<string, ITrack> Tracks => _tracks;
        }

        [SetUp]
        public void SetUp()
        {
            _fakeTrackController = new FakeTrackController();
            

            _fakeSeperationLogger = Substitute.For<ISeperationLogger>();
            
            _uut = new SeperationMonitor(_fakeTrackController);
            _uut.OnSeperationEvent += _fakeSeperationLogger.SeperationHandler;
            _uut.OnSeperationEvent += delegate (object sender, EventArgs e) { nEventsRaised++; };


            nEventsRaised = 0;
        }

        // Altitude1, X1, Y1, altitude2, x2, y2      nEventsRaised
        [TestCase(2400, 40000, 40000, 2699, 40000, 40000, 1)] //Delta Altitude testing = 299 
        [TestCase(2400, 40000, 40000, 2701, 40000, 40000, 0)] //Delta Altitude testing = 301
        [TestCase(2400, 40000, 40000, 2700, 40000, 40000, 0)] //Delta Altitude testing = 300
        [TestCase(2400, 20000, 39999, 2400, 20000, 35000, 1)] //Delta Position testing = 4999
        [TestCase(2400, 20000, 40000, 2400, 20000, 35000, 0)] //Delta Position testing = 5000
        [TestCase(2400, 40000, 40001, 2400, 20000, 35000, 0)] //Delta Position testing = 5001
        public void TrackDataHandler_AddsTwoTracks_SeperationEventRaised(int altitude1, int x1, int y1, int altitude2, int x2, int y2, int nrEventsRaised)
        {
            //Arrange
            var fakeTrack1 = Substitute.For<ITrack>();
            fakeTrack1.Tag = "ASD234";
            fakeTrack1.CurrentAltitude = altitude1;
            fakeTrack1.CurrentPosition.x = x1;
            fakeTrack1.CurrentPosition.y = y1;


            var fakeTrack2 = Substitute.For<ITrack>();
            fakeTrack2.Tag = "ABG234";
            fakeTrack2.CurrentAltitude = altitude2;
            fakeTrack2.CurrentPosition.x = x2;
            fakeTrack2.CurrentPosition.y = y2;
            
            //act

            _fakeTrackController.TransponderDataHandler(fakeTrack1, EventArgs.Empty);
            _fakeTrackController.TransponderDataHandler(fakeTrack2, EventArgs.Empty);
            

            //assert

            Assert.AreEqual(nEventsRaised, nrEventsRaised);
            
        }

        [Test]
        public void TrackDataHandler_AddsTwoTracks_SeperationEventRaised_UntilConflictResolved()
        {
            //Arrange
            var fakeTrack1 = Substitute.For<ITrack>();
            fakeTrack1.Tag = "ASD234";
            fakeTrack1.CurrentAltitude = 3000;
            fakeTrack1.CurrentPosition.x = 13000;
            fakeTrack1.CurrentPosition.y = 13000;


            var fakeTrack2 = Substitute.For<ITrack>();
            fakeTrack2.Tag = "ABG234";
            fakeTrack2.CurrentAltitude = 3000;
            fakeTrack2.CurrentPosition.x = 13000;
            fakeTrack2.CurrentPosition.y = 13000;

            //act

            _fakeTrackController.TransponderDataHandler(fakeTrack1, EventArgs.Empty);
            _fakeTrackController.TransponderDataHandler(fakeTrack2, EventArgs.Empty);

            Thread.Sleep(1000);

            fakeTrack2.CurrentAltitude = 3301;
            _fakeTrackController.TransponderDataHandler(fakeTrack2, EventArgs.Empty);

            //assert

            Assert.AreEqual(1, nEventsRaised);

        }

        [Test]
        public void TrackDataHandler_AddsTwoTracks_NoSeperationEventRaised()
        {
            //Arrange
            var fakeTrack1 = Substitute.For<ITrack>();
            fakeTrack1.Tag = "ASD234";
            fakeTrack1.CurrentAltitude = 3000;
            fakeTrack1.CurrentPosition.x = 13000;
            fakeTrack1.CurrentPosition.y = 13000;


            var fakeTrack2 = Substitute.For<ITrack>();
            fakeTrack2.Tag = "ABG234";
            fakeTrack2.CurrentAltitude = 6000;
            fakeTrack2.CurrentPosition.x = 13000;
            fakeTrack2.CurrentPosition.y = 23000;


            _fakeTrackController.TransponderDataHandler(fakeTrack1, EventArgs.Empty);
            _fakeTrackController.TransponderDataHandler(fakeTrack2, EventArgs.Empty);


            // _fakeTrackController.OnTrackUpdated += Raise.EventWith(fakeTrack2, new EventArgs());

            //assert

            Assert.AreEqual(0, nEventsRaised);

        }
    }
}
