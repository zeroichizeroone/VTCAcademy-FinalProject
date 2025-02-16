using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

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

    [Header("Post-Processing Magic Eyes")]
    public GameObject post_processing;
    public bool isActiveDetectiveVision = false;

    public Image detectiveEnergy;
    public float maxEnergy = 5000f;
    public float energyPerSec = 100f;
    private float currentEnergy;
    private Coroutine energyCorotine;

    private void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyUI();
    }

    private void Update()
    {
        HandleCameraRotation();
        DetectObject();
        InitCameraHotKey();
    }

    public void InitCameraHotKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            OnOffMouseOption();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActiveDetectiveVision();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            RestoreEnergy(2000f);
        }
    }

    private void ActiveDetectiveVision()
    {
        if (currentEnergy <= 0)
        {
            MessageManager.Instance.ShowMessageWarning("[Cần thuốc an thần để vào \"Trạng thái thám tử\"]");
        }

        isActiveDetectiveVision = !isActiveDetectiveVision;

        post_processing.GetComponent<PostProcessVolume>().enabled = isActiveDetectiveVision;

        foreach (Item item in InventoryManager.Instance.globalItemList)
        {
            Outline outline = item.GetComponent<Outline>();
            if (outline == null)
            {
                outline = item.gameObject.AddComponent<Outline>();
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 4f;
                outline.OutlineMode = Outline.Mode.OutlineVisible;
            }
            outline.enabled = isActiveDetectiveVision;
        }

        if (isActiveDetectiveVision)
        {
            if (energyCorotine != null)
            { 
                StopCoroutine(energyCorotine);
            }
            energyCorotine = StartCoroutine(UseEnergy());
        }
    }

    private IEnumerator UseEnergy()
    {
        while (isActiveDetectiveVision)
        {
            if (currentEnergy > 0)
            {
                currentEnergy -= energyPerSec;
                if (currentEnergy < 0) currentEnergy = 0;
                UpdateEnergyUI();
            }

            if (currentEnergy <= 0)
            {
                isActiveDetectiveVision = false;
                post_processing.GetComponent<PostProcessVolume>().enabled = isActiveDetectiveVision;
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateEnergyUI()
    {
        detectiveEnergy.fillAmount = currentEnergy / maxEnergy;
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

    public void RestoreEnergy(float energy)
    {
        if (currentEnergy == maxEnergy)
        {
            MessageManager.Instance.ShowMessageWarning("[Độ tập trung đang đầy]");
            return;
        }
        
        currentEnergy += energy;
        
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }

        UpdateEnergyUI();
    }
}