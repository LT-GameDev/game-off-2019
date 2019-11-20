#pragma warning disable 649

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Game.Cameras;
using Game.Components.Movement.Interface;
using Game.Models.Movement;
using Game.Utility;

namespace Game.Components.Movement
{
    public enum MovementMode
    {
        Humanoid,
        WallMovement
    }

    public enum CharacterMovementState
    {
        Falling,
        Idle,
        Walking,
        Running,
        Sprinting,
        WallSprinting,
        Jumping
    }

    public class PlayerMovementController : MovementController
    {
        public event Action Falling;
        public event Action Idle;
        public event Action Walking;
        public event Action Running;
        public event Action Sprinting;
        
        [Header("Properties")]
        [SerializeField] private Transform childMeshRoot;
        [SerializeField] private Rigidbody characterBody;
        [SerializeField] private CapsuleCollider characterCollider;
        [SerializeField] private GroundCheck groundChecker;
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private LayerMask wallLayers;
        
        [Header("Movement Modes")]
        [SerializeField] private DefaultMovement defaultMovement;
        [SerializeField] private WallMovement wallMovement;

        private Transform myTransform;
        private PlayerMovementContext context;
        private Coroutine delayedJump;
        private IMovementLogic currentMovementLogic;
        private MovementMode currentMovementMode;

        private Dictionary<MovementMode, IMovementLogic> movementModes;

        private bool wasMoving;

        private void Awake()
        {
            myTransform = transform;

            playerCamera = FindObjectOfType<PlayerCamera>();
            
            context       = new PlayerMovementContext();
            movementModes = new Dictionary<MovementMode, IMovementLogic> {
                { MovementMode.Humanoid, defaultMovement },
                { MovementMode.WallMovement, wallMovement }
            };

            context.meshRoot = childMeshRoot;
            context.body     = characterBody;
            
            defaultMovement.Initialize(context);
            wallMovement.Initialize(context);

            State = CharacterMovementState.Idle;

            SwitchMode(default);
        }

        private void FixedUpdate()
        {
            GlobalGroundCheck();
            ManageTransitions();
            ManageStates();
            
            currentMovementLogic?.Run(Time.fixedDeltaTime);

            context.direction = Vector3.zero;
            context.jump      = false;
        }

        public void Move(Vector2 movementInput)
        {
            context.direction = GetForward() * movementInput.y + GetRight() * movementInput.x;
        }

        public void Jump()
        {
            if (characterBody.velocity.magnitude < 0.1f && delayedJump == null)
            {
                delayedJump = StartCoroutine(DelayedJump());
                
                IEnumerator DelayedJump()
                {
                    yield return new WaitForSeconds(0.5f);
                    context.jump = true;
                    delayedJump = null;
                }   
            }
            else
            {
                context.jump = true;
            }
        }

        public void SetSprinting(bool sprinting)
        {
            context.sprint = sprinting;
        }

        public void SetWalking(bool walking)
        {
            context.walk = walking;
        }

        private void DefaultMovementCallback()
        {
            defaultMovement.Run(Time.fixedDeltaTime);
        }

        private void WallMovementCallback()
        {
            wallMovement.Run(Time.fixedDeltaTime);
        }

        private void ManageTransitions()
        {
            if (ShouldEnterWallMovement())
                SwitchMode(MovementMode.WallMovement);
            else if (ShouldLeaveWallMovement())
                SwitchMode(MovementMode.Humanoid);
        }

        private void GlobalGroundCheck()
        {
            var position   = characterBody.position + characterBody.rotation * characterCollider.center + Vector3.up * 0.01f;
            var halfHeight = characterCollider.height / 2;
            
            groundChecker.Check(position, halfHeight * 0.9f, characterCollider.radius * 0.5f);

            context.grounded = groundChecker.Grounded;
        }

        private void SwitchMode(MovementMode mode)
        {
            currentMovementLogic = movementModes[mode];
            currentMovementMode  = mode;
            
            currentMovementLogic.WarmUp();
            
            this.Log().Debug($"Switch to {mode} movement!", nameof(SwitchMode));
        }

        private void ManageStates()
        {
            if (characterBody.velocity.magnitude > 0.1f)
            {
                // Set Loco State if character is moving
                SetLocoState();
            }
            else
            {
                if (groundChecker.Distance <= 10f)
                {
                    State = CharacterMovementState.Idle;
                    Idle?.Invoke();
                }
                else
                {
                    State = CharacterMovementState.Falling;
                    Falling?.Invoke();
                }
            }


            void SetLocoState()
            {
                if (context.sprint && State != CharacterMovementState.Sprinting)     // Change state to sprinting
                {
                    State = CharacterMovementState.Sprinting;
                    Sprinting?.Invoke();
                }
                else if (context.walk && State != CharacterMovementState.Walking && State != CharacterMovementState.Sprinting)    // Change state to walking
                {
                    State = CharacterMovementState.Walking;
                    Walking?.Invoke();
                }
                else if (State != CharacterMovementState.Running && State != CharacterMovementState.Walking && State != CharacterMovementState.Sprinting)    // Change state to running
                {
                    State = CharacterMovementState.Running;
                    Running?.Invoke();
                }
            }
        }

        public override Vector3 GetForward()
        {
            var forward          = characterBody.position - playerCamera.Position;
            var projectedForward = Vector3.Scale(GameWorld.GetGroundPlane(), forward);

            return projectedForward.normalized;
        }
        
        public override Vector3 GetRight()
        {
            var right = Vector3.Cross(Vector3.up, GetForward());

            return right;
        }

        private bool ShouldEnterWallMovement()
        {
            // If player is grounded or is already in wall movement mode, cannot switch to wall movement
            if (context.grounded || currentMovementMode == MovementMode.WallMovement)
                return false;

            var right  = GetRight();
            var length = characterCollider.radius + 0.02f;
            
            var position = characterBody.position;

            var rightResult = Raycast(right);
            var leftResult  = Raycast(-right);

            GizmoDrawer.Draw(new LineGizmo(position, position + right * length, rightResult.success ? Color.green : Color.red));
            GizmoDrawer.Draw(new LineGizmo(position, position - right * length, leftResult.success ? Color.green : Color.red));

            // Cannot snap to wall if cornered or in a tight place
            if (rightResult.success && leftResult.success)
                return false;

            if (rightResult.success)
            {
                context.wallNormal   = rightResult.hit.normal;
                context.wallDistance = rightResult.hit.distance;
                
                return true;
            }
            
            if (leftResult.success)
            {
                context.wallNormal   = leftResult.hit.normal;
                context.wallDistance = leftResult.hit.distance;
                
                return true;
            }

            return false;


            (bool success, RaycastHit hit) Raycast(Vector3 direction)
            {
                if (Physics.Raycast(position, direction, out var hit, length, wallLayers.value))
                {
                    return (true, hit);
                }
                
                return (false, default);
            }
        }
        
        public CharacterMovementState State { get; private set; }

        private bool ShouldLeaveWallMovement()
        {
            return context.grounded && currentMovementMode == MovementMode.WallMovement;
        }

        public bool Grounded => (currentMovementMode == MovementMode.Humanoid && context.grounded) || 
                                (currentMovementMode == MovementMode.WallMovement && !context.falling && !context.jumpOff);

        public bool IsFalling => characterBody.velocity.y < -0.1f && !groundChecker.Grounded &&
                                 groundChecker.Distance >= groundChecker.MaxDistance;
    }
}