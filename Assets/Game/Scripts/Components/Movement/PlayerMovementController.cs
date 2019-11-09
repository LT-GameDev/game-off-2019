#pragma warning disable 649

using System;
using UnityEngine;
using System.Collections.Generic;
using Game.Components.Movement.DefaultMovement;
using Game.Utility;

namespace Game.Components.Movement
{
    public enum MovementMode
    {
        Humanoid,
        WallMovement
    }

    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private Transform childMeshRoot;
        [SerializeField] private Rigidbody characterBody;
        [SerializeField] private CapsuleCollider characterCollider;
        [SerializeField] private GroundCheck groundChecker;
        [SerializeField] private float wallRunRequiredMinSpeed;
        [SerializeField] private float maxWallHandlingAngle;
        
        [Header("Movement Modes")]
        [SerializeField] private DefaultMovementLogic defaultMovement;

        private Transform myTransform;
        private MovementMode currentMovementMode;
        private Action currentMovementUpdate;
        private Vector2 movementInput;

        private Dictionary<MovementMode, Action> movementModes;

        private void Awake()
        {
            myTransform = transform;
            
            movementModes = new Dictionary<MovementMode, Action> {
                { MovementMode.Humanoid, DefaultMovementUpdate },
                { MovementMode.WallMovement, WallMovementUpdate }
            };

            SwitchMode(default);
        }

        private void FixedUpdate()
        {
            GlobalGroundCheck();
            
            currentMovementUpdate?.Invoke();
        }

        public void Move(Vector2 movementInput)
        {
            this.movementInput = movementInput;
        }

        public void Jump()
        {
            defaultMovement.Context.Jumping = true;
        }

        public void SetSprinting(bool sprinting)
        {
            defaultMovement.Context.Sprinting = sprinting;
        }

        public void SetWalking(bool walking)
        {
            defaultMovement.Context.Walking = walking;
        }

        private void GlobalGroundCheck()
        {
            var position   = characterBody.position + characterBody.rotation * characterCollider.center;
            var halfHeight = characterCollider.height / 2;
            
            groundChecker.Check(position, halfHeight, characterCollider.radius);
        }

        private void SwitchMode(MovementMode mode)
        {
            currentMovementUpdate = movementModes[mode];
            currentMovementMode   = mode;

            switch (mode)
            {
                case MovementMode.Humanoid:
                    this.Log().Debug("Switch to default movement!", nameof(SwitchMode));
                    defaultMovement.Initialize();
                    break;
            }
        }

        private void DefaultMovementUpdate()
        {
            if (CanSwitchToWallRun())
            {
                SwitchMode(MovementMode.WallMovement);
                return;
            }
            
            var context = defaultMovement.Context;
            
            context.Accelerate = movementInput.magnitude > 0.1f;

            context.MovementDirection = (myTransform.forward * movementInput.y + myTransform.right * movementInput.x).normalized;

            defaultMovement.Run(context, Time.fixedDeltaTime);
            
            // Consume state
            context.MovementDirection = Vector3.zero;
            context.Accelerate        = false;
            context.Jumping           = false;
        }

        private void WallMovementUpdate()
        {
            if (CanSwitchToDefault())
            {
                SwitchMode(MovementMode.Humanoid);
                return;
            }
        }

        private bool CanSwitchToWallRun()
        {
            var position = transform.position;
            var length   = 0.6f;
            
            var right  = Vector3.Cross(characterBody.velocity.normalized, Vector3.up);

            // Check whether character has any surfaces
            // it can perform wall movement on
            var rightCheck = InternalCheck(right);
            var leftCheck  = InternalCheck(-right);

            return (rightCheck || leftCheck) && !(rightCheck && leftCheck);

            
            bool InternalCheck(Vector3 direction)
            {
                if (Physics.Raycast(position, direction, out var hit, length))
                {
                    this.Log().Debug($"Can run on wall: {hit.collider.gameObject.name}");

                    var angle = Vector3.SignedAngle(hit.normal, -direction, Vector3.up);

                    // Check if character approached the wall withing snapping threshold
                    // and is running at or above the required minimum speed
                    if (Mathf.Abs(angle) < maxWallHandlingAngle && characterBody.velocity.magnitude > wallRunRequiredMinSpeed)
                    {
                        GizmoDrawer.Draw(new LineGizmo(position, position + length * direction, Color.green), 20);
                        return true;
                    }
                    
                    GizmoDrawer.Draw(new LineGizmo(position, position + length * direction, Color.blue), 20);
                    return false;
                }
            
                GizmoDrawer.Draw(new LineGizmo(position, position + length * direction, Color.red), 20);

                return false;
            }
        }
        
        private bool CanSwitchToDefault()
        {
            if (currentMovementMode == MovementMode.WallMovement && groundChecker.Grounded)
            {
                this.Log().Debug("Can move in default mode");
                return true;
            }

            return false;
        }

        public bool Grounded => groundChecker.Grounded;
    }
}