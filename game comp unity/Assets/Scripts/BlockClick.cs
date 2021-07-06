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

    public void BlockClicked() {
        Debug.Log("fff");
        if (blockType == "refiner") {
            Debug.Log("pos");
            SceneReferences.CraftingMenuRefiner.SetActive(true);
            SceneReferences.CraftingMenuPlayer.SetActive(false);
            SceneReferences.CraftingMenuAssembler.SetActive(false);
            WorldBuilder.player.GetComponent<PlayerInventory>().inventoryPanel.SetActive(true);
        }
        if (blockType == "assembler") {
            SceneReferences.CraftingMenuRefiner.SetActive(false);
            SceneReferences.CraftingMenuPlayer.SetActive(false);
            SceneReferences.CraftingMenuAssembler.SetActive(true);
            WorldBuilder.player.GetComponent<PlayerInventory>().inventoryPanel.SetActive(true);

        }
        if (blockType == "cockpit") {
            if (!transform.parent.gameObject.GetComponent<ShipControl>().shipCreated) {
                transform.parent.gameObject.GetComponent<ShipControl>().CreateShip(gameObject);
            }
            else {
                //transform.parent.gameObject.GetComponent<ShipControl>().SelectShip(gameObject);

            }
            
        }
    }
  
}
