using System;

namespace ATMSystem.Interfaces
{
    public interface ITrackController
    {
        event EventHandler OnTransponderDataReady;

        void TransponderDataHandler(object obj, EventArgs args);
        ITrack ConvertRawDataToTrack(string rawData);

    }
}