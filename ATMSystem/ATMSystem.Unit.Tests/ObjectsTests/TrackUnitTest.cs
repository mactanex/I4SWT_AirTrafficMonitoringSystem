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

        [Test]
        public void TrackCreated_TagCoordinateAltitudeAndTimestampPassedAsParameter_AttributesHaveCorrectValues()
        {
            //Arrange
            var tag = "ATR423";
            var startCoords = Substitute.For<ICoordinate>();
            startCoords.x = 1;
            startCoords.y = 5;
            var altitude = 8000;
            var timestamp = DateTime.Now;
            var directionCalc = Substitute.For<IDirectionCalc>();
            //Act
            var uut = new Track(tag, startCoords, altitude, timestamp, directionCalc);
            //Assert
            Assert.That(uut.CurrentPosition, Is.EqualTo(startCoords));
            Assert.That(uut.CurrentAltitude,Is.EqualTo(altitude));
            Assert.That(uut.LastSeen, Is.EqualTo(timestamp));
            Assert.That(uut.Tag, Is.EqualTo(tag));
            Assert.That(uut.CurrentCompassCourse,Is.EqualTo(0));
        }

        [Test]
        public void TrackCreated_WrongTagFormatProvided_ExceptionIsThrown()
        {
            //Arrange
            var tag = "12345649";
            var startCoord = Substitute.For<ICoordinate>();
            var altitude = 8000;
            var timestamp = DateTime.Now;
            var directionCalc = Substitute.For<IDirectionCalc>();
            //Act/
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var uut = new Track(tag, startCoord, altitude, timestamp, directionCalc);
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
            var directionCalc = Substitute.For<IDirectionCalc>();
            //Act/
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var uut = new Track(tag, startCoord, altitude, timestamp, directionCalc);
            });
        }

        [Test]
        public void UpdateCalled_CurrentPositionChanged()
        {
            //Arrange
            var tag = "ATR423";
            var startCoords = Substitute.For<ICoordinate>();
            var altitude = 8000;
            var timestamp = DateTime.Now;
            var directionCalc = Substitute.For<IDirectionCalc>();
            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 15;
            newCoord.y = 20;
            var uut = new Track(directionCalc) { Tag = tag, LastKnownPosition = startCoords, CurrentPosition = newCoord, CurrentAltitude = altitude};
            //Act
            uut.Update(newCoord, 500, timestamp);
            //Assert
            Assert.That(uut.CurrentPosition, Is.EqualTo(newCoord));
        }

        [Test]
        public void UpdateCalled_CurrentHorizontalVelocityIsCorrect()
        {
            //Arrange
            var startCoord = Substitute.For<ICoordinate>();
            var tag = "ATR423";
            var altitude = 8000;
            var timestamp = DateTime.Now;
            var directionCalc = Substitute.For<IDirectionCalc>();
            var uut = new Track(tag, startCoord, altitude, timestamp, directionCalc);
            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 5;
            newCoord.y = 6;
            Thread.Sleep(500);
            var newTimestamp = DateTime.Now;
            var result = Math.Sqrt(61);
            //Act
            uut.Update(newCoord, 500, newTimestamp);
            //Assert
            Assert.That(uut.CurrentHorizontalVelocity, Is.EqualTo(result));
        }

        [Test]
        public void UpdateCalled_LastSeenIsUpdated()
        {
            //Arrange
            var startCoord = Substitute.For<ICoordinate>();
            var directionCalc = Substitute.For<IDirectionCalc>();
            var uut = new Track(directionCalc) {Tag = "ATR423", CurrentPosition = startCoord, LastSeen = DateTime.Now, CurrentAltitude = 7, CurrentCompassCourse = 5};
            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 7;
            newCoord.y = 2;
            Thread.Sleep(1000);
            var timestamp = DateTime.Now;
            //Act
            uut.Update(newCoord, 500, timestamp);
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
            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 1;
            newCoord.y = 0;
            var timestamp = DateTime.Now;
            //Act
            uut.Update(newCoord, 1000, timestamp);
            //Assert
            calc.Received().CalculateDirection(startCoord, newCoord);
        }

        [Test]
        public void UpdateCalled_AltitudeUpdatedCorrectly()
        {
            //Arrange
            var startCoord = Substitute.For<ICoordinate>();
            var tag = "ATR423";
            var altitude = 8000;
            var timestamp = DateTime.Now;
            var directionCalc = Substitute.For<IDirectionCalc>();
            var uut = new Track(tag, startCoord, altitude, timestamp, directionCalc);
            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 1;
            newCoord.y = 0;
            var newTimestamp = DateTime.Now;
            //Act
            uut.Update(newCoord, 5000, newTimestamp);
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
            var directionCalc = Substitute.For<IDirectionCalc>();
            var uut = new Track(tag, startCoord, altitude, timestamp, directionCalc);
            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 500;
            newCoord.y = 30;
            Thread.Sleep(500);
            var newTimestamp = DateTime.Now;
            //Act/
            uut.Update(newCoord, 5000, newTimestamp);
            uut.Update(newCoord, 5000, newTimestamp);
            //Assert
            Assert.That(uut.CurrentHorizontalVelocity,Is.EqualTo(Math.Sqrt(250900)));
        }
    }
}
