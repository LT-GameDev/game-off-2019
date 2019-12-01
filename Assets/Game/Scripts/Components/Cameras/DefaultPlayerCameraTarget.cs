#pragma warning disable 649

using System;
using Game.Components;
using UnityEngine;

namespace Game.Cameras
{
    public class DefaultPlayerCameraTarget : MonoBehaviour
    {
        [SerializeField] private Transform follow;
        [SerializeField] private Transform look;

        private PlayerCamera playerCamera;
        private PlayerController playerController;

        private void OnEnable()
        {
            PlayerCamera.SetTarget(follow, look);
        }

        private void LateUpdate()
        {
            transform.position = PlayerController.transform.position;
        }

        private PlayerCamera PlayerCamera
        {
            get
            {
                if (!playerCamera)
                    playerCamera = FindObjectOfType<PlayerCamera>();

                return playerCamera;
            }
        }

        private PlayerController PlayerController
        {
            get
            {
                if (!playerController)
                    playerController = FindObjectOfType<PlayerController>();

                return playerController;
            }
        }
    }
}