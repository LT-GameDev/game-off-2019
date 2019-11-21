#pragma warning disable 649

using System;
using System.Collections;
using Game.Components.Movement;
using Game.Components.Abilities;
using Game.Components.Animations;
using Game.Enums;
using Game.Managers;
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

        private Coroutine staminaRecoveryRoutine;
        private PlayerResourceManager resourceManager;
        private GameManager gameManager;
        private PlayerInput input;
        private Vector2 playerInput;
        private bool inputBlocked;
        private float fallTime;

        private void Awake()
        {
            gameManager     = FindObjectOfType<GameManager>();
            resourceManager = gameManager.GetService<PlayerResourceManager>();
            
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
            
            resourceManager.StaminaConsumed += OnStaminaConsumed;
        }

        private void FixedUpdate()
        {
            if (!movementController.IsFalling)
            {
                fallTime = 0;
            }
            else
            {
                fallTime += Time.fixedDeltaTime;

                if (fallTime >= 1)
                {
                    FallToDeath();
                }
            }
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

            resourceManager.StaminaConsumed -= OnStaminaConsumed;
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

        private void FallToDeath()
        {
            enabled = false;
            
            // Play falling to death sound effect?
            // Switch to falling camera?
            // Additional logic?

            StartCoroutine(ContinueFromLastSave());
            
            
            IEnumerator ContinueFromLastSave()
            {
                yield return new WaitForSeconds(2f);
                gameManager.ContinueGame();
            }
        }

        private void OnStaminaConsumed()
        {
            if (staminaRecoveryRoutine != null)
                StopCoroutine(staminaRecoveryRoutine);

            staminaRecoveryRoutine = StartCoroutine(StaminaRecoveryRoutine());


            IEnumerator StaminaRecoveryRoutine()
            {
                yield return new WaitForSeconds(1f);

                while (true)
                {
                    if (!resourceManager.RecoverStamina(Time.fixedDeltaTime))
                        yield break;
                        
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}