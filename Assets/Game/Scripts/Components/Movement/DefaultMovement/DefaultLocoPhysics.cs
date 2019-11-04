#pragma warning disable 649

using Game.Models.Movement;
using System;
using Game.Models.Movement.Contexts;
using UnityEngine;

namespace Game.Components.Movement.DefaultMovement
{
    [Serializable]
    public class DefaultLocoPhysics : PhysicsCallback<DefaultMovementContext>
    {
        [SerializeField] private DefaultLocoProperties properties;

        public override void Apply(DefaultMovementContext context, float physicsTimeStep)
        {
            var gravityComponent = Vector3.Scale(new Vector3(0, 1, 0), context.CharacterBody.velocity);
            var existingVelocity = Vector3.Scale(new Vector3(1, 0, 1), context.CharacterBody.velocity);
            var deltaSpeed       = (context.Accelerate ? properties.Acceleration : -properties.Deceleration) * physicsTimeStep;

            // movement direction is available only when player is supposed to be moving, so when slowing down
            // it becomes (0,0,0) causing player to immediately stop due to below multiplication.
            // So this is necessary
            var direction = context.Accelerate ? context.MovementDirection : existingVelocity.normalized; 
            
            // Apply air control penalty if not grounded
            if (!context.GroundCheckState.Grounded)
            {
                context.MovementDirection = Vector3.Lerp(existingVelocity.normalized, context.MovementDirection, properties.AirControl);
                deltaSpeed *= properties.AirControl;
            }
            
            var newSpeed = Mathf.Clamp(existingVelocity.magnitude + deltaSpeed, 0, GetMovementSpeed(context));

            context.CharacterBody.velocity = (direction * newSpeed) + gravityComponent;
        }

        private float GetMovementSpeed(DefaultMovementContext context)
        {
            if (context.Sprinting)
                return properties.MovementSpeed * properties.SprintMultiplier;

            if (context.Walking)
                return properties.MovementSpeed * properties.WalkMultiplier;

            return properties.MovementSpeed;
        }
    }
}