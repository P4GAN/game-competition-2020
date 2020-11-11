using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{

    public int inventoryIndex;
    public GameObject blockControlGameObject;
    public List<int> InventoryItems;
    public List<int> InventoryAmounts;
    public GameObject blockInstance;
    public Vector2 mousePos;
    public Vector2 blockPos;
    public float timer = 0;
    public BlockControl blockControlScript;
    public List<GameObject> HotbarIconGameObjects;
    public List<Image> HotbarIcons;
    public List<Text> HotbarNumbers;



    // Start is called before the first frame update
    void Start()
    {
        blockControlScript = blockControlGameObject.GetComponent<BlockControl>();
        for (int i=0; i<20; i++) {
            InventoryItems.Add(0);
            InventoryAmounts.Add(0);
        }
        for (int i=0; i<HotbarIconGameObjects.Count; i++) {
            HotbarIconGameObjects[i].transform.localPosition = new Vector3((i * 100) - 400, -300, 0);
            HotbarIcons.Add(HotbarIconGameObjects[i].GetComponent<Image>());
            HotbarNumbers[i].transform.localPosition = new Vector3((i * 100) - 400, -300, 0) + new Vector3(30, -30, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            AsteroidBlockControl asteroidBlockControlScript = blockControlScript.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            GameObject block = asteroidBlockControlScript.RemoveBlock(mousePos);
            Debug.Log(block);
            if (block) {
                AddItem(block.GetComponent<BlockData>().blockID, 1);
            }

        }
        if (Input.GetMouseButtonDown(1)) {
            AsteroidBlockControl asteroidBlockControlScript = blockControlScript.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            if (!asteroidBlockControlScript.IsOccupied(mousePos)) {
                if (RemoveItem(inventoryIndex, 1)) {
                    asteroidBlockControlScript.PlaceBlock(InventoryItems[inventoryIndex], mousePos, true);
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
        int index = InventoryItems.IndexOf(itemID);
        if (index != -1) {
            InventoryAmounts[index] += amount;
        }
        else {
            index = InventoryItems.IndexOf(0);
            InventoryItems[index] = itemID;
            InventoryAmounts[index] += amount;
        }
        updateHotbarIcons();

    }

    public bool RemoveItem(int index, int amount) {
        if (index != -1) {
            if (amount <= InventoryAmounts[index]) {
                InventoryAmounts[index] -= amount;
                updateHotbarIcons();
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

    public void updateHotbarIcons() {
        for (int i=0; i<9; i++) {
            HotbarIcons[i].sprite = blockControlScript.blockSprites[InventoryItems[i]];
            HotbarNumbers[i].text = InventoryAmounts[i].ToString();
        }
    }
}
