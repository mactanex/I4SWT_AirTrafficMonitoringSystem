using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransponderReceiver;

namespace ATMSystem.Application
{
    /// <summary>
    /// Not implemented yet
    /// </summary>
    public class Class1
    {
        static void lol(object obj, RawTransponderDataEventArgs args)
        {
            foreach (var derp in args.TransponderData)
            {
                Console.WriteLine(derp);
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        public static void Main(string[] args)
        {
            ITransponderReceiver receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            receiver.TransponderDataReady += lol;

            while (true)
            {
                Thread.Sleep(1000);
            }

        }
    }
}