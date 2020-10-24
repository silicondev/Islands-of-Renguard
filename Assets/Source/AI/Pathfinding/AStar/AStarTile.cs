using dEvine_and_conquer.Base;
using dEvine_and_conquer.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.AI.Pathfinding.AStar
{
    internal class AStarTile : VisualObject
    {
        public float f = 0;
        public float g = 0;
        public float h = 0;
        public List<AStarTile> Local = new List<AStarTile>();
        public List<AStarTile> Traversable = new List<AStarTile>();
        public AStarTile Prev { get; set; } = null;
        public Block Block { get; }
        public bool IsWall => Block.IsCollidable;

        public AStarTile(Block block) : base(block.Location)
        {
            Block = block;
        }

        public void RefreshLocal(List<AStarTile> grid)
        {
            int x = Block.Location.X.Floor();
            int y = Block.Location.Y.Floor();

            if (grid == null)
                return;

            Local.Clear();

            bool e = grid.Contains(x + 1, y);
            bool w = grid.Contains(x - 1, y);
            bool n = grid.Contains(x, y + 1);
            bool s = grid.Contains(x, y - 1);

            bool ne = grid.Contains(x + 1, y + 1);
            bool nw = grid.Contains(x - 1, y + 1);
            bool se = grid.Contains(x + 1, y - 1);
            bool sw = grid.Contains(x - 1, y - 1);

            AStarTile te = e ? grid.Get(x + 1, y) : null;
            AStarTile tw = w ? grid.Get(x - 1, y) : null;
            AStarTile tn = n ? grid.Get(x, y + 1) : null;
            AStarTile ts = s ? grid.Get(x, y - 1) : null;
            AStarTile tne = ne ? grid.Get(x + 1, y + 1) : null;
            AStarTile tnw = nw ? grid.Get(x - 1, y + 1) : null;
            AStarTile tse = se ? grid.Get(x + 1, y - 1) : null;
            AStarTile tsw = sw ? grid.Get(x - 1, y - 1) : null;

            if (e) Local.Add(te);
            if (w) Local.Add(tw);
            if (n) Local.Add(tn);
            if (s) Local.Add(ts);
            if (ne) Local.Add(tne);
            if (nw) Local.Add(tnw);
            if (se) Local.Add(tse);
            if (sw) Local.Add(tsw);

            bool we = e ? te.IsWall : true;
            bool ww = w ? tw.IsWall : true;
            bool wn = n ? tn.IsWall : true;
            bool ws = s ? ts.IsWall : true;
            bool wne = ne ? tne.IsWall : true;
            bool wnw = nw ? tnw.IsWall : true;
            bool wse = se ? tse.IsWall : true;
            bool wsw = sw ? tsw.IsWall : true;

            if (!we) Traversable.Add(te);
            if (!ww) Traversable.Add(tw);
            if (!wn) Traversable.Add(tn);
            if (!ws) Traversable.Add(ts);

            if (!wne && !wn && !we) Traversable.Add(tne);
            if (!wnw && !wn && !ww) Traversable.Add(tnw);
            if (!wse && !ws && !we) Traversable.Add(tse);
            if (!wsw && !ws && !ww) Traversable.Add(tsw);
        }
    }
}
