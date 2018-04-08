using System;

namespace ATMSystem.Interfaces
{
    public interface ITrackController
    {
        event EventHandler OnTrackUpdated;
        void TransponderDataHandler(object obj, EventArgs args);
    }
}