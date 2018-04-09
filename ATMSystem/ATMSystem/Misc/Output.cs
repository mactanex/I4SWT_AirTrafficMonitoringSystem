using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Misc
{
    class Output : IOutput
    {
        public void WriteToOutput(ITrack track)
        {
            Console.WriteLine("Tag: " + track.Tag);
            Console.WriteLine();
            Console.WriteLine();
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
