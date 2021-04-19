using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    // Start is called before the first frame update

    public int inventoryIndex;
    public GameObject itemControlGameObject;

    
    public ItemControl ItemControlScript;

    public Inventory InventoryScript;

    public GameObject canvas;

    public GameObject inventoryPanel;
    public GameObject craftingPanel;

    public GameObject currentHeldGameObject;
    public int currentHeldAmount;

    public GameObject emptyItem;

    public int hotbarSize;
    public GameObject hotbarIndicator;
    public GameObject hotbarGameObject;
    public List<GameObject> hotbarSlots;

    void Start()
    {
        InventoryScript = GetComponent<Inventory>();
        ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();
        inventoryPanel = InventoryScript.InventoryPanelGameObjectInstance;
        craftingPanel = GetComponent<Crafting>().CraftingPanel;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Tab)) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);

            GameObject.Find("Canvas").transform.Find("CraftingMenuPlayer").gameObject.SetActive(inventoryPanel.activeSelf);
            GameObject.Find("Canvas").transform.Find("CraftingMenuRefiner").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("CraftingMenuAssembler").gameObject.SetActive(false);
        }

        if (inventoryPanel.activeSelf) {
            currentHeldGameObject.transform.position = Input.mousePosition;
        }

        //if (Input.mouseScrollDelta.y > 0) {


        if (Input.GetKeyDown(KeyCode.M)) {
            inventoryIndex += 1;
            inventoryIndex = inventoryIndex % hotbarSize;
            hotbarIndicator.transform.position = hotbarSlots[inventoryIndex].transform.position;
        } 
        if (Input.GetKeyDown(KeyCode.N)) {
            inventoryIndex -= 1;
            inventoryIndex = inventoryIndex % hotbarSize;
            hotbarIndicator.transform.position = hotbarSlots[inventoryIndex % hotbarSize].transform.position;
        } 
    }

    public void UpdateHotbarUI() {
        for (int i = 0; i < hotbarSize; i++) {

            ItemControl ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();
            InventorySlot InventorySlotScript = hotbarSlots[i].GetComponent<InventorySlot>();
            Inventory InventoryScript = GetComponent<Inventory>();

            if (InventoryScript.InventoryItems[i] != InventorySlotScript.containedItem.GetComponent<ItemData>().itemID) {
                GameObject item = Instantiate(ItemControlScript.itemIconList[InventoryScript.InventoryItems[i]], transform.position, Quaternion.identity);      
                if (InventoryScript.InventoryAmounts[i] == 0) {
                    item = Instantiate(emptyItem, transform.position, Quaternion.identity);
                }

                Destroy(InventorySlotScript.containedItem);
                InventorySlotScript.containedItem = item;
                InventorySlotScript.containedItem.transform.SetParent(hotbarSlots[i].transform, false);
                InventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);
            }

            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryScript.InventoryAmounts[i].ToString();
            if (InventoryScript.InventoryAmounts[i] == 0 || InventoryScript.InventoryAmounts[i] == 1) {
                InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
            }
            
        }
    }

    

    public void InventorySlotRightClicked(GameObject InventorySlotGameObject) {

        InventorySlot InventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();
        InventoryScript = GetComponent<Inventory>();

        if (currentHeldGameObject.GetComponent<ItemData>().itemID == 0) {
            Destroy(currentHeldGameObject);
            currentHeldGameObject = Instantiate(InventorySlotScript.containedItem, transform.position, Quaternion.identity);
            currentHeldGameObject.transform.SetParent(canvas.transform);
            currentHeldGameObject.GetComponent<Image>().raycastTarget = false;

            currentHeldAmount = InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] - InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex]/2;

            currentHeldGameObject.GetComponentInChildren<Text>().text = currentHeldAmount.ToString();
            if (currentHeldAmount == 0 || currentHeldAmount == 1) {
                currentHeldGameObject.GetComponentInChildren<Text>().text = "";
            }


            InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] -= currentHeldAmount;
            if (InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] == 0 ) {
                Destroy(InventorySlotScript.containedItem);
                InventorySlotScript.containedItem = Instantiate(emptyItem, transform.position, Quaternion.identity);
                InventorySlotScript.containedItem.transform.SetParent(InventorySlotGameObject.transform, false);
                InventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);
            }
            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex].ToString();
            if (InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] == 0 || InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] == 1) {
                InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
            }
        }
        
        UpdateHotbarUI();
        GetComponent<Crafting>().updateCraftingRecipes();
    }

    public void InventorySlotLeftClicked(GameObject InventorySlotGameObject) {

        InventorySlot InventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();
        InventoryScript = GetComponent<Inventory>();

        if (InventorySlotScript.containedItem.GetComponent<ItemData>().itemID == currentHeldGameObject.GetComponent<ItemData>().itemID) {
            int newAmount = InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] + currentHeldAmount;
            InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] = newAmount;
            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = newAmount.ToString();
            Destroy(currentHeldGameObject);
            currentHeldGameObject = Instantiate(emptyItem, transform.position, Quaternion.identity);
        }
        else {
            GameObject temporaryContainedItem = InventorySlotScript.containedItem;
            InventorySlotScript.containedItem = currentHeldGameObject;

            InventorySlotScript.containedItem.transform.SetParent(InventorySlotGameObject.transform, false);
            InventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);

            currentHeldGameObject = temporaryContainedItem;
            currentHeldGameObject.transform.SetParent(canvas.transform);
            currentHeldGameObject.GetComponent<Image>().raycastTarget = false;

            InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex] = InventorySlotScript.containedItem.GetComponent<ItemData>().itemID;
            
            int temporaryAmount = InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex];
            InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] = currentHeldAmount;
            currentHeldAmount = temporaryAmount;

        }

        InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex].ToString();
        if (InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] == 0 || InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex] == 1 || InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex] == 0) {
            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
        } 

        UpdateHotbarUI();
        GetComponent<Crafting>().updateCraftingRecipes();
    }
}
