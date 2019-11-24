#pragma warning disable 649

using System;
using Game.Components.Movement.Interface;
using Game.Models.Movement;
using Game.Utility;
using UnityEngine;

namespace Game.Components.Movement
{
    [Serializable]
    public class DefaultMovement : IMovementLogic<DefaultMovementContext>
    {
        [Header("Movement Properties")]
        [SerializeField] private float fallGravityFactor;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float jumpPower;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float airControl;
        [SerializeField] private float walkingFactor;
        [SerializeField] private float sprintingFactor;
        [SerializeField] private float climbAngle;
        
        [Header("Damping")]
        [SerializeField] private float lookSpeed;
        [SerializeField] private float upRotationSpeed;
        
        private DefaultMovementContext movementContext;
        
        public void Initialize(DefaultMovementContext context)
        {
            movementContext = context;
        }

        public void WarmUp()
        {
            movementContext.body.useGravity = true;
        }

        public void Run(float deltaTime)
        {
            var groundPlane = GameWorld.GetGroundPlane();
            var gravityAxis = GameWorld.GetGravityAxis();
            
            var direction  = movementContext.direction;
            var deltaSpeed = GetAcceleration(direction) * GetControlFactor() * deltaTime;

            var body     = movementContext.body;
            var meshRoot = movementContext.meshRoot;
            var rotation = movementContext.body.rotation;

            var groundComponent  = Vector3.Scale(groundPlane, body.velocity);
            var gravityComponent = Vector3.Scale(gravityAxis, body.velocity);
            
            var newSpeed = Mathf.Clamp(groundComponent.magnitude + deltaSpeed, 0, GetSpeedLimit());

            if (movementContext.jump && movementContext.grounded)
            {
                body.AddForce(gravityAxis * jumpPower, ForceMode.VelocityChange);
            }

            var gravityAxisVelocityAngle    = Vector3.SignedAngle(gravityAxis, gravityComponent, Vector3.forward);
            var absGravityAxisVelocityAngle = Mathf.Abs(gravityAxisVelocityAngle);

            if (!movementContext.grounded && absGravityAxisVelocityAngle > 90)
            {
                var deltaGravity    = fallGravityFactor * deltaTime;
                var newGravitySpeed = gravityComponent.magnitude + deltaGravity;

                gravityComponent = gravityComponent.normalized * newGravitySpeed;
            }

            var meshRotation  = meshRoot.rotation;
            var lookDirection = direction.magnitude > 0.1f ? direction : meshRotation * Vector3.forward;
            
            var lookRotation  = Quaternion.LookRotation(lookDirection, rotation * Vector3.up);   
            meshRoot.rotation = Quaternion.Lerp(meshRotation, lookRotation, lookSpeed * GetControlFactor() * deltaTime);
            
            body.velocity = meshRoot.rotation * Vector3.forward * newSpeed + gravityComponent;

            if (movementContext.grounded && !movementContext.jump && body.velocity.y <= 0)
            {
                var up = meshRoot.up;
                
                var tangent = Vector3.Cross(up, movementContext.groundNormal).normalized;
                var angle   = Vector3.SignedAngle(up, movementContext.groundNormal, tangent);

                if (Mathf.Abs(angle) <= climbAngle)
                {
                    var normalization    = Quaternion.Euler(angle * tangent);
                    var surfaceDirection = normalization * meshRotation * Vector3.forward;

                    body.velocity = surfaceDirection * newSpeed;
                }
            }
        }

        private float GetAcceleration(Vector3 input)
        {
            return input.magnitude > 0.1f ? acceleration : -deceleration;
        }

        private float GetControlFactor()
        {
            return movementContext.grounded ? 1 : airControl;
        }

        private float GetSpeedLimit()
        {
            if (movementContext.sprint)
                return maxSpeed * sprintingFactor;

            if (movementContext.walk)
                return maxSpeed * walkingFactor;

            return maxSpeed;
        }
    }
}