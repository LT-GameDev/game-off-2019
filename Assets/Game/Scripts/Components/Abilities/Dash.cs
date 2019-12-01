#pragma warning disable 649

using System;
using UnityEngine;

namespace Game.Components.Abilities
{
    public class Dash : MonoBehaviour
    {
        [SerializeField] private Rigidbody characterBody;
        [SerializeField] private CapsuleCollider characterCollider;
        [SerializeField] private LayerMask dashBlockingLayers;
        [SerializeField] private float maxDashRange;
        [SerializeField] private float dashSpeed;

        private Coroutine dashRoutine;
        private Action onInterupted;
        private Action onComplete;
        private Vector3 direction;
        private Vector3 cahcedVelocity;
        private float distance;

        private void Awake()
        {
            enabled = false;
        }

        private void FixedUpdate()
        {
            var velocity      = direction * dashSpeed;
            var deltaPosition = velocity * Time.fixedDeltaTime;

            if (deltaPosition.magnitude > distance)
            {
                characterBody.position += deltaPosition;
                characterBody.velocity = cahcedVelocity;
                
                enabled = false;
                
                onComplete?.Invoke();
                
                return;
            }
            
            characterBody.velocity = velocity;

            distance -= deltaPosition.magnitude;
        }

        public void Use(Vector3 dashDirection, Action onComplete, Action onInterupted)
        {
            if (enabled)
                return;
            
            cahcedVelocity = characterBody.velocity;
            direction      = Vector3.Scale(new Vector3(1, 0, 1), dashDirection);

            characterBody.rotation = Quaternion.LookRotation(direction, characterBody.transform.up);
            
            distance  = GetDistance();
            enabled   = true;
            
            this.onComplete = onComplete;
        }

        public void Interupt()
        {
            characterBody.velocity = Vector3.zero;
                
            enabled = false;
                
            onInterupted?.Invoke();
        }

        private float GetDistance()
        {
            var position = characterBody.position + characterBody.rotation * characterCollider.center;

            if (Physics.SphereCast(position, characterCollider.radius, direction, out var hit, maxDashRange, dashBlockingLayers.value))
            {
                return Mathf.Min(hit.distance, maxDashRange);
            }

            return maxDashRange;
        }
    }
}