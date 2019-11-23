#pragma warning disable 649

using Game.Enums;
using Game.Interactions;
using Game.Managers;
using Game.Models.Inventory;
using Game.Utility;
using UnityEngine;

namespace Game.Components.Interactables
{
    public class Pickup : BaseInteractable
    {
        [SerializeField] private ItemModel item;
        
        private InventoryManager inventoryManager;
        
        protected override void Awake()
        {
            base.Awake();
            
            var gameManager = FindObjectOfType<GameManager>();

            inventoryManager = gameManager.GetService<InventoryManager>();

            if (inventoryManager.HasItem(item))
            {
                gameObject.SetActive(false);
            }
        }

        public override int GetInteractionType()
        {
            return (int) PlayerInteractions.Pickup;
        }

        public override void Interact()
        {
            inventoryManager.AddItem(item);
            
            gameObject.SetActive(false);
            
            FindObjectOfType<GameManager>().SaveGame();
            Notify(false);
        }

        protected override string NotificationText => $"take {item.ItemName}";
    }
}
