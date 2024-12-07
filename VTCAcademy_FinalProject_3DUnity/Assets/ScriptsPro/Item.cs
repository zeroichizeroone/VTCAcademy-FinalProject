using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{ 
    door,
    drawer,
    collectionItem,
    examineItem,
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public string itemName;
    public string itemDescription;
    public string itemMessage;
    public bool isInteracted;

    // Attributes for examine item
    private bool isExamineMode;
    private Vector3 oldPositon;
    private Quaternion oldRotation;

    private void Update()
    {
        // Rotate object if player in Examine Mode
        if (isExamineMode)
        {
            RotateExamineObject();

            // Complete examine
            if (Input.GetKeyDown(KeyCode.P))
            {
                ExamineItem();
            }
        }
    }

    public void ActiveInteraction()
    {
        Debug.Log("Activeee");
        switch (itemType)
        {
            case ItemType.door:
                StartCoroutine(DoorInteraction(125));
                break;

            case ItemType.drawer:
                StartCoroutine(DrawerInteraction(0.6f));
                break;

            case ItemType.collectionItem:
                ExamineItem();
                ShowDescription();
                break;

            case ItemType.examineItem:
                ExamineItem();
                ShowDescription();
                break;
        }
    }

    private void ShowDescription()
    {
        if (itemDescription != "")
        {
            // Show description
            MessageManager.Instance.ShowTextMessage(itemDescription);
        }
    }

    private void ShowMessage()
    {
        if (itemMessage != "")
        {
            // Show message
        }

    }

    private IEnumerator DoorInteraction(int angleRotate)
    {
        float timeToRotate = 1.6f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);

        if (isInteracted == false)
        {
            endRotation = Quaternion.Euler(0, transform.eulerAngles.y + angleRotate, 0);
        }
        else
        {
            endRotation = Quaternion.Euler(0, transform.eulerAngles.y - angleRotate, 0);
        }

        float elapsedTime = 0;

        while (elapsedTime < timeToRotate)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, (elapsedTime / timeToRotate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        isInteracted = !isInteracted;
    }

    private IEnumerator DrawerInteraction(float pullDistance)
    {
        float timeToMove = 0.8f;
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition;

        Vector3 moveDirection = transform.InverseTransformDirection(-transform.forward);

        if (isInteracted == false)
        {
            endPosition = startPosition + moveDirection * pullDistance;
        }
        else
        {
            endPosition = startPosition - moveDirection * pullDistance;
        }

        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = endPosition;
        isInteracted = !isInteracted;
    }

    private void RotateExamineObject()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 rotation = new Vector3(-mouseY, mouseX, 0) * 200 * Time.deltaTime;

            transform.Rotate(rotation, Space.World);
        }
    }

    private void ExamineItem()
    {
        // Make item in center camera
        Camera mainCam = Camera.main;
        CameraManager cameraManager = mainCam.GetComponent<CameraManager>();

        if (isExamineMode == false)
        {
            // Enter examine item
            cameraManager.isProcessing = false;
            cameraManager.OnOffMouseOption();

            isExamineMode = true;

            oldPositon = transform.position;
            oldRotation = transform.rotation;

            transform.position = mainCam.transform.position + mainCam.transform.forward;
        }
        else
        {
            // Exit examine item
            cameraManager.isProcessing = true;
            cameraManager.OnOffMouseOption();

            isExamineMode = false;

            if (itemType == ItemType.collectionItem)
            {
                CollectItem();
            }
            else if (itemType == ItemType.examineItem)
            {
                transform.position = oldPositon;
                transform.rotation = oldRotation;
            }

            // Turn off message, description
            MessageManager.Instance.HideMessage();
        }
    }

    private void CollectItem()
    { 
        InventoryManager.Instance.AddItemToInventory(this);
        transform.gameObject.SetActive(false);
    }
}
