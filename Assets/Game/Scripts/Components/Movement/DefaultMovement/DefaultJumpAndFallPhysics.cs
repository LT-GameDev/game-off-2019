#pragma warning disable 649

using System;
using Game.Models.Movement;
using Game.Models.Movement.Contexts;
using UnityEngine;

namespace Game.Components.Movement.DefaultMovement
{
    [Serializable]
    public class DefaultJumpAndFallPhysics : PhysicsCallback<DefaultMovementContext>
    {
        [SerializeField] private DefaultJumpAndFallProperties properties;
        
        public override void Apply(DefaultMovementContext context, float physicsTimeStep)
        {
            if (context.CharacterBody.velocity.y < -0.1f)
            {
                var deltaGravity = physicsTimeStep * properties.AdditionalFallGravity * -Vector3.up;

                context.CharacterBody.velocity += deltaGravity;
            }
            
            if (context.GroundCheckState.Grounded && context.Jumping)
            {
                context.CharacterBody.AddForce(Vector3.up * properties.JumpPower, ForceMode.VelocityChange);
            }
        }
    }
}