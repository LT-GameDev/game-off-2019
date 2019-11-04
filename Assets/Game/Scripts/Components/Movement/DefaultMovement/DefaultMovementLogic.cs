#pragma warning disable 649

using UnityEngine;
using System;
using Game.Components.Movement.MovementControl;
using Game.Models.Movement.Contexts;
using Game.Utility;

namespace Game.Components.Movement.DefaultMovement
{
    
    [Serializable]
    public class DefaultMovementLogic : CharacterMovementLogic<DefaultMovementContext>
    {
        [Header("Components")]
        [SerializeField] private Transform mesh;
        [SerializeField] private Rigidbody body;
        [SerializeField] private CapsuleCollider coll;
        
        [Header("Ground Checking and Course Correction")]
        [SerializeField] private GroundCheck groundChecker;
        
        [Header("Movement Physics")]
        [SerializeField] private DefaultLocoPhysics loco;
        [SerializeField] private DefaultJumpAndFallPhysics jumpAndFall;

        [Header("Properties")]
        [SerializeField] private float smoothCourseSpeed;

        public override DefaultMovementContext CreateContext()
        {
            var context = new DefaultMovementContext();
            
            context.CharacterBody    = body;
            context.GroundCheckState = groundChecker;

            return context;
        }

        public override DefaultMovementContext Preprocess(DefaultMovementContext context)
        {
            var position   = body.position + body.rotation * coll.center;
            var halfHeight = coll.height / 2;
            
            groundChecker.Check(position, halfHeight, coll.radius);

            return context;
        }

        public override void Postprocess(DefaultMovementContext context)
        {
            // Make character face where it is going smoothly
            if (context.MovementDirection.magnitude > 0.1f)
            {
                var lookRotation = Quaternion.LookRotation(context.MovementDirection, mesh.up);

                mesh.rotation = Quaternion.Lerp(mesh.rotation, lookRotation, smoothCourseSpeed * DeltaTime);
            }
        }

        protected override PhysicsCallback<DefaultMovementContext> LocoPhysics => loco;
        protected override PhysicsCallback<DefaultMovementContext> JumpAndFallPhysics => jumpAndFall;
    }
}