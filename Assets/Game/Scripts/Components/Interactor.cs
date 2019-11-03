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

        public void Interact()
        {
            if (interactable.IsNull())
                return;
            
            interactable.Interact();
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