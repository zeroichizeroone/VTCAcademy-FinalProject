using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Charactermanager : MonoBehaviour
{
    [Header("Character Attribute")]
    public float characterWalkingSpeed = 5;
    public float characterSprintingSpeed = 8;
    public float characterJumpForce = 8;
    public Transform handTransform;
    public Camera playerCamera;
    public float pickupRange = 3f; 
    public Transform holdPosition; 
    private GameObject heldItem; // vat pham hien dang cam 


    [Header("Button in game")]
    public KeyCode keyCodeForSprint = KeyCode.LeftShift;

    private Rigidbody rb;
    private Camera mainCamera;
    private void Start() {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

    }
    private void Update() 
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
            {
                TryPickupItem();
            }
            else
            {
                DropItem();
            }
        }

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

    private void TryPickupItem()
    {
        // Raycast kiem tra item co dang trong tam nhat hay khong
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null && item.itemType == ItemType.none && item.isInteracting == true )
            {
                PickupItem(hit.collider.gameObject);
                
            }
        }

    }
    private Vector3 originalScale;


    private void PickupItem(GameObject item_obj)
    {
        heldItem = item_obj;

        // luu ty le dau vao cua item
        originalScale = heldItem.transform.localScale;

        // gan vat pham vao holdPosition
        heldItem.transform.SetParent(holdPosition);
        heldItem.transform.localPosition = Vector3.zero; // dat vat pham o giua
        heldItem.transform.localRotation = Quaternion.identity; // Reset goc quay
        heldItem.transform.localScale = Vector3.one; // dat ty le chuan de tranh bien dang

        // vo hieu hoa vat ly 
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb == null)
        {
            item_obj.AddComponent<Rigidbody>();
            item_obj.GetComponent<Rigidbody>().isKinematic = true;
            
            
        }
        else
        {
            rb.isKinematic = true; 
           
        }

        MeshCollider collider = heldItem.GetComponent<MeshCollider>();
        if (collider != null)
        {
            collider.convex = true;
            collider.isTrigger = true;
        }
    }

    private void DropItem()
    {
        heldItem.GetComponent<Item>().isInteracting = false;
        heldItem.transform.SetParent(null);


        // khoi phuc ty le ban dau
        heldItem.transform.localScale = originalScale;

        // dat item truoc mat nguoi choi
        Vector3 dropPosition = playerCamera.transform.position + playerCamera.transform.forward * 0.5f;
        heldItem.transform.position = dropPosition;

        // kich lai vat lys
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        MeshCollider collider = heldItem.GetComponent<MeshCollider>();
        if (collider != null)
        {
            
            collider.isTrigger = false;
        }

        StartCoroutine(DisablePhysicsAfterDelay(heldItem, rb, collider, 1.5f));

        heldItem = null;


    }
    private IEnumerator DisablePhysicsAfterDelay(GameObject item_obj, Rigidbody rb, MeshCollider collider, float delay)
    {
        
        yield return new WaitForSeconds(delay);

        // xoa Rigidbody và tat convex sau khoan tg
        if (rb != null)
        {
            Destroy(rb);
        }

        if (collider != null)
        {
            collider.convex = false;
        }
        item_obj.GetComponent<Item>().isInteracting = true ;
        
    }

}
