﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Handlers
{
    class TrackController : ITrackController
    {
        public event EventHandler OnTransponderDataReady;

        public void TransponderDataHandler(object obj, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
