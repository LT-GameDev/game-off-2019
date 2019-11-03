#pragma warning disable 649

using UnityEngine;
using System.Collections;
using Game.Utility;
using System.Collections.Generic;

namespace Game.Components.Movement
{
    public enum MovementMode
    {
        Humanoid
    }

    public interface IGroundable
    {
        void GroundCheck();
    }

    public struct MovementContext
    {
        public bool accelerate;
        public bool sprinting;
        public bool jumping;
        public bool walking;
        public Rigidbody characterBody;
        public GroundCheck groundCheckState;
        public Vector3 movementDirection;
    }

    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private DefaultMovement defaultMovement;

        private Transform myTransform;
        private CharacterMovement currentMovementMode;
        private MovementContext context;
        private Vector2 movementInput;

        private Dictionary<MovementMode, CharacterMovement> movementModes;

        private void Awake()
        {
            myTransform = transform;
            
            movementModes = new Dictionary<MovementMode, CharacterMovement> {
                { MovementMode.Humanoid, defaultMovement }
            };

            SwitchMode(default);
        }

        private void FixedUpdate()
        {
            context.accelerate = movementInput.magnitude > 0.1f;

            context.movementDirection = (myTransform.forward * movementInput.y + myTransform.right * movementInput.x).normalized;
            
            if (currentMovementMode is IGroundable groundable)
                groundable.GroundCheck();

            currentMovementMode?.ApplyPhysics(context, Time.fixedDeltaTime);
            
            // Consume state
            context.movementDirection = Vector3.zero;
            context.accelerate        = false;
            context.jumping           = false;
        }

        public void Move(Vector2 movementInput)
        {
            this.movementInput = movementInput;
        }

        public void Jump()
        {
            context.jumping = true;
        }

        public void StartSprinting()
        {
            context.sprinting = true;
        }

        public void StopSprinting()
        {
            context.sprinting = false;
        }

        public void ToggleWalking()
        {
            context.walking = !context.walking;
        }

        public void SwitchMode(MovementMode mode)
        {
            currentMovementMode = movementModes[mode];

            context = currentMovementMode.InitializeContext(context);
        }
    }
}