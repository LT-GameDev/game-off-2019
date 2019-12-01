using System;
using Game.Interactions;
using Game.UI;
using UnityEngine;

namespace Game.Components.Interactables
{
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        private InteractionHintView hintView;
        
        protected virtual void Awake()
        {
            var gameManager = FindObjectOfType<GameManager>();

            hintView = gameManager.GetService<InteractionHintView>();
        }

        public abstract int GetInteractionType();

        public void Notify(bool notifiedState)
        {
            if (notifiedState)
            {
                hintView.DisplayInteraction($"Press 'F' to {NotificationText}");
            }
            else
            {
                hintView.Hide();
            }
        }

        public abstract void Interact();

        protected abstract string NotificationText { get; }
    }
}