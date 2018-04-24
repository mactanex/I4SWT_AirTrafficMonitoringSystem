using System.Collections.Generic;

namespace ATMSystem.Interfaces
{
    public interface IMapDrawer
    {
        string GenerateMap(List<ITrack> tracks);
        void DrawMap(string map);
    }
}