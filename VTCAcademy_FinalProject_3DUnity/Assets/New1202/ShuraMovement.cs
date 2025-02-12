using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuraMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crawlSpeed = 1.5f;
    public float rotationSpeed = 200f;
    public float lookSpeed = 2f;
    public CharacterController controller;
    public Transform cameraTransform;

    private float currentSpeed;
    private float verticalRotation = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = crawlSpeed;
        }
        else
        {
            currentSpeed = walkSpeed; 
        }

        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(0, mouseX, 0);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
