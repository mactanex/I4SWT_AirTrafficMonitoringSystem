using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using ATMSystem.Objects;
using NUnit.Framework;

namespace ATMSystem.Unit.Tests.ObjectsTests
{
    [TestFixture]
    class TrackUnitTest
    {
        [Test]
        public void TrackCreated_TagAndCoordinatePassedAsParameter_AttributesHaveCorrectValues()
        {
            var startCoords = new Coordinate() {x = 1, y = 5};
            var uut = new Track("AF1234", startCoords);
            Assert.That(uut.CurrentPosition, Is.EqualTo(startCoords));
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(0));
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(0));
            Assert.That(uut.Tag,Is.EqualTo("AF1234"));
        }


        [Test]
        public void TrackCreated_TagPassedAsOnlyParameter_AttributesHaveCorrectValues()
        {
            var startCoords = new Coordinate() { x = 1, y = 5 };
            var uut = new Track("AF1234");
            Assert.That(uut.CurrentPosition, Is.TypeOf<ICoordinate>());
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(0));
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(0));
            Assert.That(uut.Tag, Is.EqualTo("AF1234"));
        }

        [Test]
        public void TrackCreated_TagPassedAsOnlyParameter_CurrentHorizontalVelocityIs0()
        {
            var startCoords = new Coordinate() { x = 1, y = 5 };
            var uut = new Track("AF1234", startCoords);
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(0));
        }

    }
}
