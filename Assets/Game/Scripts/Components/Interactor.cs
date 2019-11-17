#pragma warning disable 649

using Game.Interactions;
using Game.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Components
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private bool shouldNotifyUI;
        
        private IInteractable interactable;

        public int Interact()
        {
            if (interactable.IsNull())
                return -1;
            
            interactable.Interact();

            return interactable.GetInteractionType();
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            interactable = collider.GetComponent<IInteractable>();
            
            if (!interactable.IsNull() && shouldNotifyUI)
                interactable.Notify(true);
        }

        private void OnTriggerExit(Collider collider)
        {
            var interactableInstance = collider.GetComponent<IInteractable>();

            if (interactableInstance == interactable)
            {
                interactable = null;
                
                interactableInstance.Notify(false);
            }
        }
    }
}