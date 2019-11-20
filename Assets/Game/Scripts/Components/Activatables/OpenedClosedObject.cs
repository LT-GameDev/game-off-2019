#define EDITOR_TEST_INPUT     // Comment out when using for production

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components.Activatables
{
    public class OpenedClosedObject : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform; // The transform to animate          
        [SerializeField] private Transform opened;          // A transform set from editor that defines the position and orientation of the object when opened
        [SerializeField] private Transform closed;          // A transform set from editor that defines the position and orientation of the object when closed
        [SerializeField] private float moveSpeed;           // Movement speed of the object when opening/closing
        [SerializeField] private float rotateSpeed;         // Rotation speed of the object when opening/closing
        
        private bool state;
        
        #if EDITOR_TEST_INPUT
        // To test its functionality
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetState(!state);
            }
        }
        #endif
        
        public void SetState(bool open)
        {
            // Make sure object is set as opened when closed, or as closed when opened
            if (state == open)
                return;
            
            state = open;

            if (state)
            {
                StopAllCoroutines();    // Cancel currently active action if there is one
                StartCoroutine(UpdateObjectState(opened));    // Start opening action
            }
            else
            {
                StopAllCoroutines();    // Cancel currently active action if there is one
                StartCoroutine(UpdateObjectState(closed));    // Start closing action
            }
        }

        private IEnumerator UpdateObjectState(Transform target)
        {
            var distanceThreshold = Time.deltaTime * moveSpeed * 1.2f;
            var angleThreshold    = Time.deltaTime * rotateSpeed * 1.2f;

            while (true)
            {
                var distance = Vector3.Distance(targetTransform.position, target.position);
                var angle    = Quaternion.Angle(targetTransform.rotation, target.rotation);
                
                var distanceThresholdReached = distance < distanceThreshold;
                var angleThresholdReached    = angle < angleThreshold;

                // Check if thresholds are reached
                if (distanceThresholdReached && angleThresholdReached)
                {
                    targetTransform.position = target.position;    // Set to target position
                    targetTransform.rotation = target.rotation;    // Set to target rotation
                    
                    yield break;    // Halt the coroutine
                }
                
                // Rotate and translate with interpolation
                targetTransform.position = Vector3.Slerp(targetTransform.position, target.position, Time.deltaTime * moveSpeed);
                targetTransform.rotation = Quaternion.Slerp(targetTransform.rotation, target.rotation, Time.deltaTime * rotateSpeed);
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
