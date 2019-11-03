#pragma warning disable 649

using UnityEngine;
using System.Collections;

namespace Game.Components.Movement
{
    public enum MovementMode
    {
        Humanoid
    }

    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private HumanoidMovement humanoidMovement;

        private bool walking;

        public void Move(Vector2 movementInput)
        {
            humanoidMovement.Move(movementInput);
        }

        public void Jump()
        {
            humanoidMovement.Jump();
        }

        public void StartSprinting()
        {
            humanoidMovement.SetSprinting(true);
        }

        public void StopSprinting()
        {
            humanoidMovement.SetSprinting(false);
        }

        public void ToggleWalking()
        {
            walking = !walking;

            humanoidMovement.SetWalking(walking);
        }

        public void SwitchMovementMode(MovementMode mode)
        {
            switch (mode)
            {
                case MovementMode.Humanoid:
                    SetAllModeStates(false);

                    humanoidMovement.enabled = true;
                    break;
            }
        }

        private void SetAllModeStates(bool state)
        {
            humanoidMovement.enabled = state;
        }
    }
}