using System;
using System.Collections.Generic;
using System.Linq;
using ATMSystem.Handlers;
using ATMSystem.Interfaces;
using ATMSystem.Misc;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TransponderReceiver;

namespace ATMSystem.Integration.Tests
{
    [TestFixture]
    public class IntegrationTest1
    {
        private ITrackController _trackController;
        private IMapDrawer _mapDrawer;
        private ITransponderDataConverter _transponderDataConverter;
        private ITransponderReceiver _transponderReceiver;
        
        [SetUp]
        public void SetUp()
        {
            //fakes
            _mapDrawer = Substitute.For<IMapDrawer>();
            _transponderReceiver = Substitute.For<ITransponderReceiver>();

            //included
            _transponderDataConverter = new TransponderDataConverter();
            _trackController = new TrackController(_transponderReceiver, _transponderDataConverter, _mapDrawer);
        }

        [Test]
        public void TransponderDataReadyEventRaised_TransponderDataConverter_ReturnsCorrectTrack()
        {
            //arrange
            var fakeTrack = Substitute.For<ITrack>();
            fakeTrack.Tag = "ATR423";
            fakeTrack.CurrentPosition.x = 39045;
            fakeTrack.CurrentPosition.y = 12932;
            fakeTrack.CurrentAltitude = 14000;
            fakeTrack.LastSeen = new DateTime(2015,10,6,21,34,56,789);
           
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

            //Assert
            Assert.That(_trackController.Tracks.First().Key, Is.EqualTo(fakeTrack.Tag));
            Assert.That(_trackController.Tracks.First().Value.CurrentPosition.x, Is.EqualTo(fakeTrack.CurrentPosition.x));
            Assert.That(_trackController.Tracks.First().Value.CurrentPosition.y, Is.EqualTo(fakeTrack.CurrentPosition.y));
            Assert.That(_trackController.Tracks.First().Value.CurrentAltitude, Is.EqualTo(fakeTrack.CurrentAltitude));
            Assert.That(_trackController.Tracks.First().Value.LastSeen, Is.EqualTo(fakeTrack.LastSeen));
        }

    }
}