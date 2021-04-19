using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClick : MonoBehaviour
{
    // Start is called before the first frame update

    public string blockType = "";
    public GameObject canvas;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if (blockType == "refiner") {
            Debug.Log("pos");
            GameObject.Find("Canvas").transform.Find("CraftingMenuRefiner").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("CraftingMenuPlayer").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("CraftingMenuAssembler").gameObject.SetActive(false);
            GameObject.Find("player").GetComponent<PlayerInventory>().inventoryPanel.SetActive(true);
        }
        if (blockType == "assembler") {
            GameObject.Find("Canvas").transform.Find("CraftingMenuRefiner").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("CraftingMenuPlayer").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("CraftingMenuAssembler").gameObject.SetActive(true);
            GameObject.Find("player").GetComponent<PlayerInventory>().inventoryPanel.SetActive(true);

        }
    }
  
}
