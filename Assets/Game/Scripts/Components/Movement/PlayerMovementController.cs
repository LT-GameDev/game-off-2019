#pragma warning disable 649

using System;
using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;
using Game.Cameras;
using Game.Utility;

namespace Game.Components.Movement
{
    public enum MovementMode
    {
        Humanoid,
        WallMovement
    }

    public class PlayerMovementController : MovementController
    {
        [Header("Properties")]
        [SerializeField] private Transform childMeshRoot;
        [SerializeField] private Rigidbody characterBody;
        [SerializeField] private CapsuleCollider characterCollider;
        [SerializeField] private GroundCheck groundChecker;
        [SerializeField] private PlayerCamera playerCamera;
        
        [Header("Movement Modes")]
        [SerializeField] private DefaultMovement defaultMovement;

        private Transform myTransform;
        private MovementMode currentMovementMode;
        private Action currentMovementUpdate;
        private DefaultMovementContext context;

        private Dictionary<MovementMode, Action> movementModes;

        private void Awake()
        {
            myTransform = transform;

            playerCamera = FindObjectOfType<PlayerCamera>();
            
            context       = new DefaultMovementContext();
            movementModes = new Dictionary<MovementMode, Action> {
                { MovementMode.Humanoid, DefaultMovementCallback }
            };

            context.meshRoot = childMeshRoot;
            context.body     = characterBody;
            
            defaultMovement.Initialize(context);

            SwitchMode(default);
        }

        private void FixedUpdate()
        {
            GlobalGroundCheck();
            
            currentMovementUpdate?.Invoke();
        }

        public void Move(Vector2 movementInput)
        {
            context.direction = GetForward() * movementInput.y + GetRight() * movementInput.x;
        }

        public void Jump()
        {
            context.jump = true;
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

        private void GlobalGroundCheck()
        {
            var position   = characterBody.position + characterBody.rotation * characterCollider.center;
            var halfHeight = characterCollider.height / 2;
            
            groundChecker.Check(position, halfHeight, characterCollider.radius);

            context.grounded = groundChecker.Grounded;
        }

        private void SwitchMode(MovementMode mode)
        {
            currentMovementUpdate = movementModes[mode];
            currentMovementMode   = mode;
            
            this.Log().Debug($"Switch to {mode} movement!", nameof(SwitchMode));
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

        public bool Grounded => context.grounded;
    }
}