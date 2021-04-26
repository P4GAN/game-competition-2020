using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public int inventoryIndex;
    public PlayerInventory PlayerInventoryScript;
    public GameObject containedItem;

    public bool mouseOver = false;

    public GameObject itemCaption;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventoryScript = GameObject.Find("player").GetComponent<PlayerInventory>();
        itemCaption = GameObject.Find("ItemCaption");
    }

    // Update is called once per frame
    void Update()
    {
        itemCaption.transform.position = Input.mousePosition;       
        if (Input.GetMouseButtonDown(0) && mouseOver) {
            
            PlayerInventoryScript.InventorySlotLeftClicked(gameObject);
            
        }
        if (Input.GetMouseButtonDown(1) && mouseOver) {
            
            PlayerInventoryScript.InventorySlotRightClicked(gameObject);
            
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        itemCaption.SetActive(true);
        itemCaption.GetComponent<Text>().text = containedItem.GetComponent<ItemData>().itemCaptionString;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        itemCaption.SetActive(false);
    }

}
