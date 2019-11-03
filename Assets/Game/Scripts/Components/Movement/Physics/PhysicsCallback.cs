using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components.Movement.Physics
{
    public abstract class PhysicsCallback
    {
        public abstract void Apply(MovementContext context, float physicsTimeStep);
    }
}