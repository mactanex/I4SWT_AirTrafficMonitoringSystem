using System;

namespace ATMSystem.Interfaces
{
    public interface ISeperationHandler
    {
        event EventHandler OnSeperationStarted;
        event EventHandler OnSeperationEnded;
    }
}