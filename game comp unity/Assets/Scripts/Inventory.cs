using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{

    public int inventoryIndex;
    public GameObject itemControlGameObject;
    public List<int> InventoryItems;
    public List<int> InventoryAmounts;
    public Vector2 mousePos;
    
    public ItemControl ItemControlScript;
    public InventoryUI InventoryUIScript;

    // Start is called before the first frame update
    void Awake()
    {
        ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();
        InventoryUIScript = GetComponent<InventoryUI>();

        for (int i=0; i<20; i++) {
            InventoryItems.Add(0);
            InventoryAmounts.Add(0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            AsteroidBlockControl AsteroidBlockControlScript = ItemControlScript.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            GameObject block = AsteroidBlockControlScript.RemoveBlock(mousePos);
            if (block) {
                AddItem(block.GetComponent<ItemData>().itemID, 1);
            }

        }
        if (Input.GetMouseButtonDown(1)) {
            AsteroidBlockControl AsteroidBlockControlScript = ItemControlScript.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            if (InventoryAmounts[inventoryIndex] >= 1) {
                if (AsteroidBlockControlScript.PlaceBlock(InventoryItems[inventoryIndex], mousePos, true)) {
                    RemoveItemAtIndex(inventoryIndex, 1);
                }

            }

        }
        //if (Input.mouseScrollDelta.y > 0) {
        if (Input.GetKeyDown(KeyCode.M)) {
            inventoryIndex += 1;
        } 
        if (Input.GetKeyDown(KeyCode.N)) {
            inventoryIndex -= 1;
        } 
                

    }

    public void AddItem(int itemID, int amount) {
        Debug.Log("add");
        int index = InventoryItems.IndexOf(itemID);
        if (index != -1) {
            InventoryAmounts[index] += amount;
        }
        else {
            index = InventoryItems.IndexOf(0);
            InventoryItems[index] = itemID;
            InventoryAmounts[index] += amount;
        }
        InventoryUIScript.updateInventoryUI();

    }

    public bool ItemInInventory(int itemIndex, int amount) {
        int totalAmount = 0;
        for (int i = 0; i < InventoryItems.Count; i++) {
            if (InventoryItems[i] == itemIndex) { 
                totalAmount += InventoryAmounts[i];
            }
        }
        return (totalAmount >= amount);
    }

    public void RemoveItemAtIndex(int index, int amount) {
        InventoryAmounts[index] -= amount;
        InventoryUIScript.updateInventoryUI();

    }

    public void RemoveItem(int itemID, int amount) {
        for (int i = 0; i < InventoryItems.Count; i++) {
            if (InventoryItems[i] == itemID) { 
                if (amount <= InventoryAmounts[i]) {
                    InventoryAmounts[i] -= amount;
                    break;
                }
                else {
                    amount -= InventoryAmounts[i];
                    InventoryAmounts[i] = 0;
                }
            }
        } 
        InventoryUIScript.updateInventoryUI();
    }

}
