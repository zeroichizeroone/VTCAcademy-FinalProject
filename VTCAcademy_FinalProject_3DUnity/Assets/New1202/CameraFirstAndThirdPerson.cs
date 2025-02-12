using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFirstAndThirdPersonal : MonoBehaviour
{
    // Public
    public Transform player;

    public Vector3 firstPersonOffset = new Vector3(0, 1.8f, 0);
    public Transform thirdPersonOffset;

    public float rotationSpeed = 2f;

    // Private
    private bool isFirstPerson = true;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        { 
            isFirstPerson = !isFirstPerson;
            if (isFirstPerson)
                SwitchToFirstPerson();
            else
                SwitchToThirdPerson();

        }
    }

    private void LateUpdate()
    {
        if (isFirstPerson)
        {
            cameraTransform.position = player.position + firstPersonOffset;
            cameraTransform.rotation = player.rotation;
        }
        else
        {
            Vector3 desiredPosition = thirdPersonOffset.position;
            cameraTransform.position = desiredPosition;
            cameraTransform.LookAt(player.position + Vector3.up * 1.5f);
        }
    }

    public void SwitchToFirstPerson()
    {
        cameraTransform.position = player.position + firstPersonOffset;
        cameraTransform.rotation = player.rotation;
    }

    public void SwitchToThirdPerson() 
    {
        cameraTransform.position = thirdPersonOffset.position;
        cameraTransform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
