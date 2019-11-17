#pragma warning disable 649

using System;
using Game.Components.Movement.Interface;
using Game.Models.Movement;
using Game.Utility;
using UnityEngine;

namespace Game.Components.Movement
{
    [Serializable]
    public class WallMovement : IMovementLogic<PlayerMovementContext>
    {
        [SerializeField] private float minRequiredSpeed;
        [SerializeField] private float climbVelocity;
        [SerializeField] private float fallAcceleration;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float jumpPower;
        
        private PlayerMovementContext movementContext;
        private Vector3 previousNormal;
        private float upwardsVector;
        
        public void Initialize(PlayerMovementContext context)
        {
            movementContext = context;
        }

        public void WarmUp()
        {
            movementContext.body.useGravity = false;
            movementContext.falling         = false;
            
            previousNormal = movementContext.wallNormal;
            upwardsVector  = climbVelocity;

            movementContext.jumpOff = false;
        }

        public void Run(float deltaTime)
        {
            if (movementContext.falling)
            {
                return;
            }

            if (movementContext.jumpOff)
            {
                var groundPlacneVelocity = Vector3.Scale(new Vector3(1, 0, 1), movementContext.body.velocity);
                var jumpOffUpRotation    = Quaternion.LookRotation(groundPlacneVelocity.normalized, Vector3.up);
            
                movementContext.meshRoot.rotation = Quaternion.Lerp(movementContext.meshRoot.rotation, jumpOffUpRotation, rotationSpeed * deltaTime);
                
                return;
            }

            if (!movementContext.sprint)
            {
                movementContext.falling = true;
                return;
            }
            
            var result = Raycast();

            if (!result.success)
            {
                movementContext.falling = true;
                return;
            }

            if (movementContext.jump)
            {
                var velocity          = movementContext.body.velocity;
                var velocityDirection = (movementContext.wallNormal + velocity.normalized + 0.5f * climbVelocity * Vector3.up).normalized;
                
                movementContext.body.AddForce(velocityDirection * jumpPower, ForceMode.VelocityChange);
                movementContext.body.useGravity = true;

                movementContext.jumpOff = true;
                return;
            }

            movementContext.wallNormal   = result.hit.normal;
            movementContext.wallDistance = result.hit.distance;
            
            var deltaGravity     = fallAcceleration * deltaTime;
            var newClimbVelocity = Vector3.up * Mathf.Clamp(upwardsVector - deltaGravity, -float.MaxValue, climbVelocity);
            var existingVelocity = movementContext.body.velocity;
            
            if (Vector3.Scale(GameWorld.GetGroundPlane(), existingVelocity).magnitude < minRequiredSpeed)
            {
                movementContext.falling = true;
                return;
            }
            
            var tangent = Vector3.Cross(Vector3.up, movementContext.wallNormal);
            var angle   = Vector3.SignedAngle(existingVelocity.normalized, movementContext.wallNormal, Vector3.up);

            var upRotation = Quaternion.identity;

            if (angle > 0)
            {
                movementContext.body.velocity = Vector3.ClampMagnitude(-tangent * existingVelocity.magnitude, 12);
                upRotation *= Quaternion.Euler(tangent * 30);
            }
            else
            {
                movementContext.body.velocity = Vector3.ClampMagnitude(tangent * existingVelocity.magnitude, 12);
                upRotation *= Quaternion.Euler(tangent * 30);
            }
            
            movementContext.body.velocity += newClimbVelocity;

            var rotation = Quaternion.FromToRotation(previousNormal, movementContext.wallNormal);

            var normalizedVelocity = movementContext.body.velocity.normalized;
            var lookRotation       = Quaternion.LookRotation(normalizedVelocity, upRotation * Vector3.up);
            
            movementContext.meshRoot.rotation = Quaternion.Lerp(movementContext.meshRoot.rotation, lookRotation, rotationSpeed * deltaTime);

            movementContext.body.velocity = rotation * movementContext.body.velocity;
            previousNormal = movementContext.wallNormal;
            
            upwardsVector -= deltaGravity;
        }

        private (bool success, RaycastHit hit) Raycast()
        {
            if (Physics.Raycast(movementContext.body.position, -movementContext.wallNormal, out var hit,
                movementContext.wallDistance * 1.4f))
            {
                return (true, hit);
            }

            return (false, default);
        }
    }
}