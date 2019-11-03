using System;
using Game.Models.Movement;
using UnityEngine;

namespace Game.Components.Movement.Physics
{
    [Serializable]
    public class HumanoidJumpAndFallPhysics : PhysicsCallback
    {
        [SerializeField] private HumanoidJumpAndFallProperties properties;
        
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