using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public Inventory userInventory = new Inventory(); // Khởi tạo luôn tránh lỗi null
    public List<Item> globalItemList = new List<Item>();

    [Header("UI")]
    public GameObject inventoryUI;
    public GameObject bag;
    public GameObject itemPrefab;

    [Header("Item Information")]
    public GameObject itemInformationForm;
    public Text textItemName;
    public Image itemImage;
    public Text textDescription;
    public Text textMessage;

    private Item currentItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGlobalInventory();
    }

    private void InitializeGlobalInventory()
    {
        Item[] allItems = FindObjectsOfType<Item>();
        foreach (Item item in allItems)
        {
            if (!globalItemList.Contains(item))
            {
                globalItemList.Add(item);
            }
        }
    }

    public void AddItemToInventory(Item itemCollect)
    {
        userInventory.AddItem(itemCollect);

        // Add item in UI
        GameObject newItem = Instantiate(itemPrefab, bag.transform);
        if (itemCollect.itemImage != null)
        {
            newItem.GetComponent<Image>().sprite = itemCollect.itemImage;
        }

        newItem.GetComponent<Button>().onClick.AddListener(() => ShowItemInfo(itemCollect));
    }

    public void ShowItemInfo(Item crrItem)
    { 
        itemInformationForm.SetActive(true);
        textItemName.text = crrItem.name;
        itemImage.sprite = crrItem.itemImage;
        textDescription.text = crrItem.itemDescription;
        textMessage.text = crrItem.itemMessage;
    }

    public void ActiveInventory()
    {
        Camera mainCamera = Camera.main;
        ShuraCamera shuraCamera = mainCamera.GetComponent<ShuraCamera>();

        if (inventoryUI.activeSelf)
        {
            inventoryUI.SetActive(false);
            shuraCamera.isProcessing = true;
        }
        else
        {
            inventoryUI.SetActive(true);
            shuraCamera.isProcessing = false;
        }
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