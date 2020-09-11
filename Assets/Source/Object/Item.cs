using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Object
{
    public class Item
    {
        public ID Id { get; } = 0;
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

        public Item() { }
        public Item(ID id)
        {
            Id = id;
        }
        public Item(ID id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }

    public static class ItemID
    {
        public static class GENERIC
        {
            public static readonly ID VOID = 0;
        }
        //public static class WEAPON
        //{
        //    
        //}
        //public static class TOOL
        //{
        //
        //}
        //public static class BUILDABLE
        //{
        //
        //}
    }
}
