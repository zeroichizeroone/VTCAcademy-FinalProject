using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;


public class Charactermanager : MonoBehaviour
{
    [Header("Character Attribute")]
    public float characterWalkingSpeed = 5;
    public float characterSprintingSpeed = 8;
    public float characterJumpForce = 8;
    public Transform handTransform;
    private GameObject currentItem;
    private ItemInGame itemScript;

    [Header("Button in game")]
    public KeyCode keyCodeForSprint = KeyCode.LeftShift;

    private Rigidbody rb;
    private Camera mainCamera;
    private void Start() {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

    }
    private void Update() {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentItem != null)
            {
                DropItem();
            }
            else
            {
                PickUpItem();
            }
        }
    }
    //void DropItem()
    //{
    //    if (currentItem != null)
    //    {
    //        itemScript.Drop();
    //        currentItem.transform.SetParent(null);
    //        currentItem = null;
    //    }
    //}
    //void PickUpItem()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))  // Kiểm tra phạm vi 3m
    //    {
    //        Item item = hit.collider.GetComponent<Item>();
    //        if (item != null && !item.isHeld)
    //        {
    //            currentItem = hit.collider.gameObject;
    //            itemScript = item;
    //            itemScript.PickUp();

    //            
    //            currentItem.transform.SetParent(handTransform);
    //            currentItem.transform.localPosition = Vector3.zero;  // Đặt đồ vật vào vị trí chính xác trong tay
    //            currentItem.transform.localRotation = Quaternion.identity;
    //        }
    //    }
    //}

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


    private void PickUpItem()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            ItemInGame item = hit.collider.GetComponent<ItemInGame>();
            if (item != null && !item.isHeld)
            {
                currentItem = hit.collider.gameObject;
                itemScript = item;
                itemScript.PickUP();

                // Gắn đồ vật vào tay nhân vật
                currentItem.transform.SetParent(handTransform);
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void DropItem()
    {
        if (currentItem != null)
        {
            itemScript.Drop();
            currentItem.transform.SetParent(null);
            currentItem = null;
        }
    }


}
