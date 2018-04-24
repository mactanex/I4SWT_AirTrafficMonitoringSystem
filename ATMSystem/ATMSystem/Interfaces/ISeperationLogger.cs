using System;

namespace ATMSystem.Interfaces
{
    public interface ISeperationLogger
    {
        void LogSeperation(ISeperation seperation);
        void SeperationHandler(object obj, SeperationEventArgs args);
    }
}