using System;

namespace ATMSystem.Interfaces
{
    public interface ITransponderDataHandler
    {
        event EventHandler OnTransponderDataReady;
    }
}