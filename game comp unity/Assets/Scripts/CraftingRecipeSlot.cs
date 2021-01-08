using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeSlot : MonoBehaviour
{
    // Start is called before the first frame update

    public int index;
    public bool craftable = false;
    public GameObject playerGameObject;
    public Crafting CraftingScript;

    void Start()
    {
        CraftingScript = playerGameObject.GetComponent<Crafting>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && craftable){
            CraftingScript.craftItem(index);
        }
    }
}
