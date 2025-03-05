using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject patrolBoss;
    public GameObject settingObject;

    private void Start()
    {
        animator = GetComponent<Animator>();

        originalCameraDeep = cameraTransform.localPosition.z;
        originalCameraHeight = cameraTransform.localPosition.y;
        originalCharacterHeight = controller.height;
        originalCharacterCenterY = controller.center.y;
    }

    public void OnOffPatrolBoss()
    { 
        patrolBoss.SetActive(!patrolBoss.activeSelf);
    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
        UpdateAnimations();
        HandleCrawl();
        CharacterHotKey();

        if (Input.GetKeyDown(KeyCode.M))
        { 
            OnOffPatrolBoss();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            settingObject.SetActive(!settingObject.activeSelf);
        }
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
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, originalCameraHeight, originalCameraDeep);
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

    public void FaintRightNow()
    {
        animator.SetBool("isFaint", true);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("faint_zone"))
        { 
            FaintRightNow();
            StartThirdPersonView();
            StartCoroutine(ChangeScene());
        }
    }

    public void StartFaintToMoveScene2()
    {
        FaintRightNow();
        StartThirdPersonView();
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(5f);
        //ShuraLoading.sceneName = "IntroGame";
        SceneManager.LoadScene("IntroGame");
    }

    public void StartThirdPersonView()
    {
        StartCoroutine(TransitionToThirdPerson());
    }

    private IEnumerator TransitionToThirdPerson()
    {
        float duration = 1.5f; // Thời gian chuyển đổi
        float elapsed = 0f;
        Vector3 startPosition = cameraTransform.localPosition;
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, -1f); // Kéo camera ra sau

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            cameraTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        yield return new WaitForSeconds(5f); // Đợi 3 giây để người chơi thấy cảnh ngã

        // Trả camera về góc nhìn thứ nhất
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            cameraTransform.localPosition = Vector3.Lerp(targetPosition, startPosition, t);
            yield return null;
        }
    }
}
