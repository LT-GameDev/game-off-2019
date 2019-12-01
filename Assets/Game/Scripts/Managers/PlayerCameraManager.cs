#pragma warning disable 649

using System;
using System.Collections;
using Cinemachine;
using Game.Components.Movement;
using UnityEngine;

namespace Game.Managers
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        [SerializeField] private NoiseConfig idleNoise;
        [SerializeField] private NoiseConfig walkingNoise;
        [SerializeField] private NoiseConfig runningNoise;
        [SerializeField] private NoiseConfig sprintingNoise;
        
        private PlayerMovementController movementController;
        private CinemachineBasicMultiChannelPerlin topRigNoise;
        private CinemachineBasicMultiChannelPerlin midRigNoise;
        private CinemachineBasicMultiChannelPerlin botRigNoise;
        
        private void Awake()
        {
            topRigNoise = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            midRigNoise = freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            botRigNoise = freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            movementController = FindObjectOfType<PlayerMovementController>();
        }

        private void OnEnable()
        {
            if (!movementController)
                return;

            StartCoroutine(EnableAfterFixedUpdate());

            
            IEnumerator EnableAfterFixedUpdate()
            {
                yield return new WaitForFixedUpdate();
            
                SetNoiseByState();

                movementController.Sprinting += OnStartSprinting;
                movementController.Walking   += OnStartWalking;
                movementController.Running   += OnStartRunning;
                movementController.Idle      += OnBecomeIdle;
            }
        }

        private void OnDisable()
        {
            if (!movementController)
                return;
            
            movementController.Idle      += OnBecomeIdle;
            movementController.Walking   += OnStartWalking;
            movementController.Running   += OnStartRunning;
            movementController.Sprinting += OnStartSprinting;
        }

        private void SetNoiseByState()
        {
            switch (movementController.State)
            {
                case CharacterMovementState.Idle:
                    SetNoise(idleNoise);
                    break;
                
                case CharacterMovementState.Walking:
                    SetNoise(walkingNoise);
                    break;
                
                case CharacterMovementState.Running:
                    SetNoise(runningNoise);
                    break;
                
                case CharacterMovementState.Sprinting:
                    SetNoise(sprintingNoise);
                    break;
            }
        }

        private void SetNoise(NoiseConfig config)
        {
            topRigNoise.m_AmplitudeGain = config.amplitudeGain;
            topRigNoise.m_FrequencyGain = config.frequencyGain;
            
            midRigNoise.m_AmplitudeGain = config.amplitudeGain;
            midRigNoise.m_FrequencyGain = config.frequencyGain;
            
            botRigNoise.m_AmplitudeGain = config.amplitudeGain;
            botRigNoise.m_FrequencyGain = config.frequencyGain;
        }

        private void OnBecomeIdle()
        {
            SetNoise(idleNoise);
        }

        private void OnStartWalking()
        {
            SetNoise(walkingNoise);
        }

        private void OnStartRunning()
        {
            SetNoise(runningNoise);
        }

        private void OnStartSprinting()
        {
            SetNoise(sprintingNoise);
        }


        [Serializable]
        private struct NoiseConfig
        {
            public float amplitudeGain;
            public float frequencyGain;
        }
    }
}