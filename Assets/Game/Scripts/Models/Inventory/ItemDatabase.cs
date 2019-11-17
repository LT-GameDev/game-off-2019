#pragma warning disable 649

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models.Inventory
{
    [Serializable]
    public struct ItemDatabase
    {
        [SerializeField] private List<ItemModel> items;

        public ItemModel GetById(int id)
        {
            foreach (var item in items)
                if (item.ItemId == id)
                    return item;

            return default;
        }
    }
}