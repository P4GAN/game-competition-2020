using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public int inventoryIndex;
    public GameObject player;
    public InventoryUI InventoryUIScript;
    public GameObject containedItem;

    public bool mouseOver = false;

    // Start is called before the first frame update
    void Start()
    {
        InventoryUIScript = player.GetComponent<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            
            if (mouseOver){
                Debug.Log("t");
                InventoryUIScript.InventorySlotClicked(gameObject);
            }
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(gameObject);
        Debug.Log("enter");
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(gameObject);
        Debug.Log("exit");
        mouseOver = false;
    }

}
