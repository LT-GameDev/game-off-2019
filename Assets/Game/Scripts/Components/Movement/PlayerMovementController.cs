#pragma warning disable 649

using System;
using UnityEngine;
using System.Collections.Generic;
using Game.Components.Movement.DefaultMovement;
using Game.Components.Movement.MovementControl;
using Game.Components.Movement.Wallrun;
using Game.Models.Movement.Contexts;
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
        [SerializeField] private float wallRunBeginAngle;
        [SerializeField] private float wallRunBeginSpeed;
        
        [Header("Movement Modes")]
        [SerializeField] private DefaultMovementLogic defaultMovement;
        [SerializeField] private WallMovementLogic wallMovement;

        private Transform myTransform;
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
            currentMovementUpdate?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude < wallRunBeginSpeed)
                return;
            
            var groundAxis  = new Vector3(1, 0, 1);

            var normal  = collision.contacts[0].normal;
            var tangent = Vector3.Cross(normal, Vector3.forward);
            var up      = myTransform.up;
            
            // Get angle between surface tangent and x axis on y axis
            var tangentToForwrdAngle    = Vector3.SignedAngle(Vector3.right, normal, Vector3.up);
            var canSwitchToWallMovement = Mathf.Abs(tangentToForwrdAngle) > 180 - wallRunBeginAngle && wallMovement.Context.Sprinting && movementInput.magnitude > 0;
            
            this.Log().Debug(
                $"angle: {tangentToForwrdAngle}. can switch to wall movement: {canSwitchToWallMovement} (abs(tangentToForwardAngle) < wallRunBeginAngle)", 
                nameof(OnCollisionEnter));

            // If this angle is higher than the wall run begin angle, we cannot wall run!
            if (!canSwitchToWallMovement)
                return;
            
            SwitchMode(MovementMode.WallMovement);
        }

        public void Move(Vector2 movementInput)
        {
            this.movementInput = movementInput;
        }

        public void Jump()
        {
            defaultMovement.Context.Jumping = true;
            wallMovement.Context.Jump = true;
        }

        public void SetSprinting(bool sprinting)
        {
            defaultMovement.Context.Sprinting = sprinting;
            wallMovement.Context.Sprinting = sprinting;
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
                
                case MovementMode.WallMovement:
                    this.Log().Debug("Switch to wall movement",nameof(SwitchMode));
                    wallMovement.Initialize();
                    break;
            }
        }

        private void DefaultMovementUpdate()
        {
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
            var context = wallMovement.Context;
            
            context.MovementDirection = (myTransform.forward * movementInput.y + myTransform.right * movementInput.x).normalized;
            
            wallMovement.Run(context, Time.fixedTime);
            
            // Consume state
            context.MovementDirection = Vector3.zero;
        }
    }
}