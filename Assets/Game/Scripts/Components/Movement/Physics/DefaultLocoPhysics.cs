#pragma warning disable 649

using Game.Models.Movement;
using System;
using UnityEngine;

namespace Game.Components.Movement.Physics
{
    [Serializable]
    public class DefaultLocoPhysics : PhysicsCallback
    {
        [SerializeField] private DefaultLocoProperties properties;

        public override void Apply(MovementContext context, float physicsTimeStep)
        {
            var gravityComponent = Vector3.Scale(new Vector3(0, 1, 0), context.characterBody.velocity);
            var existingVelocity = Vector3.Scale(new Vector3(1, 0, 1), context.characterBody.velocity);
            var deltaSpeed       = (context.accelerate ? properties.Acceleration : -properties.Deceleration) * physicsTimeStep;
            
            // Apply air control penalty if not grounded
            if (!context.groundCheckState.Grounded)
            {
                context.movementDirection = Vector3.Lerp(existingVelocity.normalized, context.movementDirection, properties.AirControl);
                deltaSpeed *= properties.AirControl;
            }
            
            var newSpeed = Mathf.Clamp(existingVelocity.magnitude + deltaSpeed, 0, GetMovementSpeed(context));

            context.characterBody.velocity = (context.movementDirection * newSpeed) + gravityComponent;
        }

        private float GetMovementSpeed(MovementContext context)
        {
            if (context.sprinting)
                return properties.MovementSpeed * properties.SprintMultiplier;

            if (context.walking)
                return properties.MovementSpeed * properties.WalkMultiplier;

            return properties.MovementSpeed;
        }
    }
}