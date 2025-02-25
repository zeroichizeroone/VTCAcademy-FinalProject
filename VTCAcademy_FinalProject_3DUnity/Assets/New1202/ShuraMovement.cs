using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuraMovement : MonoBehaviour
{
    [Header("Character Attribute")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crawlSpeed = 1.5f;
    public float rotationSpeed = 200f;
    public float lookSpeed = 2f;
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Crawl")]
    public float crawlCameraDeep = 0.6f;
    public float crawlCameraHeight = 0.5f;
    public float crawlCharacterHeight = 0.6f;
    public float crawlCharacterCenterY = 0.3f;

    [Header("Ladder Climbing")]
    public float stairClimbSpeed = 2f;
    public float stairDetectionDistance = 1f;
    public LayerMask ladderLayer;

    private float originalCameraDeep;
    private float originalCameraHeight;
    private float originalCharacterHeight; 
    private float originalCharacterCenterY;

    private bool isCrawling = false;
    private bool isClimbingLadder = false;
    private Vector3 ladderDirection;

    private float verticalRotation = 0f;
    private Vector3 velocity;

    // Animation
    private Animator animator;
    private float currentSpeed;

    private void Start()
    {
        animator = GetComponent<Animator>();

        originalCameraDeep = cameraTransform.localPosition.z;
        originalCameraHeight = cameraTransform.localPosition.y;
        originalCharacterHeight = controller.height;
        originalCharacterCenterY = controller.center.y;
    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
        UpdateAnimations();
        HandleCrawl();
        CharacterHotKey();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        currentSpeed = GetCurrentSpeed();
        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y -= 9.81f * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private float GetCurrentSpeed()
    {
        if (isCrawling)
            return crawlSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            return runSpeed;
        return walkSpeed;
    }

    private void UpdateAnimations()
    {
        animator.SetFloat("Speed", currentSpeed);

        bool isWalking = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
        animator.SetBool("IsWalking", isWalking);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrawling;
        animator.SetBool("IsRunning", isRunning);

        animator.SetBool("IsCrawling", isCrawling);
        animator.SetBool("IsClimbing", isClimbingLadder);
    }

    private void HandleCrawl()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrawl();
        }
    }

    private void ToggleCrawl()
    {
        isCrawling = !isCrawling;

        if (isCrawling)
        {
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, crawlCameraHeight, crawlCameraDeep);
            controller.height = crawlCharacterHeight;
            controller.center = new Vector3(controller.center.x, crawlCharacterCenterY, controller.center.z);
        }
        else
        {
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, originalCameraHeight,originalCameraDeep);
            controller.height = originalCharacterHeight;
            controller.center = new Vector3(controller.center.x, originalCharacterCenterY, controller.center.z);
        }
    }

    public void CharacterHotKey()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryManager.Instance.ActiveInventory();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Hit: " + hit.gameObject.name );
        if (hit.gameObject.name == "DUST")
        {
            hit.gameObject.name ="DUST123";
        }
    }
}
