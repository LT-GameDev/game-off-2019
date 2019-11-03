#pragma warning disable 649

using Game.Components.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public enum MovementMode
    {
        Humanoid
    }

    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private HumanoidMovement humanoidMovement;

        private PlayerInput input;
        private MovementMode movementMode;
        private MovementBase movementMethod;

        private Vector2 playerInput;

        private void Awake()
        {
            input = new PlayerInput();

            input.Player.Move.performed += ctx => playerInput = ctx.ReadValue<Vector2>();
            input.Player.Move.canceled  += _ => playerInput = Vector2.zero;

            SetMovementState(false);
            SwitchMovementMode(default);
        }

        private void OnEnable()
        {
            input.Player.Enable();
        }

        private void Update()
        {
            movementMethod?.Move(playerInput);
        }

        private void OnDisable()
        {
            input.Player.Disable();
        }

        private void SwitchMovementMode(MovementMode mode)
        {
            switch (mode)
            {
                case MovementMode.Humanoid:
                    DisableActiveMovementMethod();

                    movementMethod = humanoidMovement;
                    movementMethod.enabled = true;
                    break;
            }

            void DisableActiveMovementMethod()
            {
                if (movementMethod)
                    movementMethod.enabled = false;
            }
        }

        private void SetMovementState(bool state)
        {
            humanoidMovement.enabled = false;
        }
    }
}