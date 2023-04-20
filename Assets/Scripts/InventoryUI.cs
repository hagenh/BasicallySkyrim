using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    InventorySlot[] slots;
    Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        UpdateUI();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Inventory"))
        {
            Debug.Log("Inventory button pressed");
            gameObject.SetActive(!inventoryUI.activeSelf);
        }
    }

    public void UpdateUI()
    {
        Debug.Log("UpdateUI ran!");
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
