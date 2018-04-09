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
        public void TrackCreated_TagCoordinateAltitudeAndTimestampPassedAsParameter_AttributesHaveCorrectValues()
        {
            //Arrange/act
            var tag = "AF1234";
            var startCoords = Substitute.For<ICoordinate>();
            startCoords.x = 1;
            startCoords.y = 5;
            var altitude = 8000;
            var timestamp = DateTime.Now;
            //Act
            var uut = new Track(tag, startCoords, altitude, timestamp);
            //Assert
            Assert.That(uut.CurrentPosition, Is.EqualTo(startCoords));
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(0));
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(0));
            Assert.That(uut.CurrentAltitude,Is.EqualTo(altitude));
            Assert.That(uut.LastSeen, Is.EqualTo(timestamp));
            Assert.That(uut.Tag, Is.EqualTo(tag));
        }

        [Test]
        public void TrackCreated_TagAndCoordinatePassedAsParameter_AttributesHaveCorrectValues()
        {
            //Arrange
            var tag = "AF1234";
            var startCoords = Substitute.For<ICoordinate>();
            startCoords.x = 1;
            startCoords.y = 5;
            //Act
            var uut = new Track(tag, startCoords);
            //Assert
            Assert.That(uut.CurrentPosition, Is.EqualTo(startCoords));
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(0));
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(0));
            Assert.That(uut.LastSeen, Is.TypeOf<DateTime>());
            Assert.That(uut.Tag,Is.EqualTo("AF1234"));
            Assert.That(uut.DirectionCalc, Is.TypeOf<DirectionCalc>());
        }


        [Test]
        public void TrackCreated_TagPassedAsOnlyParameter_AttributesHaveCorrectValues()
        {
            //Arrange
            var tag = "AF1234";
            //Act
            var uut = new Track(tag);
            //Assert
            Assert.That(uut.Tag, Is.EqualTo("AF1234"));
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(0));
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(0));
            Assert.That(uut.LastSeen, Is.TypeOf<DateTime>());
            Assert.That(uut.CurrentPosition,Is.TypeOf<Coordinate>());
            Assert.That(uut.DirectionCalc, Is.TypeOf<DirectionCalc>());

        }

        [Test]
        public void TrackCreated_DefaultConstructor_AttributesHaveCorrectValues()
        {
            //Act
            var uut = new Track();
            //Assert
            Assert.That(uut.Tag, Is.EqualTo("AAAAAA"));
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
            var date = DateTime.Now;
            //Act
            uut.Update(coord, 500, date);
            //Assert
            Assert.That(uut.CurrentPosition, Is.EqualTo(coord));
        }

        [Test]
        public void UpdateCalled_CurrentHorizontalVelocityIsCorrect()
        {
            //Arrange
            var startCoord = Substitute.For<ICoordinate>();
            startCoord.x = 0;
            startCoord.y = 0;
            var tag = "AF1234";
            var uut = new Track(tag, startCoord);
            var coord = Substitute.For<ICoordinate>();
            coord.x = 5;
            coord.y = 6;
            Thread.Sleep(500);
            var date = DateTime.Now;
            var result = Math.Sqrt(61);
            //Act
            uut.Update(coord, 500, date);
            //Assert
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(result));
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
        public void UpdateCalled_CurrentCompassCourseSetCorrectly()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 1;
            coord.y = 0;
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
            var date = DateTime.Now;
            //Act
            uut.Update(coord, 5000, date);
            //Assert
            Assert.That(uut.CurrentAltitude, Is.EqualTo(5000));
        }

        [Test]
        public void UpdateCalledTwiceInOneSecond_HorizontalVelocityIsAsExpected()
        {
            //Arrange
            var uut = new Track();
            var coord = Substitute.For<ICoordinate>();
            coord.x = 500;
            coord.y = 30;
            Thread.Sleep(500);
            var date = DateTime.Now;
            //Act/
            uut.Update(coord, 5000, date);
            uut.Update(coord, 5000, date);
            //Assert
            Assert.That(uut.CurrentHorizontalVelocity,Is.EqualTo(Math.Sqrt(250900)));
        }

        [Test]
        public void TrackCreated_WrongTagFormatProvided_TagIsSetToAAAAAA()
        {
            //Arrange
            var tag = "12345649";
            //Act
            var uut = new Track(tag);
            //Assert
            Assert.That(uut.Tag, Is.EqualTo("AAAAAA"));
        }

        [Test]
        public void TrackCreated_NoTagProvided_TagIsSetToAAAAAA()
        {
            //Act
            var uut = new Track();
            //Assert
            Assert.That(uut.Tag, Is.EqualTo("AAAAAA"));
        }

    }
}
