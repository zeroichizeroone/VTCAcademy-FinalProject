using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Attribute Camera")]
    public Transform target;
    [SerializeField] private Vector3 offset = new Vector3 (0, 0.5f, 0f);
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

    [Header("Door Interaction")]
    public GameObject detectObjectPoint;
    public float rayDistance = 5f;
    public LayerMask detectableLayer;
    private GameObject currentObject;

    // Start is called before the first frame update
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
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        CameraMove();
        rotation = Quaternion.Euler(-y, x, 0);

        Vector3 distanceVector = offset;
        Vector3 position = rotation * distanceVector + target.position;
        transform.rotation = rotation;
        transform.position = position;
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
        // Get ray point in middle screen
        Ray ray =  Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, detectableLayer))
        {
            currentObject = hit.collider.gameObject;
            detectObjectPoint.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (currentObject.tag)
                {
                    case "pickup":
                        InteractiveObject(currentObject, "pickup");
                        break;
                    case "cupboard":
                        InteractiveObject(currentObject, "cupboard"); 
                        break;
                     
                    case "door":
                        InteractiveObject(currentObject, "door");
                        break;

                    case "drawer":
                        InteractiveObject(currentObject, "drawer");
                        break;
                    case "item":
                        InteractiveObject(currentObject, "item");
                        break;

                }
            }
        }
        else
        {
            detectObjectPoint.SetActive(false);
        }
    }

    private void Update()
    {
        CheckObject();
    }

    private IEnumerator RotateDoor(GameObject door)
    {
        float timeToRotate = 1.6f;
        Quaternion startRotation = door.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(door.transform.rotation.x, door.transform.rotation.y, door.transform.rotation.z);
        if (door.name.Substring(door.name.Length - 3) != "_ON")
        {
            endRotation = Quaternion.Euler(1, door.transform.eulerAngles.y + 125, 0);
            AddObjectName(door, "_ON");
        }
        else
        {
            endRotation = Quaternion.Euler(0, door.transform.eulerAngles.y - 125, 0);
            RemoveEndName(door, 3);
        }


        float elapsedTime = 0;

        while (elapsedTime < timeToRotate)
        {
            door.transform.rotation = Quaternion.Lerp(startRotation, endRotation, (elapsedTime / timeToRotate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.rotation = endRotation;
    }

    private IEnumerator RotateCupboard(GameObject cupboard)
    {
        float timeToRotate = 1.6f;
        Quaternion startRotation = cupboard.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(cupboard.transform.rotation.x, cupboard.transform.rotation.y, cupboard.transform.rotation.z);
        if (cupboard.name.Substring(cupboard.name.Length - 3) != "_ON")
        {
            endRotation = Quaternion.Euler(1, cupboard.transform.eulerAngles.y + 90, 0);
            AddObjectName(cupboard, "_ON");
        }
        else
        {
            endRotation = Quaternion.Euler(0, cupboard.transform.eulerAngles.y - 90, 0);
            RemoveEndName(cupboard, 3);
        }


        float elapsedTime = 0;

        while (elapsedTime < timeToRotate)
        {
            cupboard.transform.rotation = Quaternion.Lerp(startRotation, endRotation, (elapsedTime / timeToRotate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cupboard.transform.rotation = endRotation;
    }

    private IEnumerator MoveDrawer(GameObject drawer, float distance)
    {
        float timeToMove = 0.8f;
        Vector3 startPosition = drawer.transform.localPosition;
        Vector3 endPosition;

        Vector3 moveDirection = drawer.transform.InverseTransformDirection(-drawer.transform.forward);

        if (drawer.name.Substring(drawer.gameObject.name.Length - 3) != "_ON")
        {
            endPosition = startPosition + moveDirection * distance;
            AddObjectName(drawer, "_ON");
        }
        else
        {
            RemoveEndName(drawer, 3);
            endPosition = startPosition - moveDirection * distance;
        }

        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            drawer.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        drawer.transform.localPosition = endPosition;
    }


    private void InteractiveObject(GameObject interactiveObject, string type)
    {
        switch (type)
        {
            case "pickup":
                StartCoroutine(RotateCupboard(interactiveObject));
                break;

            case "cupboard":
                StartCoroutine(RotateCupboard(interactiveObject)); 
                break;
            case "door":
                StartCoroutine(RotateDoor(interactiveObject));
                break;

            case "drawer":
                StartCoroutine(MoveDrawer(interactiveObject, 0.3f));
                break;

            case "item":
                MessageManager.Instance.ShowTextMessage(interactiveObject.GetComponent<ItemInGame>().itemMessage);
                break;

        }
    }

    private void PlaceItemInFront(GameObject item)
    {
        // Đặt vật phẩm ở trước mặt nhân vật
        Vector3 positionInFront = target.position + target.forward * 1.5f + Vector3.up * 1f; // Bắt đầu từ vị trí cao hơn để rơi xuống
        item.transform.position = positionInFront;
        item.transform.rotation = Quaternion.identity; // Đặt hướng mặc định

        // Bỏ parent nếu vật phẩm đang được cầm trên tay
        item.transform.SetParent(null);

        // Bắt đầu hiệu ứng rơi
        StartCoroutine(DropItemEffect(item));
    }

    private IEnumerator DropItemEffect(GameObject item)
    {
        float elapsedTime = 0f;
        float dropDuration = 0.5f; // Thời gian rơi (tùy chỉnh)
        Vector3 startPosition = item.transform.position;
        Vector3 endPosition = startPosition - Vector3.up * 0.5f; // Rơi xuống 0.5 đơn vị

        while (elapsedTime < dropDuration)
        {
            item.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / dropDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        item.transform.position = endPosition; // Đảm bảo đã đặt chính xác vị trí cuối
    }

    private void AddObjectName(GameObject crrObject, string newName)
    {
        crrObject.name += newName;        
    }

    private void RemoveEndName(GameObject crrObject, int index)
    {
        crrObject.name = crrObject.name.Substring(0, crrObject.name.Length - index);
    }
}
