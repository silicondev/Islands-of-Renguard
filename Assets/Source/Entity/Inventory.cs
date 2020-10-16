using dEvine_and_conquer.Base;
using dEvine_and_conquer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Entity
{
    public class Inventory
    {
        private List<ItemBag> _items;
        private ItemBag _nullItem = new ItemBag(ObjectID.ITEM.GENERIC.VOID);
        public int Slots { get; }

        public Inventory(int slots)
        {
            Slots = slots;
            _items = new List<ItemBag>();
            for (int i = 0; i < slots; i++) _items.Add(_nullItem);
        }

        private void checkItems()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                var item = _items[i];
                if (!item.Existing) _items[i] = _nullItem;
            }
        }

        public ItemBag GetItem(int slot)
        {
            if (slot < 0 || slot > Slots) return _nullItem;
            return _items[slot];
        }

        public void SetItem(ItemBag item, int slot)
        {
            if (slot < 0 || slot > Slots) return;
            _items[slot] = item;
            checkItems();
        }

        public void Clear()
        {
            for (int i = 0; i < Slots; i++) _items[i] = _nullItem;
        }

        public List<ItemBag> GetCopy()
        {
            return new List<ItemBag>(_items);
        }
    }
}
