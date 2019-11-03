using System.Collections;
using System.Collections.Generic;
using Game.Interactions;
using Game.Components.Activatables;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    public bool isPressed = false;
    public DoorOpener isOpened;

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
            isPressed = false;
        }
        else
        {
            isOpened.isOpen = !isPressed;
            isPressed = false;
        }
    }
}
