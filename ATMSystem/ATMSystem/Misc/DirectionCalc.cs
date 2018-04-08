using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Misc
{
    public class DirectionCalc : IDirectionCalc
    {
        public int CalculateDirection(ICoordinate oldCoordinate, ICoordinate newCoordinate)
        {
            double dY = (newCoordinate.y - oldCoordinate.y);
            double dX = (newCoordinate.x - oldCoordinate.x);
            double angle = (Math.Atan2(dX,dY)); //radians
            return (int) (360 + (angle * 180 / Math.PI )) % 360; //degrees

        }
    }
}
