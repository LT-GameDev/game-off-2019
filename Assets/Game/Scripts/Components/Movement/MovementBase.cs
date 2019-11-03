using UnityEngine;
using System.Collections;

namespace Game.Components.Movement
{
    public abstract class MovementBase : MonoBehaviour
    {
        public abstract void Move(Vector2 input);
    }
}