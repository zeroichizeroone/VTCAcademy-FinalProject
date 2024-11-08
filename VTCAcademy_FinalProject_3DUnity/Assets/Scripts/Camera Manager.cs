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
                    case "door":
                        InteractiveObject(currentObject, "door");
                        break;

                    case "drawer":
                        InteractiveObject(currentObject, "drawer");
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
        if (door.transform.rotation.x == 0)
        {
            endRotation = Quaternion.Euler(1, door.transform.eulerAngles.y + 125, 0);
        }
        else
        {
            endRotation = Quaternion.Euler(0, door.transform.eulerAngles.y - 125, 0);
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



    private bool isDrawerMoved = false; // theo doi trang thai ngan keo
    private IEnumerator MoveDrawer(GameObject drawer, float distance)
    {
        float timeToMove = 0.8f;
        Vector3 startPosition = drawer.transform.localPosition;
        Vector3 endPosition;

        // kiem tra neu da di chuyen
        Vector3 moveDirection = drawer.transform.InverseTransformDirection(-drawer.transform.forward);

        if (!isDrawerMoved)
        {
            // Di chuyen ra ngoai
            endPosition = startPosition + moveDirection * distance;
            Debug.Log("Moving Drawer Out. End Position: " + endPosition);
            isDrawerMoved = true; // danh dau da di chuyen ra
        }
        else
        {
            // di chuyen ve vi tri ban dau
            endPosition = startPosition - moveDirection * distance;
            Debug.Log("Moving Drawer Back to Start. End Position: " + endPosition);
            isDrawerMoved = false; // danh dau da quay lai
        }

        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            drawer.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        drawer.transform.localPosition = endPosition; // dam bao ngan keo dat lai vi tri cuoi cung
    }


    private void InteractiveObject(GameObject interactiveObject, string type)
    {
        switch (type)
        {
            case "door":
                Debug.Log("ee");
                StartCoroutine(RotateDoor(interactiveObject));
                break;

            case "drawer":
                Debug.Log("cc");
                StartCoroutine(MoveDrawer(interactiveObject, 0.3f));
                break;
        }
    }
}
