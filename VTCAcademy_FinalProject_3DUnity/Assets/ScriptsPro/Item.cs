using System.Collections;
using UnityEngine;

public enum ItemType
{
    none,
    cupboard,
    door,
    drawer,
    collectionItem,
    examineItem,
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public Sprite itemImage;
    public string itemName;
    public string itemDescription;
    public string itemMessage;
    public bool isInteracting;

    private bool isOpening = false; // Kiểm soát trạng thái cửa
    private bool isDoorOpening = false;

    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    public bool isInteracted;

    public string[] dialogueLines;  // Câu thoại khi tương tác
    public AudioClip[] dialogueClips;  // Âm thanh tương ứng
    public AudioSource audioSource;  // Thêm AudioSource vào cửa

    // Attributes for examine item
    public bool isExamineMode;
    private Vector3 oldPositon;
    private Quaternion oldRotation;

    public bool isHighlightItem = false;

    // Biến mới: kích hoạt hiệu ứng kinh dị khi nhặt
    public bool triggerHorrorEffect = false;

    private void Update()
    {
        // Rotate object if player in Examine Mode
        if (isExamineMode)
        {
            RotateExamineObject();
        }
    }

    public void ActiveInteraction()
    {
        Debug.Log("Active Interaction - Item");
        DialogueManager.Instance.EnqueueDialogue(dialogueLines, dialogueClips, GameObject.FindWithTag("Player"));
        switch (itemType)
        {
            case ItemType.door:
                StartCoroutine(DoorInteraction(125));
                break;
            case ItemType.drawer:
                StartCoroutine(DrawerInteraction(0.6f));
                break;
            case ItemType.cupboard:
                StartCoroutine(CupBoardInteraction(75));
                break;
            case ItemType.examineItem:
                ExamineItem();
                break;
            case ItemType.collectionItem:
                ExamineItem();
                break;
        }
    }

    private IEnumerator CupBoardInteraction(int angleRotate)
    {
        if (isOpening) yield break; // Nếu cửa đang mở, không cho phép tương tác tiếp

        isOpening = true; // Đánh dấu cửa đang mở
        float timeToRotate = 1.6f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (!isInteracted)
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
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / timeToRotate);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRotation;
        isInteracted = !isInteracted;
        isOpening = false; // Cho phép tương tác lại sau khi cửa mở xong
    }

    private IEnumerator DoorInteraction(int angleRotate)
    {
        string doorTag = gameObject.tag;

        // Kiểm tra nếu cửa KHÔNG phải là "DOOR" thì mới yêu cầu chìa khóa
        if (doorTag != "door" && !InventoryManager.Instance.HasItem(doorTag))
        {
            MessageManager.Instance.ShowTextMessage("Cần chìa khóa phù hợp để mở cửa!");
            yield break; // Dừng nếu không có chìa khóa phù hợp
        }

        if (isDoorOpening) yield break;

        isDoorOpening = true;

        // Phát âm thanh mở cửa hoặc đóng cửa dựa trên trạng thái
        if (audioSource != null)
        {
            AudioClip clipToPlay = isInteracted ? doorCloseSound : doorOpenSound;
            if (clipToPlay != null) audioSource.PlayOneShot(clipToPlay);
        }

        float timeToRotate = 1.9f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, transform.eulerAngles.y + (isInteracted ? -angleRotate : angleRotate), 0);

        float elapsedTime = 0;
        while (elapsedTime < timeToRotate)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / timeToRotate);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRotation;
        isInteracted = !isInteracted;
        isDoorOpening = false;
    }

    private IEnumerator DrawerInteraction(float pullDistance)
    {
        if (isOpening) yield break; // Nếu ngăn kéo đang mở, không cho phép tương tác tiếp

        isOpening = true; // Đánh dấu đang mở
        float timeToMove = 0.8f;
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition;

        Vector3 moveDirection = transform.InverseTransformDirection(-transform.forward);

        if (!isInteracted)
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
        isOpening = false; // Cho phép tương tác lại sau khi hoàn thành mở/ngăn kéo
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

    public void ExamineItem()
    {
        // Show description
        ShowDescription();

        // Make item in center camera
        Camera mainCamera = Camera.main;
        ShuraCamera shuraCamera = mainCamera.GetComponent<ShuraCamera>();

        if (!isExamineMode)
        {
            // Enter examine mode
            shuraCamera.isProcessing = false;
            shuraCamera.OnOffMouseOption();

            isExamineMode = true;

            // Save the item's original position and rotation
            oldPositon = transform.position;
            oldRotation = transform.rotation;

            transform.position = mainCamera.transform.position + mainCamera.transform.forward * 0.5f;
        }
        else
        {
            // Exit examine item
            shuraCamera.isProcessing = true;
            shuraCamera.OnOffMouseOption();

            isExamineMode = false;

            // Turn off description
            MessageManager.Instance.HideMessage();

            if (itemType == ItemType.collectionItem)
            {
                CollectItem();
            }
            else if (itemType == ItemType.examineItem)
            {
                transform.position = oldPositon;
                transform.rotation = oldRotation;
                ShowMessage();
            }
        }
    }

    private void CollectItem()
    {
        // Kiểm tra xem có cần kích hoạt hiệu ứng kinh dị không
        if (triggerHorrorEffect && HorrorEffects.Instance != null)
        {
            // Kích hoạt hiệu ứng kinh dị
            HorrorEffects.Instance.TriggerHorrorEffect();

            // Delay nhặt đồ để hiệu ứng được hiển thị trước
            StartCoroutine(DelayedCollect());
        }
        else
        {
            // Nhặt đồ ngay lập tức nếu không kích hoạt hiệu ứng
            AddItemAndShowMessage();
        }
    }

    private IEnumerator DelayedCollect()
    {
        // Thay vì dùng shakeDuration (đã loại bỏ hiệu ứng rung),
        // ta sử dụng motionBlurDuration để chờ hiệu ứng nhòe hoàn thành.
        yield return new WaitForSeconds(HorrorEffects.Instance.motionBlurDuration);

        AddItemAndShowMessage();
    }

    private void AddItemAndShowMessage()
    {
        InventoryManager.Instance.AddItemToInventory(this);
        gameObject.SetActive(false);
        ShowMessage();
    }

    private void ShowDescription()
    {
        if (!string.IsNullOrEmpty(itemDescription))
        {
            MessageManager.Instance.ShowTextMessage(itemDescription);
        }
    }

    private void ShowMessage()
    {
        if (!string.IsNullOrEmpty(itemMessage))
        {
            MessageManager.Instance.ShowTextMessage(itemMessage);
        }
    }
}
