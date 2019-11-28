#define EDITOR_TEST_INPUT     // Comment out when using for production

using System.Collections;
using System.Collections.Generic;
using Game.Interactions;
using Game.Components.Activatables;
using UnityEngine;
public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform targetTransform; // The transform to move       
    [SerializeField] private Transform opened;          // A transform set from editor that defines the position and orientation of the object when opened
    [SerializeField] private Transform closed;         // A transform set from editor that defines the position and orientation of the object when closed
    [SerializeField] private float moveSpeed;           // Movement speed of the object when opening/closing

    public bool isPressed = false;
    public DoorOpener isOpened;

#if EDITOR_TEST_INPUT
        // To test its functionality
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
            Activate(!isPressed);
            }
        }
#endif
    public int GetInteractionType()
    {
        throw new System.NotImplementedException();
    }

    public void Notify(bool notifiedState)
    {
        if (notifiedState)
        {
            Debug.Log($"Press 'F' to press {gameObject.name}");
        }
    }

    public void Interact()
    {
        Activate(isPressed = !isPressed);
    }

    public void Activate(bool state)
    {

        isPressed = state;
        if (isPressed && !isOpened.isOpen) // if button is pressed and door is not closed
        {
            isOpened.isOpen = isPressed;
        }
        else
        {
            isOpened.isOpen = !isPressed;
        }

        if (isPressed)
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

        while (true)
        {
            var distance = Vector3.Distance(targetTransform.position, target.position);

            var distanceThresholdReached = distance < distanceThreshold;

            // Check if thresholds are reached
            if (distanceThresholdReached)
            {
                targetTransform.position = target.position;

                yield break;    // Halt the coroutine
            }

            //Translate with interpolation
            targetTransform.position = Vector3.Slerp(targetTransform.position, target.position, Time.deltaTime * moveSpeed);

            yield return new WaitForEndOfFrame();
        }

    }
}
