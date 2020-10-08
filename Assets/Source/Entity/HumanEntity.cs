using dEvine_and_conquer.Base;
using dEvine_and_conquer.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace dEvine_and_conquer.Entity
{
    public class HumanEntity : GenericEntity
    {
        public int Gender { get; set; }

        public HumanEntity(Point location, int gender) : base(location, ObjectID.ENTITY.NPC.HUMAN.FRIENDLY.MALE, 20)
        {
            Sprite[] spr = Resources.LoadAll<Sprite>("Textures/Entity/Human/Human_Male");
            Sprite tex1 = spr.First(x => x.name == "human_male_idle_1");
            Sprite tex2 = spr.First(x => x.name == "human_male_idle_2");
            Textures = new List<Sprite>()
            {
                tex1,
                tex2
            };
            Gender = gender;
        }

        protected override void UpdateSpecific(GameSystem system)
        {
            //var chunk = system.LoadedChunks.GetChunkWithPosition(Location);
            //Pathfinder.UpdateWorld(chunk.Tiles);
            //var path = Pathfinder.GetPath(Location, )
        }
    }
}
