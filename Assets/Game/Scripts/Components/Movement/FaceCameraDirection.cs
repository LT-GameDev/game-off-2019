﻿using System;
using Game.Cameras;
using UnityEngine;

namespace Game.Components.Movement
{
    public class FaceCameraDirection : MonoBehaviour
    {
        [SerializeField] private Rigidbody characterBody;
        [SerializeField] private float lookSpeed;
        
        private PlayerCamera playerCamera;
        private Transform playerTransform;

        private void OnEnable()
        {
            playerCamera    = FindObjectOfType<PlayerCamera>();
            playerTransform = characterBody.transform;
        }

        private void FixedUpdate()
        {
            var groundPlane    = new Vector3(1, 0, 1);
            var groundVelocity = Vector3.Scale(groundPlane, characterBody.velocity);

            if (groundVelocity.magnitude < 0.1f)
                return;
            
            var forward      = (playerTransform.position - playerCamera.Position).normalized;
            var projected    = Vector3.Scale(groundPlane, forward);
            var lookRotation = Quaternion.LookRotation(projected, playerTransform.up);

            var lerpedRotation = Quaternion.Lerp(characterBody.rotation, lookRotation, lookSpeed * Time.fixedDeltaTime);

            characterBody.rotation = lerpedRotation;
        }
    }
}