﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Handlers;
using ATMSystem.Interfaces;
using ATMSystem.Misc;
using NSubstitute;
using NUnit.Framework;
using TransponderReceiver;

namespace ATMSystem.Integration.Tests
{
    [TestFixture]
    public class IntegrationTest5
    {
        private ITrackController _trackController;
        private IMapDrawer _mapDrawer;
        private ITransponderDataConverter _transponderDataConverter;
        private ISeperationMonitor _seperationMonitor;
        private ITransponderReceiver _transponderReceiver;

        [SetUp]
        public void SetUp()
        {
            //driver
            _transponderReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            //included
            _mapDrawer = Substitute.For<IMapDrawer>();
            _transponderDataConverter = new TransponderDataConverter();
            _trackController = new TrackController(_transponderReceiver, _transponderDataConverter, _mapDrawer);
            _seperationMonitor = new SeperationMonitor(_trackController);
        }
    }
}