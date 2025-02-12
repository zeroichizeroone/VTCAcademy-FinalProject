using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Inventory userInventory = new Inventory(); // Khởi tạo luôn tránh lỗi null

    [Header("UI")]
    public GameObject inventoryUI;
    public GameObject bag;
    public GameObject itemPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItemToInventory(Item itemCollect)
    {
        userInventory.AddItem(itemCollect);

        // Add item in UI
        GameObject newItem = Instantiate(itemPrefab, bag.transform);
    }

    public void ActiveInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    // Class Inventory chứa danh sách các vật phẩm của người chơi
    public class Inventory
    {
        private List<Item> inventoryItems = new List<Item>();

        public void AddItem(Item item)
        {
            inventoryItems.Add(item);
        }

        public List<Item> GetItemList()
        {
            return inventoryItems;
        }
    }

    //  Kiểm tra xem Inventory có chứa vật phẩm có tag nhất định hay không
    public bool HasItem(string itemTag)
    {
        foreach (var item in userInventory.GetItemList())
        {
            if (item.CompareTag(itemTag)) // So sánh tag của vật phẩm
                return true;
        }
        return false;
    }
}