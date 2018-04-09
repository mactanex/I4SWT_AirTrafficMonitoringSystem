using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Misc
{
    public class Output : IOutput
    {
        public void WriteToOutput(ITrack track)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine("Tag: " + track.Tag);
            Console.WriteLine("Current Position: (" + track.CurrentPosition.x + "," + track.CurrentPosition.y + ")" + " meters");
            Console.WriteLine("Current Altitude: " + track.CurrentAltitude + " meters");
            Console.WriteLine("Current Horizontal Velocity: " + track.CurrentHorizontalVelocity + " m/s");
            Console.WriteLine("Current Compass Course: " + track.CurrentCompassCourse + " degrees");
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
