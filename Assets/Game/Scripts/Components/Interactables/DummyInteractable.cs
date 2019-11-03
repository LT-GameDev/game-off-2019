using System.Collections;
using System.Collections.Generic;
using Game.Interactions;
using UnityEngine;

namespace Game.Components.Interactables
{
    public class DummyInteractable : MonoBehaviour, IInteractable
    {
        public void Notify(bool notifiedState)
        {
            if (notifiedState)
            {
                Debug.Log($"Press 'F' to destroy {gameObject.name}");
            }
        }

        public void Interact()
        {
            Destroy(gameObject);
        }
    }
}