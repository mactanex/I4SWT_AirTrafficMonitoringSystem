using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using ATMSystem.Misc;
using ATMSystem.Objects;
using NSubstitute;
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
            Assert.That(uut.LastSeen, Is.TypeOf<DateTime>());
            Assert.That(uut.Tag,Is.EqualTo("AF1234"));
        }


        [Test]
        public void TrackCreated_TagPassedAsOnlyParameter_AttributesHaveCorrectValues()
        {
            var uut = new Track("AF1234");
            Assert.That(uut.Tag, Is.EqualTo("AF1234"));
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(0));
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(0));
            Assert.That(uut.LastSeen, Is.TypeOf<DateTime>());
            Assert.That(uut.CurrentPosition,Is.TypeOf<Coordinate>());
        }

        [Test]
        public void TrackCreated_DefaultConstructor_AttributesHaveCorrectValues()
        {
            var uut = new Track();
            Assert.That(uut.Tag, Is.EqualTo("UNSET"));
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(0));
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(0));
            Assert.That(uut.LastSeen, Is.TypeOf<DateTime>());
            Assert.That(uut.CurrentPosition, Is.TypeOf<Coordinate>());
            Assert.That(uut.DirectionCalc,Is.TypeOf<DirectionCalc>());
        }

        [Test]
        public void UpdateCalled_CurrentPositionChanged()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 15;
            coord.y = 20;
            var date = new DateTime().Date;
            //Act
            uut.Update(coord, 500, date);
            //Assert
            Assert.That(uut.CurrentPosition, Is.EqualTo(coord));
        }

        [Test]
        public void UpdateCalled_CurrentHorizontalVelocityIsCorrect()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 15;
            coord.y = 20;
            Thread.Sleep(15000);
            var date = DateTime.Now;
            //Act
            uut.Update(coord, 500, date);
            //Assert
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(1));
        }

        [Test]
        public void UpdateCalled_LastSeenIsUpdated()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 7;
            coord.y = 2;
            Thread.Sleep(1000);
            var date = DateTime.Now;
            //Act
            uut.Update(coord, 500, date);
            //Assert
            Assert.That(uut.LastSeen, Is.EqualTo(date));
        }

        [Test]
        public void UpdateCalled_LastKnownPositionIsCorrent()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 1;
            coord.y = 1;
            Thread.Sleep(45);
            var date = DateTime.Now;
            //Act
            uut.Update(coord, 500, date);
            //Assert
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(0));
        }

        [Test]
        public void UpdateCalled_CurrentCompassCourseSetCorrectly()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 1;
            coord.y = 0;
            Thread.Sleep(1000);
            var date = DateTime.Now;
            //Act
            uut.Update(coord, 500, date);
            //Assert
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(90));
        }

        [Test]
        public void UpdateCalled_AltitudeUpdatedCorrectly()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 1;
            coord.y = 0;
            Thread.Sleep(1000);
            var date = DateTime.Now;
            //Act
            uut.Update(coord, 5000, date);
            //Assert
            Assert.That(uut.CurrentAltitude, Is.EqualTo(5000));
        }

        [Test]
        public void UpdateCalledTwiceInOneSecond_ExceptionIsHandled_HorizontalVelocityIsAsExpected()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 500;
            coord.y = 30;
            Thread.Sleep(1000);
            var date = DateTime.Now;
            //Act/
            uut.Update(coord, 5000, date);
            uut.Update(coord, 5000, date);

            //Assert
            Assert.That(uut.CurrentHorizontalVelocity,Is.EqualTo(500));
        }


    }
}
