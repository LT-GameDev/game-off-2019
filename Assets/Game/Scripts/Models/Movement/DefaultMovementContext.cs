using UnityEngine;

namespace Game.Models.Movement
{
    public class DefaultMovementContext
    {
        public bool sprint;
        public bool jump;
        public bool walk;
        public bool grounded;
        public Vector3 direction;
        public Vector3 groundNormal;
        public Transform meshRoot;
        public Rigidbody body;
    }
}