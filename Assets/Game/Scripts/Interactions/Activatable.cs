using UnityEngine;

namespace Game.Interactions
{
    public abstract class Activatable : MonoBehaviour
    {
        public abstract void Activate(bool state);
    }
}