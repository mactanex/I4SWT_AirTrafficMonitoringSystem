using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using ATMSystem.Misc;
using NSubstitute;
using NUnit.Framework;

namespace ATMSystem.Unit.Tests.MiscTests
{
    [TestFixture]
    public class DirectionCalcUnitTest
    {
        [Test]
        public void CalculateDirection_OldCoord22_NewCoord24_returns0()
        {
            //arrange
            var uut = new DirectionCalc();

            var oldCoord = Substitute.For<ICoordinate>();
            oldCoord.x = 2;
            oldCoord.y = 2;

            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 2;
            newCoord.y = 4;

            //act
            var result = uut.CalculateDirection(oldCoord, newCoord);

            //assert
            Assert.That(result,Is.EqualTo(0));
        }

        [Test]
        public void CalculateDirection_OldCoord24_NewCoord44_returns90()
        {
            //arrange
            var uut = new DirectionCalc();

            var oldCoord = Substitute.For<ICoordinate>();
            oldCoord.x = 2;
            oldCoord.y = 4;

            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 4;
            newCoord.y = 4;

            //act
            var result = uut.CalculateDirection(oldCoord, newCoord);

            //assert
            Assert.That(result, Is.EqualTo(90)); //2 = E
        }

        [Test]
        public void CalculateDirection_OldCoord42_NewCoord22_returns270()
        {
            //arrange
            var uut = new DirectionCalc();

            var oldCoord = Substitute.For<ICoordinate>();
            oldCoord.x = 4;
            oldCoord.y = 2;

            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 2;
            newCoord.y = 2;

            //act
            var result = uut.CalculateDirection(oldCoord, newCoord);

            //assert
            Assert.That(result, Is.EqualTo(270)); //270 = W
        }

        [Test]
        public void CalculateDirection_OldCoord44_NewCoord42_returns0()
        {
            //arrange
            var uut = new DirectionCalc();

            var oldCoord = Substitute.For<ICoordinate>();
            oldCoord.x = 4;
            oldCoord.y = 4;

            var newCoord = Substitute.For<ICoordinate>();
            newCoord.x = 4;
            newCoord.y = 2;

            //act
            var result = uut.CalculateDirection(oldCoord, newCoord);

            //assert
            Assert.That(result, Is.EqualTo(180)); //180 = S
        }
    }
}
