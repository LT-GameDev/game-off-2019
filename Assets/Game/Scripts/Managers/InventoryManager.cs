using System.Collections.Generic;
using Game.Models;
using Game.Models.Inventory;

namespace Game.Managers
{
    public class InventoryManager
    {
        private List<ItemModel> items;

        public InventoryManager()
        {
            items = new List<ItemModel>();
        }

        public void InitializeInventory(List<ItemModel> items = null)
        {
            if (items == null)
            {
                this.items.Clear();
                return;
            }
            
            this.items = new List<ItemModel>(items);
        }

        public void AddItem(ItemModel item)
        {
            items.Add(item);
        }

        public bool HasItem(ItemModel item)
        {
            return items.Contains(item);
        }

        public ItemModel GetItemAtSlot(int slot)
        {
            return items[slot];
        }

        public void RemoveItem(ItemModel item)
        {
            items.Remove(item);
        }

        public void RemoveItem(int slot)
        {
            items.RemoveAt(slot);
        }

        public List<ItemModel> GetItems()
        {
            return new List<ItemModel>(items);
        }
    }
}