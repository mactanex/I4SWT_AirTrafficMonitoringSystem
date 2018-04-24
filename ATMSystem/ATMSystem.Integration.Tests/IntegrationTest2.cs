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
    public class IntegrationTest2
    {
        private ITrackController _trackController;
        private IMapDrawer _mapDrawer;
        private ITransponderDataConverter _transponderDataConverter;
        private ISeparationMonitor _separationMonitor;
        private ITransponderReceiver _transponderReceiver;

        [SetUp]
        public void SetUp()
        {
            //fakes
            _mapDrawer = Substitute.For<IMapDrawer>();
            

            //driver
            _transponderReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            //included
            _transponderDataConverter = new TransponderDataConverter();
            _trackController = new TrackController(_transponderReceiver, _transponderDataConverter, _mapDrawer);
            _separationMonitor = new SeparationMonitor(_trackController);
        }
    }
}
