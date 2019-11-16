using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Game.Events
{
    [CreateAssetMenu(menuName = "Events/Simple Event")]
    public class GameEvent : ScriptableObject
    {
        private List<EventListener> listeners = new List<EventListener>();

        public void AddListener(EventListener listenerComponent)
        {
            listeners.Add(listenerComponent);
        }

        public void RemoveListener(EventListener listenerComponent)
        {
            listeners.Remove(listenerComponent);
        }

#if ODIN_INSPECTOR
        [Button(ButtonSizes.Medium)]
#endif
        public void Raise()
        {
            foreach (var listener in listeners)
            {
                listener.HandleEvent(this);
            }
        }
    }
}