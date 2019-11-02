using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO add raycasting or other method to verify if player is looking at the object

public class DoorOpener : MonoBehaviour
{
    public float smooth = 2f;
    public float doorOpenAngle = 90f;
    public float doorCloseAngle = 0f;
    public bool isOpen = false;
    public bool inTrigger = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ChangeDoorState()
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
        if (Input.GetKeyDown(KeyCode.F) && inTrigger)
        {
            isOpen = !isOpen;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }
}
