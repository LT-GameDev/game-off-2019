#pragma warning disable

using Game.Utility;
using UnityEngine;

namespace Game.Components.Movement
{
    public abstract class MovementController : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private Collider coll;
        
        public virtual Vector3 GetForward()
        {
            var groundVelocity = Vector3.Scale(GameWorld.GetGroundPlane(), body.velocity);

            if (groundVelocity.magnitude > 0.1f)
                return groundVelocity.normalized;

            return body.rotation * Vector3.forward;
        }
        
        public virtual Vector3 GetRight()
        {
            var groundVelocity = Vector3.Scale(GameWorld.GetGroundPlane(), body.velocity);

            if (groundVelocity.magnitude > 0.1f)
                return Vector3.Cross(groundVelocity.normalized, Vector3.up);

            return body.rotation * Vector3.right;
        }

        public virtual Vector3 GetUp()
        {
            return body.rotation * Vector3.up;
        }
    }
}