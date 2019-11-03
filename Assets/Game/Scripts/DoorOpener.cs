﻿using System.Collections;
using System.Collections.Generic;
using Game.Interactions;
using UnityEngine;

public class DoorOpener : MonoBehaviour, IInteractable
{
    public float smooth = 2f;
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
        isOpen = !isOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, doorOpenAngle);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation2 = Quaternion.Euler(0, 0, doorCloseAngle);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
        }
    }
}
