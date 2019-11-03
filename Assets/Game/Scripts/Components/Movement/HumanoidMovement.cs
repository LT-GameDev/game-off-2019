﻿#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components.Movement
{
    public class HumanoidMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody characterBody;
        [SerializeField] private float sprintMultiplier;
        [SerializeField] private float walkMultiplier;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;

        private Vector2 movementInput;
        private bool walking;
        private bool sprinting;

        private void OnDisable()
        {
            movementInput = Vector2.zero;
        }

        private void FixedUpdate()
        {
            var gravityComponent = Vector3.Scale(new Vector3(0, 1, 0), characterBody.velocity);
            var existingVelocity = Vector3.Scale(new Vector3(1, 0, 1), characterBody.velocity);

            if (movementInput.magnitude > 0.1f)
            {
                var movementDirection = (transform.forward * movementInput.y + transform.right * movementInput.x).normalized;

                // Combine existing and delta speed and convert to vector (velocity) in movement direction
                var deltaSpeed  = acceleration * Time.fixedDeltaTime;
                var newVelocity = movementDirection * (existingVelocity.magnitude + deltaSpeed);

                // change character's velocity
                characterBody.velocity = Vector3.ClampMagnitude(newVelocity, MovementSpeed) + gravityComponent;
            }
            else if (existingVelocity.magnitude > 0.1f)
            {
                // Combine existing and delta speed and convert to vector (velocity) in movement direction
                var deltaSpeed  = deceleration * Time.fixedDeltaTime;
                var newSpeed    = Mathf.Clamp(existingVelocity.magnitude - deltaSpeed, 0, float.MaxValue);
                var newVelocity = (existingVelocity.normalized * newSpeed) + gravityComponent;

                // change character's velocity
                characterBody.velocity = newVelocity;
            }
            else
            {
                characterBody.velocity = gravityComponent;
            }

            // Consume input
            movementInput = Vector3.zero;
        }

        public void Move(Vector2 input)
        {
            if (enabled)
            {
                movementInput = input;
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