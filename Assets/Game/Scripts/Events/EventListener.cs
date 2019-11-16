using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Events
{
    public class EventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent gameEvent;
        [SerializeField] private HandleGameEvent handlers;


        private void OnEnable()
        {
            gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }


        public void HandleEvent(GameEvent gameEvent)
        {
            handlers.Invoke(gameEvent);
        }


        [Serializable]
        private class HandleGameEvent : UnityEvent<GameEvent> { }
    }
}