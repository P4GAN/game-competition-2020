using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    // Start is called before the first frame update

    public Inventory InventoryScript;
    public int inventoryIndex;
    public GameObject canvas;

    public GameObject inventoryPanel;

    public GameObject currentHeldGameObject;

    public GameObject emptyItem;

    public int hotbarSize;
    public GameObject hotbarIndicator;
    public GameObject hotbarGameObject;
    public List<GameObject> hotbarSlots;

    void Start()
    {
        InventoryScript = GetComponent<Inventory>();
        inventoryPanel = InventoryScript.InventoryPanelGameObjectInstance;
        hotbarIndicator = GameObject.Find("HotbarIndicator");
        for (int i = 0; i < 10; i++) {
            hotbarSlots[i] = GameObject.Find("HotbarSlot" + i.ToString());
        }
        currentHeldGameObject = GameObject.Find("00empty");
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab)) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);

            GameObject.Find("Canvas").transform.Find("CraftingPlayerContainer").gameObject.SetActive(inventoryPanel.activeSelf);
            GameObject.Find("Canvas").transform.Find("CraftingRefinerContainer").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("CraftingAssemblerContainer").gameObject.SetActive(false);
        }

        if (inventoryPanel.activeSelf) {
            currentHeldGameObject.transform.position = Input.mousePosition;
        }

        //if (Input.mouseScrollDelta.y > 0) {


        if (Input.GetKeyDown(KeyCode.M)) {
            inventoryIndex += 1;
        } 
        if (Input.GetKeyDown(KeyCode.N)) {
            inventoryIndex -= 1;
        } 
        inventoryIndex = inventoryIndex % hotbarSize;
        if (inventoryIndex < 0) {
            inventoryIndex = hotbarSize + inventoryIndex;
        }
        hotbarIndicator.transform.position = hotbarSlots[inventoryIndex].transform.position;
    }

    public void UpdateHotbarUI() {

        if (hotbarSlots[0] == null) {
            for (int i = 0; i < 10; i++) {
                hotbarSlots[i] = GameObject.Find("HotbarSlot" + i.ToString());
            }
        }

        for (int i = 0; i < hotbarSize; i++) {

            InventorySlot InventorySlotScript = hotbarSlots[i].GetComponent<InventorySlot>();
            Inventory InventoryScript = GetComponent<Inventory>();

            GameObject item = Instantiate(emptyItem, transform.position, Quaternion.identity);
            if (InventoryScript.InventoryItems[i].itemAmount != 0) {
                item.GetComponent<Image>().sprite = ItemControl.itemList[InventoryScript.InventoryItems[i].itemID].GetComponent<SpriteRenderer>().sprite;
                item.GetComponent<ItemData>().item = InventoryScript.InventoryItems[i];
            }

            Destroy(InventorySlotScript.containedItem);
            InventorySlotScript.containedItem = item;
            InventorySlotScript.containedItem.transform.SetParent(hotbarSlots[i].transform, false);
            InventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);
        

            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryScript.InventoryItems[i].itemAmount.ToString();
            if (InventoryScript.InventoryItems[i].itemAmount == 0 || InventoryScript.InventoryItems[i].itemAmount == 1) {
                InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
            }
            
        }
    }

    

    public void InventorySlotRightClicked(GameObject InventorySlotGameObject) {

        InventorySlot InventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();
        InventoryScript = GetComponent<Inventory>();

        //when inventory slot is right clicked, if there is no current held item and there is an item in the slot, split the slot 
        //and put the smaller half in the slot
        if (!InventorySlotScript.hotbarSlot) {
            if (currentHeldGameObject.GetComponent<ItemData>().item.itemID == 0) {
                Destroy(currentHeldGameObject);
                currentHeldGameObject = Instantiate(emptyItem, transform.position, Quaternion.identity);
                currentHeldGameObject.transform.SetParent(canvas.transform);

                currentHeldGameObject.GetComponent<ItemData>().item = InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].Clone();

                currentHeldGameObject.GetComponent<ItemData>().item.itemAmount = (InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount + 1)/2;
                currentHeldGameObject.GetComponent<Image>().sprite = ItemControl.itemList[currentHeldGameObject.GetComponent<ItemData>().item.itemID].GetComponent<SpriteRenderer>().sprite;

                currentHeldGameObject.GetComponentInChildren<Text>().text = currentHeldGameObject.GetComponent<ItemData>().item.itemAmount.ToString();
                if (currentHeldGameObject.GetComponent<ItemData>().item.itemAmount == 0 || currentHeldGameObject.GetComponent<ItemData>().item.itemAmount == 1) {
                    currentHeldGameObject.GetComponentInChildren<Text>().text = "";
                }

                InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount -= currentHeldGameObject.GetComponent<ItemData>().item.itemAmount;

            }
        }
        currentHeldGameObject.transform.position = Input.mousePosition;
        
        InventoryScript.UpdateInventoryUI();
    }

    public void InventorySlotLeftClicked(GameObject InventorySlotGameObject) {

        //if item in hand and item in slot are the same, add them together and put in the slot
        //if theyre different, swap the items 

        InventorySlot InventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();
        InventoryScript = GetComponent<Inventory>();

        if (InventorySlotScript.hotbarSlot) {
            inventoryIndex = InventorySlotScript.inventoryIndex;
        }
        else {

            if (InventorySlotScript.containedItem.GetComponent<ItemData>().item.itemID == currentHeldGameObject.GetComponent<ItemData>().item.itemID) {
                int newAmount = InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount + currentHeldGameObject.GetComponent<ItemData>().item.itemAmount;
                InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount = newAmount;
                Destroy(currentHeldGameObject);
                currentHeldGameObject = Instantiate(emptyItem, transform.position, Quaternion.identity);
            }
            else {
                Item temporaryItem = InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].Clone();
                InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex] = currentHeldGameObject.GetComponent<ItemData>().item.Clone();

                Destroy(currentHeldGameObject);

                currentHeldGameObject = Instantiate(emptyItem, transform.position, Quaternion.identity);

                currentHeldGameObject.GetComponent<ItemData>().item = temporaryItem; 


                Destroy(InventorySlotScript.containedItem);

            }
            currentHeldGameObject.GetComponent<Image>().sprite = ItemControl.itemList[currentHeldGameObject.GetComponent<ItemData>().item.itemID].GetComponent<SpriteRenderer>().sprite;
            currentHeldGameObject.transform.SetParent(canvas.transform);

            currentHeldGameObject.GetComponentInChildren<Text>().text = currentHeldGameObject.GetComponent<ItemData>().item.itemAmount.ToString();
            if (currentHeldGameObject.GetComponent<ItemData>().item.itemAmount == 0 || currentHeldGameObject.GetComponent<ItemData>().item.itemAmount == 1) {
                currentHeldGameObject.GetComponentInChildren<Text>().text = "";
            }
        }
        InventoryScript.UpdateInventoryUI();

        currentHeldGameObject.transform.position = Input.mousePosition;

    }
            

}
