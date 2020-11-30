﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{

    public Inventory InventoryScript;
    
    public BlockControl blockControlScript;
    public GameObject blockControlGameObject;

    public List<GameObject> HotbarIcons;

    public bool inventoryVisible = false;
    public GameObject InventoryGameObject;
    public List<GameObject> InventorySlots;

    public GameObject currentHeldGameObject;
    public int currentHeldAmount;
    public GameObject emptyItem;

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        Inventory InventoryScript = GetComponent<Inventory>();
        BlockControl blockControlScript = blockControlGameObject.GetComponent<BlockControl>();
        /*for (int i=0; i<HotbarIcons.Count; i++) {
            HotbarIconGameObjects[i].transform.localPosition = new Vector3((i * 100) - 400, -300, 0);
            HotbarIcons.Add(HotbarIconGameObjects[i].GetComponent<Image>());
            HotbarNumbers[i].transform.localPosition = new Vector3((i * 100) - 400, -300, 0) + new Vector3(30, -30, 0);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            InventoryGameObject.SetActive(!InventoryGameObject.activeSelf);
        }
        currentHeldGameObject.transform.position = Input.mousePosition;

    }


    public void updateInventoryUI() {
        for (int i=0; i<5; i++) {
            //HotbarIcons[i].sprite = blockControlScript.itemIcons[InventoryScript.InventoryItems[i]];
            //HotbarNumbers[i].text = InventoryScript.InventoryAmounts[i].ToString();
            Inventory InventoryScript = GetComponent<Inventory>();
            BlockControl blockControlScript = blockControlGameObject.GetComponent<BlockControl>();

            GameObject item = Instantiate(blockControlScript.itemIconList[InventoryScript.InventoryItems[i]], transform.position, Quaternion.identity);
            InventorySlot inventorySlotScript = InventorySlots[i].GetComponent<InventorySlot>();
            Destroy(inventorySlotScript.containedItem);
            inventorySlotScript.containedItem = item;
            inventorySlotScript.containedItem.transform.SetParent(InventorySlots[i].transform, false);
            inventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);

            inventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryScript.InventoryAmounts[i].ToString();
        }
    }

    GameObject updateInventorySlot(int inventoryIndex, GameObject newInventoryItem) {
        InventorySlot inventorySlotScript = InventorySlots[inventoryIndex].GetComponent<InventorySlot>();

        GameObject temporaryContainedItem = inventorySlotScript.containedItem;
        inventorySlotScript.containedItem = newInventoryItem;
        inventorySlotScript.containedItem.transform.parent = InventorySlots[inventoryIndex].transform;

        return temporaryContainedItem;
    }

    public void InventorySlotClicked(GameObject InventorySlotGameObject) {
        //Debug.Log(InventorySlotGameObject);
        InventorySlot inventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();
        Inventory InventoryScript = GetComponent<Inventory>();

        GameObject temporaryContainedItem = inventorySlotScript.containedItem;
        inventorySlotScript.containedItem = currentHeldGameObject;

        inventorySlotScript.containedItem.transform.SetParent(InventorySlotGameObject.transform, false);
        inventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);

        currentHeldGameObject = temporaryContainedItem;
        currentHeldGameObject.transform.SetParent(canvas.transform);
        currentHeldGameObject.transform.SetAsFirstSibling();

        InventoryScript.InventoryItems[inventorySlotScript.inventoryIndex] = inventorySlotScript.containedItem.GetComponent<ItemData>().itemID;
        
        int temporaryAmount = InventoryScript.InventoryAmounts[inventorySlotScript.inventoryIndex];
        InventoryScript.InventoryAmounts[inventorySlotScript.inventoryIndex] = currentHeldAmount;
        currentHeldAmount = temporaryAmount;

        /*InventorySlotGameObject.GetComponentInChildren<Text>().text = InventoryScript.InventoryAmounts[inventorySlotScript.inventoryIndex].ToString(); 
        currentHeldGameObject.GetComponentInChildren<Text>().text = currentHeldAmount.ToString();
        if (InventorySlotGameObject.GetComponentInChildren<Text>().text == "0") {
            InventorySlotGameObject.GetComponentInChildren<Text>().text = "";
        }
        if (currentHeldAmount == 0) {
            currentHeldGameObject.GetComponentInChildren<Text>().text = "";
        }*/

        
        /*if (currentHeldGameObject == null) {
            if (inventorySlotScript.containedItem != null) {
                currentHeldGameObject = inventorySlotScript.containedItem;
                Destroy(inventorySlotScript.containedItem);
            }
        }
        else {
            if (inventorySlotScript.containedItem == null) {
                inventorySlotScript.containedItem = currentHeldGameObject;
            }
            else {
                GameObject temporaryContainedItem = inventorySlotScript.containedItem;
                inventorySlotScript.containedItem = currentHeldGameObject;
                currentHeldGameObject = temporaryContainedItem;

            }
            
        }*/

    }

}
