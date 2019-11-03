using Game.Components.Movement.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components.Movement
{
    [Serializable]
    public abstract class CharacterMovement
    {
        public void ApplyPhysics(MovementContext context, float deltaTime)
        {
            var gravityComponent = Vector3.Scale(new Vector3(0, 1, 0), context.characterBody.velocity);

            LocoPhysics?.Apply(context, deltaTime);
            JumpAndFallPhysics?.Apply(context,deltaTime);
        }

        public abstract MovementContext InitializeContext(MovementContext context);

        protected abstract PhysicsCallback LocoPhysics { get; }
        protected abstract PhysicsCallback JumpAndFallPhysics { get; }
    }
}