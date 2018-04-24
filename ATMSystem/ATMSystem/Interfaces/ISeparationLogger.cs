using System;

namespace ATMSystem.Interfaces
{
    public interface ISeparationLogger
    {
        void LogSeperation(ISeparation separation);
        void SeparationHandler(object obj, SeparationEventArgs args);
    }
}