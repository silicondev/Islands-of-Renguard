using dEvine_and_conquer.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.AI.Pathfinding.AStar
{
    internal class AStarTile
    {
        public float f = 0;
        public float g = 0;
        public float h = 0;
        public List<AStarTile> Local = new List<AStarTile>();
        public AStarTile Prev = null;
        public Tile Tile;

        public bool IsWall { get
            {
                return Tile.Type.IsCollidable;
            } 
        }

        public AStarTile(Tile tile)
        {
            Tile = tile;
        }

        public void RefreshLocal(List<List<AStarTile>> grid)
        {
            int x = (int)Tile.Location.X;
            int y = (int)Tile.Location.Y;

            if (grid == null || grid[0] == null || grid[0][0] == null)
                return;

            Local.Clear();

            int rows = grid[0].Count();
            int cols = grid.Count();

            bool xa = x < cols - 1;
            bool xb = x > 0;
            bool ya = y < rows - 1;
            bool yb = y > 0;

            AStarTile gxa = xa ? grid[x + 1][y] : null;
            AStarTile gxb = xb ? grid[x - 1][y] : null;
            AStarTile gya = ya ? grid[x][y + 1] : null;
            AStarTile gyb = yb ? grid[x][y - 1] : null;
            AStarTile gxyaa = (xa && ya) ? grid[x + 1][y + 1] : null;
            AStarTile gxyba = (xb && ya) ? grid[x - 1][y + 1] : null;
            AStarTile gxyab = (xa && yb) ? grid[x + 1][y - 1] : null;
            AStarTile gxybb = (xb && yb) ? grid[x - 1][y - 1] : null;

            if (xa && !gxa.IsWall) Local.Add(gxa);
            if (xb && !gxb.IsWall) Local.Add(gxb);
            if (ya && !gya.IsWall) Local.Add(gya);
            if (yb && !gyb.IsWall) Local.Add(gyb);

            if (xa && ya && !gxyaa.IsWall && !(gxa.IsWall && gya.IsWall)) Local.Add(gxyaa);
            if (xb && ya && !gxyba.IsWall && !(gxb.IsWall && gya.IsWall)) Local.Add(gxyba);
            if (xa && yb && !gxyab.IsWall && !(gxa.IsWall && gyb.IsWall)) Local.Add(gxyab);
            if (xb && yb && !gxybb.IsWall && !(gxb.IsWall && gyb.IsWall)) Local.Add(gxybb);
        }
    }
}
