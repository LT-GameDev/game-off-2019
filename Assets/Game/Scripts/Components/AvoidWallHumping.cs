#pragma warning disable 649

using System;
using UnityEngine;

namespace Game.Components
{
    public class AvoidWallHumping : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Rigidbody body;
        [SerializeField] private float maxClimbAngle;

        private void OnCollisionEnter(Collision collision)
        {
            // Check if colliding object is in a wall layer
            if (((1 << collision.gameObject.layer) & layerMask) < 1)
                return;
            
            AdjustDirection(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            // Check if colliding object is in a wall layer
            if (((1 << collision.gameObject.layer) & layerMask) < 1)
                return;
            
            AdjustDirection(collision);
        }

        private void AdjustDirection(Collision collision)
        {
            var myTransform = body.transform;
            var groundAxis  = new Vector3(1, 0, 1);

            var normal = collision.contacts[0].normal;
            var up     = myTransform.up;
            
            // Get angle between surface normal and y axis on x axis
            var upToNormalAngle = Vector3.SignedAngle(Vector3.up, normal, Vector3.right);

            // If this angle is lower than climb angle, we can climb it, as we haven't exceeded max climb angle
            if (Mathf.Abs(upToNormalAngle) <= maxClimbAngle)
                return;
            
            // Project normal to ground plane and get perpendicular vector to it (tangent)
            var groundNormal = Vector3.Scale(groundAxis, normal);
            var tangent      = Vector3.Cross(groundNormal, up);
            var angle        = Vector3.SignedAngle(groundNormal, GetBodyForward(), up);

            // If angle is positive, projection of velocity on tangent plane should be the inverse of tangent vector
            if (angle > 0)
            {
                body.rotation = Quaternion.LookRotation(-tangent, myTransform.up);
            }
            else
            {
                // Otherwise, just use tangent vector to avoid the surface
                body.rotation = Quaternion.LookRotation(tangent, myTransform.up);
            }

            var forward = body.rotation * Vector3.forward;
            
            body.velocity = forward * body.velocity.magnitude;
        }

        private Vector3 GetBodyForward() => body.rotation * Vector3.forward;
    }
}