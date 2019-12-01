#pragma warning disable 649

using Cinemachine;
using UnityEngine;

namespace Game.Cameras
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraBase cam;

        private void Update()
        {
            Validate();
        }
        
        public void SetTarget(Transform follow, Transform look)
        {
            cam.Follow = follow;
            cam.LookAt = look;
            
            Validate();
        }

        private void Validate()
        {
            var enabledAndHasTarget = enabled && cam.Follow && cam.LookAt;
            var disabledAndNoTarget = !enabled && (!cam.Follow || !cam.LookAt);

            if (enabledAndHasTarget || disabledAndNoTarget)
                return;

            var state = cam.Follow && cam.LookAt;

            cam.enabled = state;
            enabled     = state;
        }

        public Vector3 Position => cam.State.FinalPosition;
    }
}