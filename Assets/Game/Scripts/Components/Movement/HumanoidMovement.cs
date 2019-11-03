#pragma warning disable 649

using Game.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components.Movement
{
    public class HumanoidMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody characterBody;
        [SerializeField] private GroundCheck groundChecker;
        [SerializeField] private CapsuleCollider characterCollider;
        [SerializeField] private float extraFallingAcceleration;
        [SerializeField] private float sprintMultiplier;
        [SerializeField] private float walkMultiplier;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float airControl;
        [SerializeField] private float jumpForce;

        private Vector2 movementInput;
        private bool sprinting;
        private bool walking;
        private bool jump;

        private void OnDisable()
        {
            movementInput = Vector2.zero;
        }

        private void FixedUpdate()
        {
            groundChecker.Check(characterBody.position + transform.up * characterCollider.radius * 1.2f, characterCollider.height / 2, characterCollider.radius);

            HandleMovement();
        }

        public void Move(Vector2 input)
        {
            if (enabled)
            {
                movementInput = input;
            }
        }

        public void Jump()
        {
            if (enabled)
            {
                jump = true;
            }
        }
        
        public void SetWalking(bool state)
        {
            walking = state;
        }

        public void SetSprinting(bool state)
        {
            sprinting = state;
        }

        private void HandleMovement()
        {
            var gravityComponent = Vector3.Scale(new Vector3(0, 1, 0), characterBody.velocity);
            var existingVelocity = Vector3.Scale(new Vector3(1, 0, 1), characterBody.velocity);

            // If falling down, apply additional gravity so that falling is faster than rising up in jumps
            if (!groundChecker.Grounded && gravityComponent.y < -0.1f)
            {
                var deltaGravity = extraFallingAcceleration * Time.fixedDeltaTime;

                gravityComponent = gravityComponent.normalized * (gravityComponent.magnitude + deltaGravity);
            }

            if (movementInput.magnitude > 0.1f)
            {
                var movementDirection = (transform.forward * movementInput.y + transform.right * movementInput.x).normalized;

                // Combine existing and delta speed and convert to vector (velocity) in movement direction
                var deltaSpeed = acceleration * Time.fixedDeltaTime;

                // Apply air control penalty if character is in air
                var direction = !groundChecker.Grounded ? Vector3.Lerp(existingVelocity.normalized, movementDirection, airControl) : movementDirection;

                var newVelocity = direction * (existingVelocity.magnitude + deltaSpeed);

                // change character's velocity
                characterBody.velocity = Vector3.ClampMagnitude(newVelocity, MovementSpeed) + gravityComponent;
            }
            else if (existingVelocity.magnitude > 0.1f)
            {
                // Combine existing and delta speed and convert to vector (velocity) in movement direction
                var deltaSpeed  = deceleration * Time.fixedDeltaTime;

                // Apply air control penalty if character is in air
                if (!groundChecker.Grounded)
                    deltaSpeed *= airControl;

                var newSpeed    = Mathf.Clamp(existingVelocity.magnitude - deltaSpeed, 0, float.MaxValue);
                var newVelocity = (existingVelocity.normalized * newSpeed) + gravityComponent;

                // change character's velocity
                characterBody.velocity = newVelocity;
            }
            else
            {
                characterBody.velocity = gravityComponent;
            }

            if (jump && groundChecker.Grounded)
            {
                characterBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                jump = false;
            }

            // Consume input
            movementInput = Vector3.zero;
        }

        private float MovementSpeed
        {
            get
            {
                if (sprinting)
                    return movementSpeed * sprintMultiplier;

                if (walking)
                    return movementSpeed * walkMultiplier;

                return movementSpeed;
            }
        }
    }
}