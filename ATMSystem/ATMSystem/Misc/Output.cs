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
        public void WriteToOutput(string output)
        {
            Console.WriteLine(output);
            Console.SetCursorPosition(0,0);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
