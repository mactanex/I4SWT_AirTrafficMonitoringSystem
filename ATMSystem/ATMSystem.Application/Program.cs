﻿using System;
using System.Threading;
using ATMSystem.Handlers;
using ATMSystem.Misc;
using ATMSystem.Objects;
using TransponderReceiver;

namespace ATMSystem.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var myTransponderReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            var myDataConverter = new TransponderDataConverter();
            var myOutput = new Output();

            var trackController = new TrackController(myTransponderReceiver, myDataConverter, myOutput);

            while (true)
            {
                Thread.Sleep(1000);
            }

        }
    }
}