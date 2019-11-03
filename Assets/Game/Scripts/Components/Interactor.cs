#pragma warning disable 649

using Game.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Components
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private UnityEvent canInteract;
        [SerializeField] private UnityEvent cannotInteract;
        
        private IInteractable interactable;

        public void Interact()
        {
            interactable?.Interact();
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            interactable = collider.GetComponent<IInteractable>();
            
            if (interactable != null)
                canInteract.Invoke();
        }

        private void OnTriggerExit(Collider collider)
        {
            var interactableInstance = collider.GetComponent<IInteractable>();

            if (interactableInstance == interactable)
            {
                interactable = null;
                
                cannotInteract.Invoke();
            }
        }
    }
}