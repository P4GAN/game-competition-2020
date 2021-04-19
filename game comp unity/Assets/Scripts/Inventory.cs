using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public GameObject itemControlGameObject;
    public ItemControl ItemControlScript;

    public List<int> InventoryItems;
    public List<int> InventoryAmounts;

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
        ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();  

        int inventorySize = inventoryWidth * inventoryHeight;

        for (int i = 0; i < inventorySize; i++) {
            InventoryItems.Add(0);
            InventoryAmounts.Add(0);
        }

        InventoryItems[0] = 17;
        InventoryItems[1] = 18;
        InventoryItems[2] = 14;
        InventoryItems[3] = 15;
        InventoryItems[4] = 16;

        InventoryAmounts[0] = 1;
        InventoryAmounts[1] = 1;
        InventoryAmounts[2] = 1;
        InventoryAmounts[3] = 1;
        InventoryAmounts[4] = 100;


        InventoryPanelGameObjectInstance = Instantiate(InventoryPanelGameObject, position, Quaternion.identity);
        InventoryPanelGameObjectInstance.transform.SetParent(canvas.transform, false);
        Vector2 InventoryPanelSize = InventoryPanelGameObjectInstance.GetComponent<RectTransform>().sizeDelta;
        Vector2 InventorySlotSize = InventorySlotGameObject.GetComponent<RectTransform>().sizeDelta;

        int inventoryIndex = 10;
        for (int y = 0; y < inventoryHeight; y++) {
            for (int x = 0; x < inventoryWidth; x++) {
                Vector2 position = new Vector2(((-InventoryPanelSize.x/2) + (InventorySlotSize.x/2) + 5 + (x * 110)), ((-InventoryPanelSize.y/2) + (InventorySlotSize.y/2) + 5 + (y * 110)));
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

    }

    public void UpdateInventoryUI() {
        PlayerInventory playerInventoryScript = GetComponent<PlayerInventory>();
        for (int i = playerInventoryScript.hotbarSize; i < InventoryItems.Count; i++) {

            ItemControl ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();
            InventorySlot InventorySlotScript = InventorySlots[i].GetComponent<InventorySlot>();


            if (InventoryItems[i] != InventorySlotScript.containedItem.GetComponent<ItemData>().itemID) {
                GameObject item = Instantiate(ItemControlScript.itemIconList[InventoryItems[i]], transform.position, Quaternion.identity);      
                if (InventoryAmounts[i] == 0) {
                    item = Instantiate(emptyItem, transform.position, Quaternion.identity);
                }

                Destroy(InventorySlotScript.containedItem);
                InventorySlotScript.containedItem = item;
                InventorySlotScript.containedItem.transform.SetParent(InventorySlots[i].transform, false);
                InventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);
            }

            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryAmounts[i].ToString();
            if (InventoryAmounts[i] == 0 || InventoryAmounts[i] == 1) {
                InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
            }
            
            if (playerInventoryScript) {
                playerInventoryScript.UpdateHotbarUI();
                GetComponent<Crafting>().updateCraftingRecipes();
            }
        }
    }


    public void AddItem(int itemID, int amount) {
        int index = InventoryItems.IndexOf(itemID);
        if (index != -1) {
            InventoryAmounts[index] += amount;
        }
        else {
            index = InventoryItems.IndexOf(0);
            InventoryItems[index] = itemID;
            InventoryAmounts[index] += amount;
        }
        UpdateInventoryUI();

    }

    public bool ItemInInventory(int itemID, int amount) {
        int totalAmount = 0;
        for (int i = 0; i < InventoryItems.Count; i++) {
            if (InventoryItems[i] == itemID) { 
                totalAmount += InventoryAmounts[i];
            }
        }
        return (totalAmount >= amount);
    }

    public void RemoveItemAtIndex(int index, int amount) {
        InventoryAmounts[index] -= amount;
        if (InventoryAmounts[index] == 0) {
            InventoryItems[index] = 0;
        }
        UpdateInventoryUI();

    }

    public void RemoveItem(int itemID, int amount) {
        for (int i = 0; i < InventoryItems.Count; i++) {
            if (InventoryItems[i] == itemID) { 
                if (amount < InventoryAmounts[i]) {
                    InventoryAmounts[i] -= amount;
                    break;
                }
                else {
                    amount -= InventoryAmounts[i];
                    InventoryAmounts[i] = 0;
                    InventoryItems[i] = 0;
                }
            }
        } 
        UpdateInventoryUI();
    }

    


}
