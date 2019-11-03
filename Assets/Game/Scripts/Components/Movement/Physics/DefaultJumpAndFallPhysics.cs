#pragma warning disable 649

using System;
using Game.Models.Movement;
using UnityEngine;

namespace Game.Components.Movement.Physics
{
    [Serializable]
    public class DefaultJumpAndFallPhysics : PhysicsCallback
    {
        [SerializeField] private DefaultJumpAndFallProperties properties;
        
        public override void Apply(MovementContext context, float physicsTimeStep)
        {
            if (context.characterBody.velocity.y < -0.1f)
            {
                var deltaGravity = physicsTimeStep * properties.AdditionalFallGravity * -Vector3.up;

                context.characterBody.velocity += deltaGravity;
            }
            
            if (context.groundCheckState.Grounded && context.jumping)
            {
                context.characterBody.AddForce(Vector3.up * properties.JumpPower, ForceMode.VelocityChange);
            }
        }
    }
}