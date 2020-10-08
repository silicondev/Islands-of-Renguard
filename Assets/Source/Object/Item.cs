using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Object
{
    public class ItemBag
    {
        public Prefab Id { get; }
        public int Amount { 
            get
            {
                return _amount;
            } 
            set 
            {
                _amount = value;
                if (_amount <= 0)
                {
                    _amount = 0;
                    Existing = false;
                }
                else Existing = true;
            } 
        }
        private int _amount = 1;
        public bool Existing
        {
            get
            {
                if (_existing == 2)
                {
                    if (_amount <= 0)
                    {
                        _amount = 0;
                        _existing = 0;
                    } else
                    {
                        _existing = 1;
                    }
                }
                return _existing == 1;
            }
            private set
            {
                _existing = value ? 1 : 0;
            }
        }
        public int _existing = 1;
        public ItemBag(Prefab id)
        {
            Id = id;
        }
        public ItemBag(Prefab id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
