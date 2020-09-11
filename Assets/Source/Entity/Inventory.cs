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
        private List<Item> _items;
        private Item _nullItem = new Item(ItemID.GENERIC.VOID);
        public int Slots { get; }

        public Inventory(int slots)
        {
            Slots = slots;
            _items = new List<Item>();
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

        public Item GetItem(int slot)
        {
            if (slot < 0 || slot > Slots) return _nullItem;
            return _items[slot];
        }

        public void SetItem(Item item, int slot)
        {
            if (slot < 0 || slot > Slots) return;
            _items[slot] = item;
            checkItems();
        }

        public void Clear()
        {
            for (int i = 0; i < Slots; i++) _items[i] = _nullItem;
        }

        public List<Item> GetCopy()
        {
            List<Item> output = new List<Item>();
            foreach (var item in _items) output.Add(item);
            return output;
        }
    }
}
