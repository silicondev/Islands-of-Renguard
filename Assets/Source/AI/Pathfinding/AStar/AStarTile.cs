using dEvine_and_conquer.Base;
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
        public AStarTile Prev { get; set; } = null;
        public Tile Tile { get; }
        public Point Location { 
            get
            {
                return Tile.Location;
            } 
        }

        public bool IsWall 
        { 
            get
            {
                return Tile.Type.IsCollidable;
            } 
        }

        public AStarTile(Tile tile)
        {
            Tile = tile;
        }

        //public void RefreshLocal(XYContainer<AStarTile> grid)
        //{
        //    int x = (int)Tile.Location.X;
        //    int y = (int)Tile.Location.Y;

        //    if (grid == null || grid.Get(0, 0) == null)
        //        return;

        //    Local.Clear();

        //    int rows = grid.Count(true);
        //    int cols = grid.Count(false);

        //    bool xa = x < cols - 1;
        //    bool xb = x > 0;
        //    bool ya = y < rows - 1;
        //    bool yb = y > 0;

        //    AStarTile gxa = xa ? grid.Get(x + 1, y) : null;
        //    AStarTile gxb = xb ? grid.Get(x - 1, y) : null;
        //    AStarTile gya = ya ? grid.Get(x, y + 1) : null;
        //    AStarTile gyb = yb ? grid.Get(x, y - 1) : null;
        //    AStarTile gxyaa = (xa && ya) ? grid.Get(x + 1, y + 1) : null;
        //    AStarTile gxyba = (xb && ya) ? grid.Get(x - 1, y + 1) : null;
        //    AStarTile gxyab = (xa && yb) ? grid.Get(x + 1, y - 1) : null;
        //    AStarTile gxybb = (xb && yb) ? grid.Get(x - 1, y - 1) : null;

        //    if (xa && !gxa.IsWall) Local.Add(gxa);
        //    if (xb && !gxb.IsWall) Local.Add(gxb);
        //    if (ya && !gya.IsWall) Local.Add(gya);
        //    if (yb && !gyb.IsWall) Local.Add(gyb);

        //    if (xa && ya && !gxyaa.IsWall && !(gxa.IsWall && gya.IsWall)) Local.Add(gxyaa);
        //    if (xb && ya && !gxyba.IsWall && !(gxb.IsWall && gya.IsWall)) Local.Add(gxyba);
        //    if (xa && yb && !gxyab.IsWall && !(gxa.IsWall && gyb.IsWall)) Local.Add(gxyab);
        //    if (xb && yb && !gxybb.IsWall && !(gxb.IsWall && gyb.IsWall)) Local.Add(gxybb);
        //}

        public void RefreshLocal(List<AStarTile> grid)
        {
            int x = (int)Tile.Location.X;
            int y = (int)Tile.Location.Y;

            if (grid == null)
                return;

            Local.Clear();

            //int rows = grid.Count(true);
            //int cols = grid.Count(false);

            //bool xa = x < cols - 1;
            //bool xb = x > 0;
            //bool ya = y < rows - 1;
            //bool yb = y > 0;

            bool xa = grid.Contains(x + 1, y);
            bool xb = grid.Contains(x - 1, y);
            bool ya = grid.Contains(x, y + 1);
            bool yb = grid.Contains(x, y - 1);

            bool xyaa = grid.Contains(x + 1, y + 1);
            bool xyba = grid.Contains(x - 1, y + 1);
            bool xyab = grid.Contains(x + 1, y - 1);
            bool xybb = grid.Contains(x - 1, y - 1);

            AStarTile tgxa = xa ? grid.Get(x + 1, y) : null;
            AStarTile tgxb = xb ? grid.Get(x - 1, y) : null;
            AStarTile tgya = ya ? grid.Get(x, y + 1) : null;
            AStarTile tgyb = yb ? grid.Get(x, y - 1) : null;
            AStarTile tgxyaa = xyaa ? grid.Get(x + 1, y + 1) : null;
            AStarTile tgxyba = xyba ? grid.Get(x - 1, y + 1) : null;
            AStarTile tgxyab = xyab ? grid.Get(x + 1, y - 1) : null;
            AStarTile tgxybb = xybb ? grid.Get(x - 1, y - 1) : null;

            bool gxa = tgxa != null ? grid.Get(x + 1, y).IsWall : true;
            bool gxb = tgxb != null ? grid.Get(x - 1, y).IsWall : true;
            bool gya = tgya != null ? grid.Get(x, y + 1).IsWall : true;
            bool gyb = tgyb != null ? grid.Get(x, y - 1).IsWall : true;
            bool gxyaa = tgxyaa != null ? grid.Get(x + 1, y + 1).IsWall : true;
            bool gxyba = tgxyba != null ? grid.Get(x - 1, y + 1).IsWall : true;
            bool gxyab = tgxyab != null ? grid.Get(x + 1, y - 1).IsWall : true;
            bool gxybb = tgxybb != null ? grid.Get(x - 1, y - 1).IsWall : true;

            if (xa && !gxa) Local.Add(tgxa);
            if (xb && !gxb) Local.Add(tgxb);
            if (ya && !gya) Local.Add(tgya);
            if (yb && !gyb) Local.Add(tgyb);

            if (xyaa && !gxyaa && !(gxa && gya)) Local.Add(tgxyaa);
            if (xyba && !gxyba && !(gxb && gya)) Local.Add(tgxyba);
            if (xyab && !gxyab && !(gxa && gyb)) Local.Add(tgxyab);
            if (xybb && !gxybb && !(gxb && gyb)) Local.Add(tgxybb);
        }
    }
}
