using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Handlers
{
    class SeperationMonitor : ISeperationMonitor
    {
        public event EventHandler OnSeperationEvent;
    }
}
