using System;

namespace ATMSystem.Interfaces
{
    public interface ITrackController
    {
        event EventHandler OnTransponderDataReady;
    }
}