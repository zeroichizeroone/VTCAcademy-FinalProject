using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("Attribute Camera")]
    public Transform target;
    public bool isProcessing = true;

    [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0f);
    private Quaternion rotation;

    // Attribute Mouse
    private float x;
    private float y;
    [SerializeField] private float xSpeed = 5f;
    [SerializeField] private float ySpeed = 4f;

    [SerializeField] private float xMinRotation = -360f;
    [SerializeField] private float xMaxRotation = 360f;
    [SerializeField] private float yMinRotation = -80f;
    [SerializeField] private float yMaxRotation = 80f;

    [Header("Item Interaction")]
    public GameObject detectObjectPoint;
    public float rayDistance = 5f;
    public LayerMask detectableLayer;
    private GameObject currentObject;

    [Header("Post-Processing Magic Eyes")]
    public GameObject post_processing;

    void Start()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;

        Vector3 angles = this.transform.eulerAngles;
        x = angles.x;
        y = angles.y;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOffMouseOption();
        }

        if (isProcessing)
        {
            CameraMove();
            rotation = Quaternion.Euler(-y, x, 0);

            Vector3 distanceVector = offset;
            Vector3 position = rotation * distanceVector + target.position;
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public void OnOffMouseOption()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void CameraMove()
    {
        x += Input.GetAxis("Mouse X") * xSpeed;
        y += Input.GetAxis("Mouse Y") * ySpeed;

        x = ClampAngle(x, xMinRotation, xMaxRotation);
        y = ClampAngle(y, yMinRotation, yMaxRotation);
    }

    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;

        return Mathf.Clamp(angle, min, max);
    }

    private void CheckObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, detectableLayer))
        {
            currentObject = hit.collider.gameObject;
            detectObjectPoint.SetActive(true);
            ButtonSuggestionManager.Instance.ShowButtonSuggestion("E");

            if (Input.GetKeyDown(KeyCode.E))
            {
                Item itemDetected = currentObject.GetComponent<Item>();

                // Check if player in examine mode
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
            detectObjectPoint.SetActive(false);
            ButtonSuggestionManager.Instance.HideButtonSuggestion("E");
        }
    }

    private void Update()
    {
        CheckObject();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Active - Detective Vision");
            ActiveDetectiveVision();
        }
    }

    private void ActiveDetectiveVision()
    {
        post_processing.GetComponent<PostProcessVolume>().enabled = !post_processing.GetComponent<PostProcessVolume>().isActiveAndEnabled;
    }
}
