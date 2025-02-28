using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevealItem : MonoBehaviour
{
    public string requiredItem = "bb1"; // Vật phẩm cần có để kích hoạt
    public GameObject hiddenObject; // Vật phẩm bị ẩn (cần hiện ra)
    public GameObject pressEUI; // UI hiện "Nhấn E" (hiển thị khi va chạm)

    private bool isPlayerNearby = false; // Kiểm tra player có gần không

    private void Start()
    {
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false); // Đảm bảo vật phẩm bị ẩn ban đầu
        }

        if (pressEUI != null)
        {
            pressEUI.SetActive(false); // Ẩn UI "Nhấn E" khi chưa va chạm
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Nếu người chơi gần và nhấn E
        {
            if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItem))
            {
                hiddenObject.SetActive(true); // Hiện vật phẩm bị ẩn
                Debug.Log("Vật phẩm đã xuất hiện!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Nếu nhân vật va chạm vùng kích hoạt
        {
            isPlayerNearby = true;

            if (pressEUI != null)
            { 
                Debug.Log("covokhong");
                pressEUI.SetActive(true); // Hiện UI "Nhấn E"
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Khi Player rời khỏi vùng
        {
            isPlayerNearby = false;

            if (pressEUI != null)
            {
                pressEUI.SetActive(false); // Ẩn UI "Nhấn E"
            }
        }
    }
}
