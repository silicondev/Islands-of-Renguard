using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public class Prefab
    {
        public readonly ID ID;
        public readonly bool IsCollidable = false;
        public string Name;

        public Prefab(ID id, string name, bool isCollide)
        {
            ID = id;
            IsCollidable = isCollide;
            Name = name;
        }
    }
}
