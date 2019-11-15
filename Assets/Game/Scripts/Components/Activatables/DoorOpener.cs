using System.Collections;
using System.Collections.Generic;
using Game.Interactions;
using UnityEngine;

namespace Game.Components.Activatables
{
    public class DoorOpener : Activatable, IInteractable
    {
        Vector3 defaultDoorPosition;
        public float smooth = 2f;
        public float doorOpenDistance = 2f;
        public float doorOpenAngle = 90f;
        public float doorCloseAngle = 0f;
        public bool isOpen = false;

        public void Notify(bool notifiedState)
        {
            if (notifiedState)
            {
                Debug.Log($"Press 'F' to open {gameObject.name}");
            }
        }
    
        public void Interact()
        {
            Activate(isOpen = !isOpen);
        }

        public override void Activate(bool state)
        {
            isOpen = state;
        }

        private void OpenStandardDoor()
        {
            if (isOpen)
            {
                Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0); // // Calculate how much is rotation to ending position
                transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
            }
            else
            {
                Quaternion targetRotation2 = Quaternion.Euler(0, doorCloseAngle, 0); // Calculate how much is rotation to starting position
                transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
            }
        }

        private void OpenSlidingDoor()
        {
            if (isOpen)
            {
                float targetEndingPositionZ = Mathf.Lerp(transform.localPosition.z, defaultDoorPosition.z + doorOpenDistance, Time.deltaTime * smooth); // Calculate how much should door move
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, targetEndingPositionZ);
            }
            else
            {
                float targetStartingPositionZ = Mathf.Lerp(transform.localPosition.z, defaultDoorPosition.z, Time.deltaTime * smooth); // Calculate how much should door move back
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, targetStartingPositionZ);
            }
        }
        void Start()
        {
            defaultDoorPosition = transform.localPosition;
        }
        void Update()
        {
            if (gameObject.tag == "SlidingDoor") // Opens GameObjects with tag Sliding Door
            {
                OpenSlidingDoor();
            }
            else // Opens GameObjects with other tags (Basic Door)
            {
                OpenStandardDoor();
            }
        }
    }
}