using UnityEngine;

public class ShuraCamera : MonoBehaviour
{
    [Header("Camera")]
    public Transform player;
    public Transform cameraTransform;

    public bool isProcessing = true;
    public float rotationSpeed = 2f;


    [Header("Detect")]
    public GameObject detectionPoint;
    public float detectionDistance = 5f;
    public LayerMask detectableLayer;
    private GameObject currentObject;

    private void Update()
    {
        HandleCameraRotation();
        DetectObject();
    }

    private void HandleCameraRotation()
    {
        if (isProcessing)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            player.Rotate(Vector3.up * mouseX);

            float angle = cameraTransform.localEulerAngles.x - mouseY;
            angle = Mathf.Clamp(angle, -90f, 360f);
            cameraTransform.localEulerAngles = new Vector3(angle, 0, 0);
        }
    }

    public void OnOffMouseOption()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void DetectObject()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionDistance, detectableLayer))
        {
            currentObject = hit.collider.gameObject;

            detectionPoint.SetActive(true);
            ButtonSuggestionManager.Instance.ShowButtonSuggestion("E");

            if (Input.GetKeyDown(KeyCode.E))
            {
                Item itemDetected = currentObject.GetComponent<Item>();

                if (itemDetected.isExamineMode == true)
                {
                    itemDetected.ExamineItem();
                    return;
                }

                itemDetected.ActiveInteraction();
            }
        }
        else
        {
            detectionPoint.SetActive(false);
            ButtonSuggestionManager.Instance.HideButtonSuggestion("E");
        }

        Debug.DrawRay(ray.origin, ray.direction * detectionDistance, Color.red);
    }
}