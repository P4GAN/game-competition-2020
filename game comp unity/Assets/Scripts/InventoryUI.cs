 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{

    public Inventory InventoryScript;
    public ItemControl ItemControlScript;

    public GameObject itemControlGameObject;

    public List<GameObject> HotbarIcons;

    public bool inventoryVisible = false;
    public GameObject InventoryGameObject;
    public List<GameObject> InventorySlots;

    public GameObject currentHeldGameObject;
    public int currentHeldAmount;
    public GameObject emptyItem;

    public GameObject canvas;

    // Start is called before the first frame update
    void Awake()
    {
        Inventory InventoryScript = GetComponent<Inventory>();
        ItemControl ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();
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
            ItemControl ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();
            InventorySlot InventorySlotScript = InventorySlots[i].GetComponent<InventorySlot>();


            if (InventoryScript.InventoryItems[i] != InventorySlotScript.containedItem.GetComponent<ItemData>().itemID) {
                GameObject item = Instantiate(ItemControlScript.itemIconList[InventoryScript.InventoryItems[i]], transform.position, Quaternion.identity);      
                if (InventoryScript.InventoryAmounts[i] == 0) {
                    item = Instantiate(emptyItem, transform.position, Quaternion.identity);
                }

                Destroy(InventorySlotScript.containedItem);
                InventorySlotScript.containedItem = item;
                InventorySlotScript.containedItem.transform.SetParent(InventorySlots[i].transform, false);
                InventorySlotScript.containedItem.transform.localPosition = new Vector2(0, 0);
            }

            InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = InventoryScript.InventoryAmounts[i].ToString();
            if (InventoryScript.InventoryAmounts[i] == 0 || InventoryScript.InventoryAmounts[i] == 1) {
                InventorySlotScript.containedItem.GetComponentInChildren<Text>().text = "";
            }
            
        }
    }

    GameObject updateInventorySlot(int inventoryIndex, GameObject newInventoryItem) {
        InventorySlot InventorySlotScript = InventorySlots[inventoryIndex].GetComponent<InventorySlot>();

        GameObject temporaryContainedItem = InventorySlotScript.containedItem;
        InventorySlotScript.containedItem = newInventoryItem;
        InventorySlotScript.containedItem.transform.parent = InventorySlots[inventoryIndex].transform;

        return temporaryContainedItem;
    }

    public void InventorySlotRightClicked(GameObject InventorySlotGameObject) {
        Debug.Log("r");
        Inventory InventoryScript = GetComponent<Inventory>();
        InventorySlot InventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();

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
    }

    public void InventorySlotLeftClicked(GameObject InventorySlotGameObject) {
        Debug.Log("l");
        //Debug.Log(InventorySlotGameObject);
        InventorySlot InventorySlotScript = InventorySlotGameObject.GetComponent<InventorySlot>();
        Inventory InventoryScript = GetComponent<Inventory>();

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

        /*InventorySlotGameObject.GetComponentInChildren<Text>().text = InventoryScript.InventoryAmounts[InventorySlotScript.inventoryIndex].ToString(); 
        currentHeldGameObject.GetComponentInChildren<Text>().text = currentHeldAmount.ToString();
        if (InventorySlotGameObject.GetComponentInChildren<Text>().text == "0") {
            InventorySlotGameObject.GetComponentInChildren<Text>().text = "";
        }
        if (currentHeldAmount == 0) {
            currentHeldGameObject.GetComponentInChildren<Text>().text = "";
        }*/

        
        /*if (currentHeldGameObject == null) {
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
            
        }*/

    }

}
