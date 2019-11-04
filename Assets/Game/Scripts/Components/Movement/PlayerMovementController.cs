#pragma warning disable 649

using UnityEngine;
using System.Collections.Generic;
using Game.Components.Movement.DefaultMovement;
using Game.Components.Movement.MovementControl;
using Game.Models.Movement.Contexts;

namespace Game.Components.Movement
{
    public enum MovementMode
    {
        Humanoid
    }

    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private Transform childMeshRoot;
        
        [Header("Movement Modes")]
        [SerializeField] private DefaultMovementLogic defaultMovement;

        private Transform myTransform;
        private IMovementLogic<DefaultMovementContext> currentMovementMode;
        private DefaultMovementContext context;
        private Vector2 movementInput;

        private Dictionary<MovementMode, IMovementLogic<DefaultMovementContext>> movementModes;

        private void Awake()
        {
            myTransform = transform;
            
            movementModes = new Dictionary<MovementMode, IMovementLogic<DefaultMovementContext>> {
                { MovementMode.Humanoid, defaultMovement }
            };

            SwitchMode(default);
        }

        private void FixedUpdate()
        {
            context.Accelerate = movementInput.magnitude > 0.1f;

            context.MovementDirection = (myTransform.forward * movementInput.y + myTransform.right * movementInput.x).normalized;

            currentMovementMode?.Run(context, Time.fixedDeltaTime);
            
            // Consume state
            context.MovementDirection = Vector3.zero;
            context.Accelerate        = false;
            context.Jumping           = false;
        }

        public void Move(Vector2 movementInput)
        {
            this.movementInput = movementInput;
        }

        public void Jump()
        {
            context.Jumping = true;
        }

        public void SetSprinting(bool sprinting)
        {
            context.Sprinting = sprinting;
        }

        public void SetWalking(bool walking)
        {
            context.Walking = walking;
        }

        private void SwitchMode(MovementMode mode)
        {
            currentMovementMode = movementModes[mode];

            context = currentMovementMode.CreateContext();
        }
    }
}