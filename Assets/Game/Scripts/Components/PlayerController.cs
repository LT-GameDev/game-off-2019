#pragma warning disable 649

using Game.Components.Movement;
using Game.Components.Abilities;
using UnityEngine;

namespace Game.Components
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private FaceCameraDirection faceCameraDirection;
        [SerializeField] private Dash dash;

        private PlayerInput input;

        private Vector2 playerInput;

        private void Awake()
        {
            input = new PlayerInput();

            input.Player.Move.performed += ctx => playerInput = ctx.ReadValue<Vector2>();
            input.Player.Move.canceled  += _ => playerInput = Vector2.zero;

            input.Player.ToggleWalkRun.performed += _ => movementController.ToggleWalking();

            input.Player.Sprint.started  += _ => movementController.StartSprinting();
            input.Player.Sprint.canceled += _ => movementController.StopSprinting();

            input.Player.Jump.performed += _ => movementController.Jump();

            input.Player.Dash.performed += _ => Dash();
        }

        private void OnEnable()
        {
            input.Player.Enable();
        }

        private void Update()
        {
            movementController.Move(playerInput);
        }

        private void OnDisable()
        {
            input.Player.Disable();
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