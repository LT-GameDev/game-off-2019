#pragma warning disable 649

using System;
using UnityEngine;
using System.Collections.Generic;
using Game.Cameras;
using Game.Components.Movement.Interface;
using Game.Models.Movement;
using Game.Utility;

namespace Game.Components.Movement
{
    public enum MovementMode
    {
        Humanoid,
        WallMovement
    }

    public class PlayerMovementController : MovementController
    {
        [Header("Properties")]
        [SerializeField] private Transform childMeshRoot;
        [SerializeField] private Rigidbody characterBody;
        [SerializeField] private CapsuleCollider characterCollider;
        [SerializeField] private GroundCheck groundChecker;
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private LayerMask wallLayers;
        
        [Header("Movement Modes")]
        [SerializeField] private DefaultMovement defaultMovement;
        [SerializeField] private WallMovement wallMovement;

        private Transform myTransform;
        private MovementMode currentMovementMode;
        private IMovementLogic currentMovementLogic;
        private PlayerMovementContext context;

        private Dictionary<MovementMode, IMovementLogic> movementModes;

        private void Awake()
        {
            myTransform = transform;

            playerCamera = FindObjectOfType<PlayerCamera>();
            
            context       = new PlayerMovementContext();
            movementModes = new Dictionary<MovementMode, IMovementLogic> {
                { MovementMode.Humanoid, defaultMovement },
                { MovementMode.WallMovement, wallMovement }
            };

            context.meshRoot = childMeshRoot;
            context.body     = characterBody;
            
            defaultMovement.Initialize(context);
            wallMovement.Initialize(context);

            SwitchMode(default);
        }

        private void FixedUpdate()
        {
            GlobalGroundCheck();
            ManageTransitions();
            
            currentMovementLogic?.Run(Time.fixedDeltaTime);

            context.direction = Vector3.zero;
            context.jump      = false;
        }

        public void Move(Vector2 movementInput)
        {
            context.direction = GetForward() * movementInput.y + GetRight() * movementInput.x;
        }

        public void Jump()
        {
            context.jump = true;
        }

        public void SetSprinting(bool sprinting)
        {
            context.sprint = sprinting;
        }

        public void SetWalking(bool walking)
        {
            context.walk = walking;
        }

        private void DefaultMovementCallback()
        {
            defaultMovement.Run(Time.fixedDeltaTime);
        }

        private void WallMovementCallback()
        {
            wallMovement.Run(Time.fixedDeltaTime);
        }

        private void ManageTransitions()
        {
            if (ShouldEnterWallMovement())
                SwitchMode(MovementMode.WallMovement);
            else if (ShouldLeaveWallMovement())
                SwitchMode(MovementMode.Humanoid);
        }

        private void GlobalGroundCheck()
        {
            var position   = characterBody.position + characterBody.rotation * characterCollider.center;
            var halfHeight = characterCollider.height / 2;
            
            groundChecker.Check(position, halfHeight * 0.9f, characterCollider.radius * 0.5f);

            context.grounded = groundChecker.Grounded;
        }

        private void SwitchMode(MovementMode mode)
        {
            currentMovementLogic = movementModes[mode];
            currentMovementMode  = mode;
            
            currentMovementLogic.WarmUp();
            
            this.Log().Debug($"Switch to {mode} movement!", nameof(SwitchMode));
        }

        public override Vector3 GetForward()
        {
            var forward          = characterBody.position - playerCamera.Position;
            var projectedForward = Vector3.Scale(GameWorld.GetGroundPlane(), forward);

            return projectedForward.normalized;
        }
        
        public override Vector3 GetRight()
        {
            var right = Vector3.Cross(Vector3.up, GetForward());

            return right;
        }

        private bool ShouldEnterWallMovement()
        {
            // If player is grounded or is already in wall movement mode, cannot switch to wall movement
            if (context.grounded || currentMovementMode == MovementMode.WallMovement)
                return false;

            var right  = GetRight();
            var length = characterCollider.radius + 0.02f;
            
            var position = characterBody.position;

            var rightResult = Raycast(right);
            var leftResult  = Raycast(-right);

            GizmoDrawer.Draw(new LineGizmo(position, position + right * length, rightResult.success ? Color.green : Color.red));
            GizmoDrawer.Draw(new LineGizmo(position, position - right * length, leftResult.success ? Color.green : Color.red));

            // Cannot snap to wall if cornered or in a tight place
            if (rightResult.success && leftResult.success)
                return false;

            if (rightResult.success)
            {
                context.wallNormal   = rightResult.hit.normal;
                context.wallDistance = rightResult.hit.distance;
                
                return true;
            }
            
            if (leftResult.success)
            {
                context.wallNormal   = leftResult.hit.normal;
                context.wallDistance = leftResult.hit.distance;
                
                return true;
            }

            return false;


            (bool success, RaycastHit hit) Raycast(Vector3 direction)
            {
                if (Physics.Raycast(position, direction, out var hit, length, wallLayers.value))
                {
                    return (true, hit);
                }
                
                return (false, default);
            }
        }

        private bool ShouldLeaveWallMovement()
        {
            return (context.grounded || context.falling) && currentMovementMode == MovementMode.WallMovement;
        }

        public bool Grounded => (currentMovementMode == MovementMode.Humanoid && context.grounded) || 
                                (currentMovementMode == MovementMode.WallMovement && !context.falling);
    }
}