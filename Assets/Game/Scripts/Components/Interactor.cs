#pragma warning disable 649

using Game.Interactions;
using UnityEngine;

namespace Game.Components
{
    public class Interactor : MonoBehaviour
    {
        private IInteractable interactable;

        public void Interact()
        {
            interactable?.Interact();
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            interactable = collider.GetComponent<IInteractable>();
        }

        private void OnTriggerExit(Collider collider)
        {
            var interactableInstance = collider.GetComponent<IInteractable>();
            
            if (interactableInstance == interactable)
                interactable = null;
        }
    }
}