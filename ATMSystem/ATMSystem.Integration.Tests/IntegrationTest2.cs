using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class IntegrationTest2
    {
        private ITrackController _trackController;
        private IMapDrawer _mapDrawer;
        private ITransponderDataConverter _transponderDataConverter;
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
            nEventsRaised = 0;

            _trackController.OnTrackUpdated += delegate { nEventsRaised++; };
        }


        [Test]
        public void TransponderDataReady_AddedTrack_TrackNotFound_TrackInBounds_OnTrackUpdatedEventRaisedOnce()
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
            Assert.That(nEventsRaised, Is.EqualTo(1));
        }

        [Test]
        public void TransponderDataReady_AddedTrack_TrackNotFound_TrackOutOfBounds_OnTrackUpdatedEventNotRaised()
        {
            //arrange
            var inputStrings = new List<string>
            {
                "ATR423;" +
                "39045;" +
                "12932;" +
                "200;" +
                "20151006213456789"
            };
            var args = new RawTransponderDataEventArgs(inputStrings);

            //act
            _transponderReceiver.TransponderDataReady += Raise.EventWith(_transponderReceiver, args);

            //assert
            Assert.That(nEventsRaised, Is.EqualTo(0));
        }
    }
}

