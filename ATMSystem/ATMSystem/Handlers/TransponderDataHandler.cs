﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Handlers
{
    class TransponderDataHandler : ITransponderDataHandler
    {
        public event EventHandler OnTransponderDataReady;
    }
}
