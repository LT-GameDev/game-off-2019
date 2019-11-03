#pragma warning disable 649

using UnityEngine;

namespace Game.Cameras
{
    public class DefaultPlayerCameraTarget : MonoBehaviour
    {
        [SerializeField] private Transform follow;
        [SerializeField] private Transform look;

        private PlayerCamera playerCamera;

        private void OnEnable()
        {
            PlayerCamera.SetTarget(follow, look);
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
    }
}