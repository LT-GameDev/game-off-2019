#pragma warning disable 649

using Game.Components.Movement;
using Game.Components.Abilities;
using Game.Components.Animations;
using Game.Enums;
using UnityEngine;

namespace Game.Components
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerAnimationControl animationController;
        [SerializeField] private FaceCameraDirection faceCameraDirection;
        [SerializeField] private Interactor interactor;
        
        [Header("Abilities")]
        [SerializeField] private Dash dash;

        private PlayerInput input;
        private Vector2 playerInput;
        private bool inputBlocked;

        private void Awake()
        {
            input = new PlayerInput();

            input.Player.Move.performed += ctx => playerInput = ctx.ReadValue<Vector2>();
            input.Player.Move.canceled  += _ => playerInput = Vector2.zero;

            input.Player.ToggleWalkRun.performed += _ => ToggleWalking();

            input.Player.Sprint.started  += _ => SetSprinting(true);
            input.Player.Sprint.canceled += _ => SetSprinting(false);

            input.Player.Jump.performed += _ => Jump();

            input.Player.Interact.performed += _ => Interact();

            input.Player.Dash.performed += _ => Dash();
        }

        private void OnEnable()
        {
            input.Player.Enable();
            
            animationController.BecomeBusy   += OnBecomeBusy;
            animationController.NoLongerBusy += OnBecomeFree;
        }

        private void Update()
        {
            movementController.Move(playerInput);

            animationController.Context.Loco = playerInput.magnitude > 0.1f && movementController.Grounded;
        }

        private void OnDisable()
        {
            input.Player.Disable();
            
            animationController.NoLongerBusy -= OnBecomeFree;
            animationController.BecomeBusy   -= OnBecomeBusy;
        }

        private void ToggleWalking()
        {
            var walking = animationController.Context.Walking;
            
            movementController.SetWalking(animationController.Context.Walking = !walking);
        }

        private void SetSprinting(bool state)
        {
            movementController.SetSprinting(animationController.Context.Sprinting = state);
        }

        private void Jump()
        {
            if (inputBlocked)
                return;
                
            movementController.Jump();

            animationController.Context.Jump = true;
        }

        private void Dash()
        {
            if (inputBlocked)
                return;
            
            if (dash.enabled)
                return;
            
            movementController.enabled = false;
            dash.Use(faceCameraDirection.Direction, () => movementController.enabled = true, null);
        }

        private void Interact()
        {
            var interactionType = interactor.Interact();

            switch (interactionType)
            {
                case (int) PlayerInteractions.Pickup:
                    animationController.Context.Pickup = true;
                    break;
            }
        }

        private void OnBecomeBusy()
        {
            inputBlocked = true;
        }

        private void OnBecomeFree()
        {
            inputBlocked = false;
        }
    }
}