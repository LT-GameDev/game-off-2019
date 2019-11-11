#pragma warning disable 649

using UnityEngine;

namespace DefaultNamespace
{
    public delegate void ZoneDelegate(int zoneId);
    
    public class SceneLoader : MonoBehaviour
    {
        public static event ZoneDelegate TransitionToZone;
        public static event ZoneDelegate TransitionFromZone;
        
        [SerializeField] private int zoneId;
        [SerializeField] private Modes mode;

        private void OnTriggerEnter()
        {
            switch (mode)
            {
                case Modes.Load:
                    TransitionToZone?.Invoke(zoneId);
                    break;
                
                case Modes.Unload:
                    TransitionFromZone?.Invoke(zoneId);
                    break;
            }
        }
        
        private enum Modes
        {
            Load,
            Unload
        }
    }
}