#pragma warning disable 649

using UnityEngine;
using System.Collections;
using Game.Components.Movement.Physics;
using System;
using Game.Utility;

namespace Game.Components.Movement
{
    [Serializable]
    public class HumanoidMovement : CharacterMovement, IGroundable
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private CapsuleCollider coll;
        [SerializeField] private GroundCheck groundChecker;
        [SerializeField] private HumanoidLocoPhysics loco;
        [SerializeField] private HumanoidJumpAndFallPhysics jumpAndFall;

        public override MovementContext InitializeContext(MovementContext context)
        {
            context.characterBody    = body;
            context.groundCheckState = groundChecker;

            return context;
        }

        public void GroundCheck()
        {
            var position   = body.position + body.rotation * coll.center;
            var halfHeight = coll.height / 2;
            
            groundChecker.Check(position, halfHeight, coll.radius);
        }

        protected override PhysicsCallback LocoPhysics => loco;
        protected override PhysicsCallback JumpAndFallPhysics => jumpAndFall;
    }
}