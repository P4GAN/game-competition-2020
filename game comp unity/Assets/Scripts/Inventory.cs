using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public List<Item> InventoryItems;
    public GameObject InventoryPanelGameObject;
    public GameObject InventoryPanelGameObjectInstance;

    public GameObject canvas;

    public GameObject InventorySlotGameObject;
    public List<GameObject> InventorySlots;

    public GameObject emptyItem;

    public Vector2 position;
    public int inventoryWidth;
    public int inventoryHeight;

    // Start is called before the first frame update
    void Awake()
    {
        canvas = GameObject.Find("Canvas");

        int inventorySize = inventoryWidth * inventoryHeight;

        InventoryPanelGameObjectInstance = Instantiate(InventoryPanelGameObject, position, Quaternion.identity);
        InventoryPanelGameObjectInstance.transform.SetParent(canvas.transform, false);
        Vector2 InventoryPanelSize = InventoryPanelGameObjectInstance.GetComponent<RectTransform>().sizeDelta;
        Vector2 InventorySlotSize = InventorySlotGameObject.GetComponent<RectTransform>().sizeDelta;

        int inventoryIndex = 0;
        for (int y = 0; y < inventoryHeight; y++) {
            for (int x = 0; x < inventoryWidth; x++) {
                position = new Vector2(((-InventoryPanelSize.x/2) + (InventorySlotSize.x/2) + 5 + (x * 110)), ((-InventoryPanelSize.y/2) + (InventorySlotSize.y/2) + 5 + (y * 110)));
                GameObject InventorySlotInstance = Instantiate(InventorySlotGameObject, position, Quaternion.identity); 
                InventorySlotInstance.GetComponent<InventorySlot>().inventoryIndex = inventoryIndex;
                inventoryIndex += 1;
                InventorySlotInstance.transform.SetParent(InventoryPanelGameObjectInstance.transform, false);
                InventorySlots.Add(InventorySlotInstance);
            }
        }

        UpdateInventoryUI();

    }

    // Update is called once per frame
    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.F)) {
            UpdateInventoryUI();
        }
    }

    public void UpdateInventoryUI() {
        PlayerInventory playerInventoryScript = GetComponent<PlayerInventory>();
        for (int i = 0; i < InventoryItems.Count; i++) {

            InventorySlot InventorySlotScript = InventorySlots[i].GetComponent<InventorySlot>();

            //make new item object to update
            GameObject newItem = Instantiate(emptyItem, transform.position, Quaternion.identity);
            if (InventoryItems[i].itemAmount != 0 && InventoryItems[i].itemID != 0) {
                newItem.GetComponent<Image>().sprite = ItemControl.itemList[InventoryItems[i].itemID].GetComponent<SpriteRenderer>().sprite;
                newItem.GetComponent<ItemData>().item = InventoryItems[i];
            }
            else {
                InventoryItems[i] = emptyItem.GetComponent<ItemData>().item.Clone();
            }

            Destroy(InventorySlotScript.containedItem);
            InventorySlotScript.containedItem = newItem;
            InventorySlotScript.containedItem.transform.SetParent(InventorySlots[i].transform, false);
            InventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);

            //update inventory amount


            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryItems[i].itemAmount.ToString();
            if (InventoryItems[i].itemAmount == 0 || InventoryItems[i].itemAmount == 1) {
                InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
            }
            
            if (playerInventoryScript) {
                playerInventoryScript.UpdateHotbarUI();
                GetComponent<Crafting>().updateCraftingRecipes();
            }
        }
    }


    public void AddItem(Item item, int amount) {
        int index = InventoryItems.FindIndex(x => x.itemID == item.itemID);
        if (index != -1) {
            InventoryItems[index].itemAmount += amount;
        }
        else {
            index = InventoryItems.FindIndex(x => x.itemID == 0);
            InventoryItems[index] = item;
            InventoryItems[index].itemAmount += amount;
        }
        UpdateInventoryUI();

    }

    public void AddItem(int itemID, int amount) {
        int index = InventoryItems.FindIndex(x => x.itemID == itemID);
        if (index != -1) {
            InventoryItems[index].itemAmount += amount;
        }
        else {
            index = InventoryItems.FindIndex(x => x.itemID == 0);
            InventoryItems[index] = ItemControl.itemList[itemID].GetComponent<ItemData>().item.Clone();
            InventoryItems[index].itemAmount += amount;
        }
        UpdateInventoryUI();

    }

    public bool ItemInInventory(int itemID, int amount) {
        int totalAmount = 0;
        for (int i = 0; i < InventoryItems.Count; i++) {
            if (InventoryItems[i].itemID == itemID) { 
                totalAmount += InventoryItems[i].itemAmount;
            }
        }
        return (totalAmount >= amount);
    }

    public void RemoveItemAtIndex(int index, int amount) {
        InventoryItems[index].itemAmount -= amount;
        if (InventoryItems[index].itemAmount == 0) {
            InventoryItems[index] = emptyItem.GetComponent<ItemData>().item.Clone();
        }
        UpdateInventoryUI();

    }

    public void RemoveItem(int itemID, int amount) {
        for (int i = 0; i < InventoryItems.Count; i++) {
            if (InventoryItems[i].itemID == itemID) { 
                if (amount < InventoryItems[i].itemAmount) {
                    InventoryItems[i].itemAmount -= amount;
                    break;
                }
                else {
                    amount -= InventoryItems[i].itemAmount;
                    InventoryItems[i] = emptyItem.GetComponent<ItemData>().item.Clone();
                }
            }
        } 
        UpdateInventoryUI();
    }

    


}
