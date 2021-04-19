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
    public GameObject itemCaptionInstance;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventoryScript = GameObject.Find("player").GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        mouseOver = false;
    }

}
