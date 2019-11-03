#pragma warning disable 649

using Game.Components.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController movementController;

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
    }
}