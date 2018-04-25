using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATMSystem.Handlers;
using ATMSystem.Interfaces;
using ATMSystem.Misc;
using NSubstitute;
using NUnit.Framework;
using TransponderReceiver;

namespace ATMSystem.Integration.Tests
{
    [TestFixture]
    public class IntegrationTest3
    {
        private ITrackController _trackController;
        private IMapDrawer _mapDrawer;
        private ITransponderDataConverter _transponderDataConverter;
        private ISeparationMonitor _separationMonitor;
        private ITransponderReceiver _transponderReceiver;

        private int nEventsRaised;

        [SetUp]
        public void SetUp()
        {
            //fakes
            _mapDrawer = Substitute.For<IMapDrawer>();
            _transponderReceiver = Substitute.For<ITransponderReceiver>();

            //included
            _transponderDataConverter = new TransponderDataConverter();
            _trackController = new TrackController(_transponderReceiver, _transponderDataConverter, _mapDrawer);
            _separationMonitor = new SeparationMonitor(_trackController);

            _separationMonitor.OnSeparationEvent += delegate { nEventsRaised++; }; 

            nEventsRaised = 0;
        }


        [Test]
        public void TransponderDataReady_AddedOneTrack_OnSeparationEventRaised0Times()
        {
            //arrange
            var inputStrings = new List<string>
            {
                "ATR423;" +
                "39045;" +
                "12932;" +
                "14000;" +
                "20151006213456789"
            };
            var args = new RawTransponderDataEventArgs(inputStrings);

            //act

            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args);


            //assert
            Assert.That(nEventsRaised, Is.EqualTo(0));
        }

        [Test]
        public void TransponderDataReady_AddedTwoTracksNoConflict_OnSeparationEventRaised0Times()
        {
            //arrange
            var track1 = new List<string>
            {
                "ATR423;" +
                "39045;" +
                "12932;" +
                "14000;" +
                "20151006213456789"
            };
            var args1 = new RawTransponderDataEventArgs(track1);

            var track2 = new List<string>
            {
                "KOL543;" +
                "80000;" +
                "45000;" +
                "9000;" +
                "20151006213456789"
            };
            var args2 = new RawTransponderDataEventArgs(track2);


            //act

            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args1);
            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args2);

            //assert
            Assert.That(nEventsRaised, Is.EqualTo(0));
        }

        [Test]
        public void TransponderDataReady_AddedTwoTracksInConflict_OnSeparationEventRaised1Time()
        {
            //arrange
            var track1 = new List<string>
            {
                "ATR423;" +
                "39045;" +
                "12932;" +
                "14000;" +
                "20151006213456789"
            };
            var args1 = new RawTransponderDataEventArgs(track1);

            var track2 = new List<string>
            {
                "RTA322;" +
                "40000;" +
                "13000;" +
                "14100;" +
                "20151006213456789"
            };
            var args2 = new RawTransponderDataEventArgs(track2);


            //act

            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args1);
            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args2);

            //assert
            Assert.That(nEventsRaised, Is.EqualTo(1));
        }

        [Test]
        public void TransponderDataReady_AddedTwoTracksInConflict_ConflictResolved_OnSeparationEventRaised2Times()
        {
            //arrange
            var track1 = new List<string>
            {
                "ATR423;" +
                "39045;" +
                "12932;" +
                "14000;" +
                "20151006213456789"
            };
            var args1 = new RawTransponderDataEventArgs(track1);

            var track2 = new List<string>
            {
                "RTA322;" +
                "40000;" +
                "13000;" +
                "14100;" +
                "20151006213456789"
            };
            var args2 = new RawTransponderDataEventArgs(track2);


            //act

            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args1);
            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args2);

            track2 = new List<string>
            {
                "RTA322;" +
                "40000;" +
                "13000;" +
                "9000;" +
                "20151006213456789"
            };
            args2 = new RawTransponderDataEventArgs(track2);
            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args2);
            //assert
            Assert.That(nEventsRaised, Is.EqualTo(2));
        }

    }
}