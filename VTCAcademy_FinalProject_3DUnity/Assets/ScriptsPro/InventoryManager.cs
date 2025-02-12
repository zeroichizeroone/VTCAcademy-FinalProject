using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Inventory userInventory;

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

    private void Start()
    {
        userInventory = new Inventory();
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
}
