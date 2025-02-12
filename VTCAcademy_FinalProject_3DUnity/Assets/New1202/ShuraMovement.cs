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

    private float verticalRotation = 0f;

    private void Update()
    {
        HandleMovement();
        CharacterHotKey();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentSpeed = GetCurrentSpeed();
        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private float GetCurrentSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            return runSpeed;
        if (Input.GetKey(KeyCode.LeftControl))
            return crawlSpeed;
        return walkSpeed;
    }

    public void CharacterHotKey()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryManager.Instance.ActiveInventory();
        }
    }
}
