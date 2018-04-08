using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Handlers
{
    class SeperationHandler : ISeperationHandler
    {
        public event EventHandler OnSeperationStarted;
        public event EventHandler OnSeperationEnded;
    }
}
