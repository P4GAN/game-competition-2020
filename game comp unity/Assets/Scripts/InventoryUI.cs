 /*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{

    public Inventory InventoryScript;


    public GameObject InventoryPanelGameObject;    
    public GameObject RefinerCraftingMenu;
    public GameObject AssemblerCraftingMenu;

    public List<GameObject> InventorySlots;

    public GameObject currentHeldGameObject;
    public int currentHeldAmount;
    public GameObject emptyItem;

    public GameObject canvas;

    public GameObject HotbarGameObject;
    public List<GameObject> HotbarSlots;

    // Start is called before the first frame update
    void Start()
    {
        Inventory InventoryScript = GetComponent<Inventory>();
        /*for (int i=0; i<HotbarIcons.Count; i++) {
            HotbarIconGameObjects[i].transform.localPosition = new Vector3((i * 100) - 400, -300, 0);
            HotbarIcons.Add(HotbarIconGameObjects[i].GetComponent<Image>());
            HotbarNumbers[i].transform.localPosition = new Vector3((i * 100) - 400, -300, 0) + new Vector3(30, -30, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            InventoryPanelGameObject.SetActive(!InventoryPanelGameObject.activeSelf);
            RefinerCraftingMenu.SetActive(false);
            AssemblerCraftingMenu.SetActive(false);
        }


        currentHeldGameObject.transform.position = Input.mousePosition;

    }

    /*GameObject updateInventorySlot(int inventoryIndex, GameObject newInventoryItem) {
        InventorySlot InventorySlotScript = InventorySlots[inventoryIndex].GetComponent<InventorySlot>();

        GameObject temporaryContainedItem = InventorySlotScript.containedItem;
        InventorySlotScript.containedItem = newInventoryItem;
        InventorySlotScript.containedItem.transform.parent = InventorySlots[inventoryIndex].transform;

        return temporaryContainedItem;
    }

    public void InventorySlotRightClicked(GameObject InventorySlotGameObject) {
        Inventory InventoryScript = GetComponent<Inventory>();
        InventorySlot InventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();

        if (currentHeldGameObject.GetComponent<ItemData>().item.itemID == 0) {
            Destroy(currentHeldGameObject);
            currentHeldGameObject = Instantiate(InventorySlotScript.containedItem, transform.position, Quaternion.identity);
            currentHeldGameObject.transform.SetParent(canvas.transform);
            currentHeldGameObject.GetComponent<Image>().raycastTarget = false;

            currentHeldAmount = (InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount + 1)/2;

            currentHeldGameObject.GetComponentInChildren<Text>().text = currentHeldAmount.ToString();
            if (currentHeldAmount == 0 || currentHeldAmount == 1) {
                currentHeldGameObject.GetComponentInChildren<Text>().text = "";
            }

            InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount -= currentHeldAmount;

            if (InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemID == 0 ) {
                Destroy(InventorySlotScript.containedItem);
                InventorySlotScript.containedItem = Instantiate(emptyItem, transform.position, Quaternion.identity);
                InventorySlotScript.containedItem.transform.SetParent(InventorySlotGameObject.transform, false);
                InventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);
            }
            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount.ToString();
            if (InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount == 0 || InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount == 1) {
                InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    public void InventorySlotLeftClicked(GameObject InventorySlotGameObject) {
        //Debug.Log(InventorySlotGameObject);
        InventorySlot InventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();
        Inventory InventoryScript = GetComponent<Inventory>();

        if (InventorySlotScript.containedItem.GetComponent<ItemData>().item.itemID == currentHeldGameObject.GetComponent<ItemData>().item.itemID) {
            int newAmount = InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount + currentHeldAmount;
            InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemAmount = newAmount;
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

            InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemID = InventorySlotScript.containedItem.GetComponent<ItemData>().item.itemID;
            
            int temporaryAmount = InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemID;
            InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemID = currentHeldAmount;
            currentHeldAmount = temporaryAmount;

        }

        InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemID.ToString();
        if (InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemID == 0 || InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemID == 1 || InventoryScript.InventoryItems[InventorySlotScript.inventoryIndex].itemID == 0) {
            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
        } 

        /*InventorySlotGameObject.GetComponentInChildren<Text>().text = InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex].ToString(); 
        currentHeldGameObject.GetComponentInChildren<Text>().text = currentHeldAmount.ToString();
        if (InventorySlotGameObject.GetComponentInChildren<Text>().text == "0") {
            InventorySlotGameObject.GetComponentInChildren<Text>().text = "";
        }
        if (currentHeldAmount == 0) {
            currentHeldGameObject.GetComponentInChildren<Text>().text = "";
        }

        
        if (currentHeldGameObject == null) {
            if (InventorySlotScript.containedItem != null) {
                currentHeldGameObject = InventorySlotScript.containedItem;
                Destroy(InventorySlotScript.containedItem);
            }
        }
        else {
            if (InventorySlotScript.containedItem == null) {
                InventorySlotScript.containedItem = currentHeldGameObject;
            }
            else {
                GameObject temporaryContainedItem = InventorySlotScript.containedItem;
                InventorySlotScript.containedItem = currentHeldGameObject;
                currentHeldGameObject = temporaryContainedItem;

            }
            
        }
    

    }

}
*/