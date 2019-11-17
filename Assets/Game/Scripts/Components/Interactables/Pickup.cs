#pragma warning disable 649

using Game.Enums;
using Game.Interactions;
using Game.Managers;
using Game.Models.Inventory;
using Game.Utility;
using UnityEngine;

namespace Game.Components.Interactables
{
    public class Pickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemModel item;
        
        private InventoryManager inventoryManager;
        
        private void Awake()
        {
            var gameManager = FindObjectOfType<GameManager>();

            inventoryManager = gameManager.GetService<InventoryManager>();

            if (inventoryManager.HasItem(item))
            {
                gameObject.SetActive(false);
            }
        }

        public int GetInteractionType()
        {
            return (int) PlayerInteractions.Pickup;
        }

        public void Notify(bool notifiedState)
        {
            if (notifiedState)
            {
                this.Log().Debug($"Press 'F' to pickup");
            }
        }

        public void Interact()
        {
            inventoryManager.AddItem(item);
            
            gameObject.SetActive(false);
            
            FindObjectOfType<GameManager>().SaveGame();
        }
    }
}
