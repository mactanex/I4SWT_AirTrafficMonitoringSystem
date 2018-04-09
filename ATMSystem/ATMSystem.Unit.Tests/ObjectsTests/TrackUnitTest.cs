using System;
using System.Threading;
using ATMSystem.Interfaces;
using ATMSystem.Objects;
using NSubstitute;
using NUnit.Framework;

namespace ATMSystem.Unit.Tests.ObjectsTests
{
    [TestFixture]
    class TrackUnitTest
    {

        public class FakeDirectionCalculator : IDirectionCalc
        {
            public int CalculateDirection(ICoordinate oldCoordinate, ICoordinate newCoordinate)
            {
                return 1;
            }
        }

        [Test]
        public void TrackCreated_TagCoordinateAltitudeAndTimestampPassedAsParameter_AttributesHaveCorrectValues()
        {
            //Arrange/act
            var tag = "ATR423";
            var startCoords = Substitute.For<ICoordinate>();
            startCoords.x = 1;
            startCoords.y = 5;
            var altitude = 8000;
            var timestamp = DateTime.Now;
            //Act
            var uut = new Track(tag, startCoords, altitude, timestamp, new FakeDirectionCalculator());
            //Assert
            Assert.That(uut.CurrentPosition, Is.EqualTo(startCoords));
            Assert.That(uut.CurrentAltitude,Is.EqualTo(altitude));
            Assert.That(uut.LastSeen, Is.EqualTo(timestamp));
            Assert.That(uut.Tag, Is.EqualTo(tag));
        }

        [Test]
        public void TrackCreated_WrongTagFormatProvided_ExceptionIsThrown()
        {
            //Arrange

            var tag = "12345649";
            var startCoord = Substitute.For<ICoordinate>();
            var altitude = 8000;
            var timestamp = DateTime.Now;
            //Act/
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var uut = new Track(tag, startCoord, altitude, timestamp, new FakeDirectionCalculator());
            });
        }

        [Test]
        public void TrackCreated_NoTagProvided_ExceptionIsThrown()
        {
            //Arrange
            string tag = null;
            var startCoord = Substitute.For<ICoordinate>();
            var altitude = 8000;
            var timestamp = DateTime.Now;
            //Act/
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var uut = new Track(tag, startCoord, altitude, timestamp, new FakeDirectionCalculator());
            });
        }

        [Test]
        public void UpdateCalled_CurrentPositionChanged()
        {
            //Arrange
            var tag = "ATR423";
            var startCoords = Substitute.For<ICoordinate>();
            var altitude = 8000;
            var coord = Substitute.For<ICoordinate>();
            coord.x = 15;
            coord.y = 20;
            var date = DateTime.Now;
            var uut = new Track(new FakeDirectionCalculator()) { Tag = tag, LastKnownPosition = startCoords, CurrentPosition = coord, CurrentAltitude = altitude};
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
            var tag = "ATR423";
            var altitude = 8000;
            var timestamp = DateTime.Now;
            var uut = new Track(tag, startCoord, altitude, timestamp, new FakeDirectionCalculator());
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
            var startCoord = Substitute.For<ICoordinate>();
            var uut = new Track(new FakeDirectionCalculator()) {Tag = "ATR423", CurrentPosition = startCoord, LastSeen = DateTime.Now, CurrentAltitude = 7, CurrentCompassCourse = 5};
            var endCoord = Substitute.For<ICoordinate>();
            endCoord.x = 7;
            endCoord.y = 2;
            Thread.Sleep(1000);
            var timestamp = DateTime.Now;
            //Act
            uut.Update(endCoord, 500, timestamp);
            //Assert
            Assert.That(timestamp.CompareTo(uut.LastSeen), Is.EqualTo(0));
        }

        [Test]
        public void UpdateCalled_DirectionCalcIsCalledWithExpectedParameters()
        {
            //Arrange
            var tag = "ATR423";
            var calc = Substitute.For<IDirectionCalc>();
            var startCoord = Substitute.For<ICoordinate>();
            var uut = new Track(tag, startCoord, 1000, DateTime.Now, calc) ;
            var endCoord = Substitute.For<ICoordinate>();
            endCoord.x = 1;
            endCoord.y = 0;
            var timestamp = DateTime.Now;
            //Act
            uut.Update(endCoord, 1000, timestamp);
            //Assert
            calc.Received().CalculateDirection(startCoord, endCoord);
        }

        [Test]
        public void UpdateCalled_CurrentCompassCourseSetCorrectly()
        {
            //Arrange
            var tag = "ATR423";
            var startCoord = Substitute.For<ICoordinate>();
            var uut = new Track(new FakeDirectionCalculator()) {Tag = tag, CurrentPosition = startCoord};
            var coord = Substitute.For<ICoordinate>();
            coord.x = 1;
            coord.y = 0;
            var timestamp = DateTime.Now;
            //Act
            uut.Update(coord, 500, timestamp);
            //Assert
            Assert.That(uut.CurrentCompassCourse, Is.EqualTo(1));
        }

        [Test]
        public void UpdateCalled_AltitudeUpdatedCorrectly()
        {
            //Arrange
            var startCoord = Substitute.For<ICoordinate>();
            var tag = "ATR423";
            var altitude = 8000;
            var timestamp = DateTime.Now;
            var uut = new Track(tag, startCoord, altitude, timestamp, new FakeDirectionCalculator());
            var coord = Substitute.For<ICoordinate>();
            coord.x = 1;
            coord.y = 0;
            var newTimestamp = DateTime.Now;
            //Act
            uut.Update(coord, 5000, newTimestamp);
            //Assert
            Assert.That(uut.CurrentAltitude, Is.EqualTo(5000));
        }

        [Test]
        public void UpdateCalledTwiceInOneSecond_HorizontalVelocityIsAsExpected()
        {
            //Arrange
            var startCoord = Substitute.For<ICoordinate>();
            var tag = "ATR423";
            var altitude = 8000;
            var timestamp = DateTime.Now;
            var uut = new Track(tag, startCoord, altitude, timestamp, new FakeDirectionCalculator());
            var coord = Substitute.For<ICoordinate>();
            coord.x = 500;
            coord.y = 30;
            Thread.Sleep(500);
            var newTimestamp = DateTime.Now;
            //Act/
            uut.Update(coord, 5000, newTimestamp);
            uut.Update(coord, 5000, newTimestamp);
            //Assert
            Assert.That(uut.CurrentHorizontalVelocity,Is.EqualTo(Math.Sqrt(250900)));
        }
    }
}