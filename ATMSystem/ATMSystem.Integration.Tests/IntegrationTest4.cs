using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Handlers;
using ATMSystem.Interfaces;
using ATMSystem.Misc;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TransponderReceiver;

namespace ATMSystem.Integration.Tests
{
    [TestFixture]
    public class IntegrationTest4
    {
        private ITrackController _trackController;
        private IMapDrawer _mapDrawer;
        private ITransponderDataConverter _transponderDataConverter;
        private ISeparationMonitor _separationMonitor;
        private ITransponderReceiver _transponderReceiver;
        private IFileWriter _fileWriter;
        private ISeparationLogger _separationLogger;


        [SetUp]
        public void SetUp()
        {
            //fakes
            _mapDrawer = Substitute.For<IMapDrawer>();


            //driver
            _transponderReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            //included
            _transponderDataConverter = new TransponderDataConverter();
            _fileWriter = new FileWriter();
            _trackController = new TrackController(_transponderReceiver, _transponderDataConverter, _mapDrawer);
            _separationMonitor = new SeparationMonitor(_trackController);
            _separationLogger = new SeparationLogger(_fileWriter,_separationMonitor);
        }
    }
}
