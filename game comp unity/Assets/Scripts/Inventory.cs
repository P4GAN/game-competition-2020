using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int inventoryIndex;
    public BlockControl blockControlScript;
    public GameObject blockControlObject;
    public List<int> InventoryItems;
    public List<int> InventoryAmounts;
    public GameObject blockInstance;
    public GameObject player;
    public Vector2 mousePos;
    public Vector2 blockPos;
    public float timer = 0;
    public float breakBlockTime = 2.5f;
    public float playerReach = 5f;


    // Start is called before the first frame update
    void Start()
    {
        blockControlScript = blockControlObject.GetComponent<BlockControl>();
        for (int i=0; i<20; i++) {
            InventoryItems.Add(0);
            InventoryAmounts.Add(0);

        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blockPos = new Vector2 (Mathf.Ceil(mousePos.x) - 0.5f, Mathf.Ceil(mousePos.y) - 0.5f);
        if (Input.GetMouseButtonDown(0)) {
            blockControlScript.PlaceBlock(inventoryIndex, blockPos);
        }
        if (Input.GetMouseButtonDown(1)) {
            blockControlScript.RemoveBlock(blockPos);

        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            inventoryIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            inventoryIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            inventoryIndex = 3;
        }

    }

    public void AddItem(int itemID, int amount) {
        int index = InventoryItems.FindIndex(x => x == itemID);
        if (index != -1) {
            InventoryAmounts[index] += amount;
        }
        else {
            index = InventoryItems.FindIndex(x => x == 0);
            InventoryItems[index] = itemID;
            InventoryAmounts[index] += amount;
        }

    }

    public bool RemoveItem(int itemID, int amount) {
        int index = InventoryItems.FindIndex(x => x == itemID);
        if (index != -1) {
            if (amount <= InventoryAmounts[index]) {
                InventoryAmounts[index] -= amount;
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
}
