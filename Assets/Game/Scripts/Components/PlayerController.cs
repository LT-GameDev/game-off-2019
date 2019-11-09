#pragma warning disable 649

using Game.Components.Movement;
using Game.Components.Abilities;
using Game.Components.Animations;
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

        private void Awake()
        {
            input = new PlayerInput();

            input.Player.Move.performed += ctx => playerInput = ctx.ReadValue<Vector2>();
            input.Player.Move.canceled  += _ => playerInput = Vector2.zero;

            input.Player.ToggleWalkRun.performed += _ => ToggleWalking();

            input.Player.Sprint.started  += _ => SetSprinting(true);
            input.Player.Sprint.canceled += _ => SetSprinting(false);

            input.Player.Jump.performed += _ => movementController.Jump();

            input.Player.Interact.performed += _ => interactor.Interact();

            input.Player.Dash.performed += _ => Dash();
        }

        private void OnEnable()
        {
            input.Player.Enable();
        }

        private void Update()
        {
            movementController.Move(playerInput);

            animationController.Context.Loco = playerInput.magnitude > 0.1f && movementController.Grounded;
        }

        private void OnDisable()
        {
            input.Player.Disable();
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

        private void Dash()
        {
            if (dash.enabled)
                return;
            
            movementController.enabled = false;
            dash.Use(faceCameraDirection.Direction, () => movementController.enabled = true, null);
        }
    }
}