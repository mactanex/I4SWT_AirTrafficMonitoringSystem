using System;

namespace ATMSystem.Interfaces
{
    public interface ISeperationMonitor
    {
        event EventHandler OnSeperationEvent;
    }
}