#pragma warning disable 649

using Game.Enums;
using Game.Interactions;
using Game.Utility;
using UnityEngine;

namespace Game.Components.Interactables
{
    public class Pickup : MonoBehaviour, IInteractable
    {
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
            // Add item to player inventory
            
            gameObject.SetActive(false);
        }
    }
}
