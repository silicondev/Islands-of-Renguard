using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.AI.Pathfinding.AStar
{
    internal static class AStarExtensions
    {
        public static T Get<T>(this List<T> tiles, int x, int y) where T : VisualObject => tiles.Get(new Point(x, y));

        public static T Get<T>(this List<T> tiles, Point loc) where T : VisualObject => tiles.Where(x => (int)x.Location.X == (int)loc.X && (int)x.Location.Y == (int)loc.Y).First();

        public static bool Contains(this List<AStarTile> tiles, int x, int y) => tiles.Contains(new Point(x, y));
        public static bool Contains(this List<AStarTile> tiles, Point loc) => tiles.Any(x => (int)x.Location.X == (int)loc.X && (int)x.Location.Y == (int)loc.Y);
    }
}
