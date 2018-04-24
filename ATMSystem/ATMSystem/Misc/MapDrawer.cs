using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ATMSystem.Misc
{
    public class MapDrawer : IMapDrawer
    {
        private IOutput _output;

        public MapDrawer(IOutput output)
        {
            _output = output;
        }

        public string GenerateMap(List<ITrack> tracks)
        {
            var sb = new StringBuilder();

            var width = Console.WindowHeight;
            var stepSize = (double) width / 80000;

            var grid = new List<List<char>>(width);

            for (int y = 0; y < width; y++)
            {
                grid.Add(new List<char>(width));
                for (int x = 0; x < width; x++)
                {
                    if (y == 0 || y == width - 1)
                    {
                        grid[y].Insert(x, 'X');
                    }
                    else if (y != 0 && x == 0 || x == width - 1)
                    {
                        grid[y].Insert(x, 'X');
                    }
                    else
                    {
                        grid[y].Insert(x, ' ');
                    }
                }
            }

            foreach (var track in tracks)
            {

                int x = (int) ((track.CurrentPosition.x - 10000) * stepSize);
                int y = (int) ((track.CurrentPosition.y - 10000) * stepSize);

                if (x >= 0 && x < width && y >= 0 && y < width)
                {
                    grid[y].RemoveAt(x);
                    grid[y].Insert(x, 'O');
                }
            }

            for (int i = width - 1; i >= 0; i--)
            {
                sb.Append(new string(grid[i].ToArray()) + "\n");
            }

            return sb.ToString();
        }

        public void DrawMap(string map)
        {
            _output.Clear();
            _output.WriteToOutput(map);
        }
    }
}
