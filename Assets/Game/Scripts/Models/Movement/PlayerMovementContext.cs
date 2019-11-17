using UnityEngine;

namespace Game.Models.Movement
{
    public class PlayerMovementContext : DefaultMovementContext
    {
        public bool jumpOff;
        public bool falling;
        public Vector3 wallNormal;
        public float wallDistance;
    }
}