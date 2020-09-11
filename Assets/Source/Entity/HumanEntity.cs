using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Entity
{
    public class HumanEntity : GenericEntity
    {
        public int Gender { get; set; }
        

        public HumanEntity(Point location, int gender) : base(location, EntityID.NPC.HUMAN, 20, new List<Texture>()
        {
            Resources.Load<Texture>("Textures/Entity/Human/human_male_idle_1"),
            Resources.Load<Texture>("Textures/Entity/Human/human_make_idle_2")
        })
        {
            Gender = gender;
        }
    }
}
