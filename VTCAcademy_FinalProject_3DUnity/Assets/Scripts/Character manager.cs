using UnityEngine;

public class Charactermanager : MonoBehaviour
{
    [Header("Character Attribute")]
    public float characterWalkingSpeed = 5;
    public float characterSprintingSpeed = 8;
    public float characterJumpForce = 8;

    [Header("Button in game")]
    public KeyCode keyCodeForSprint = KeyCode.LeftShift;
    
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    private void Update() {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float ipHorizontal = Input.GetAxis("Horizontal");
        float ipVertical = Input.GetAxis("Vertical");
        
        float currentSpeed = characterWalkingSpeed;
        if(Input.GetKey(keyCodeForSprint))
        {
            currentSpeed = characterSprintingSpeed;
        }

        Vector3 movement = new Vector3(ipHorizontal, 0,ipVertical);
        movement.Normalize();
        
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y=0;
        movement = Quaternion.LookRotation(cameraForward) * movement;

        rb.MovePosition(transform.position + movement * currentSpeed * Time.fixedDeltaTime);
        HandleRotation(movement);
        
        if(Input.GetKeyDown(KeyCode.Space) && (Mathf.Abs(rb.velocity.y)<0.01f) == true)
        {
            rb.AddForce(new Vector3(0,1,0) * characterJumpForce, ForceMode.Impulse);
        }
    }

    private void HandleRotation (Vector3 playerMovementInput)
    {
        Vector3 lookDirection = playerMovementInput;
        lookDirection.y = 0;

        if(lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = rotation;
        }
    }

    
}
