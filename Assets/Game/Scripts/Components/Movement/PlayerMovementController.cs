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
        
        [Header("Movement Modes")]
        [SerializeField] private DefaultMovementLogic defaultMovement;

        private Transform myTransform;
        private Action currentMovementUpdate;
        private Vector2 movementInput;

        private Dictionary<MovementMode, Action> movementModes;

        private void Awake()
        {
            myTransform = transform;
            
            movementModes = new Dictionary<MovementMode, Action> {
                { MovementMode.Humanoid, DefaultMovementUpdate }
            };

            SwitchMode(default);
        }

        private void FixedUpdate()
        {
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

        private void SwitchMode(MovementMode mode)
        {
            currentMovementUpdate = movementModes[mode];

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
                // switch mode
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

        private bool CanSwitchToWallRun()
        {
            var position = transform.position;
            
            var right  = transform.right;
            var length = 0.6f;

            if (Physics.Raycast(position, right, out var rightHit, length))
            {
                this.Log().Debug($"Can run on wall: {rightHit.collider.gameObject.name}");
                GizmoDrawer.Draw(new LineGizmo(position, position + length * right, Color.green), 20);

                return true;
            }
            else
            {
                GizmoDrawer.Draw(new LineGizmo(position, position + length * right, Color.red), 20);
            }
            
            if (Physics.Raycast(position, -right, out var leftHit, length))
            {
                this.Log().Debug($"Can run on wall: {leftHit.collider.gameObject.name}");
                GizmoDrawer.Draw(new LineGizmo(position, position - length * right, Color.green), 20);

                return true;
            }
            else
            {
                GizmoDrawer.Draw(new LineGizmo(position, position - length * right, Color.red), 20);
            }

            return false;
        }
    }
}