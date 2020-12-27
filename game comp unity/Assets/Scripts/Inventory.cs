using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{

    public int inventoryIndex;
    public GameObject blockControlGameObject;
    public List<int> InventoryItems;
    public List<int> InventoryAmounts;
    public GameObject blockInstance;
    public Vector2 mousePos;
    public Vector2 blockPos;
    
    public BlockControl blockControlScript;
    public InventoryUI InventoryUIScript;

    // Start is called before the first frame update
    void Start()
    {
        blockControlScript = blockControlGameObject.GetComponent<BlockControl>();
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
            AsteroidBlockControl asteroidBlockControlScript = blockControlScript.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            GameObject block = asteroidBlockControlScript.RemoveBlock(mousePos);
            if (block) {
                AddItem(block.GetComponent<ItemData>().itemID, 1);
            }

        }
        if (Input.GetMouseButtonDown(1)) {
            AsteroidBlockControl asteroidBlockControlScript = blockControlScript.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            if (ItemInInventory(inventoryIndex, 1)) {
                if (asteroidBlockControlScript.PlaceBlock(InventoryItems[inventoryIndex], mousePos, true)) {
                    RemoveItem(inventoryIndex, 1);
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
        InventoryUIScript = GetComponent<InventoryUI>();
        InventoryUIScript.updateInventoryUI();

    }

    public bool ItemInInventory(int index, int amount) {
        if (index != -1 && amount <= InventoryAmounts[index]) {
            return true;
        }
        return false;
    }

    public void RemoveItem(int index, int amount) {
        Debug.Log("remove");
        InventoryAmounts[index] -= amount;
        InventoryUIScript = GetComponent<InventoryUI>();
        InventoryUIScript.updateInventoryUI();

    }

}
