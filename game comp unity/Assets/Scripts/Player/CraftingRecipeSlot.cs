using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingRecipeSlot : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update

    public int index;
    public bool craftable = false;
    public GameObject playerGameObject;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData) {
        if (craftable) {
            GameObject.Find("player").GetComponent<Crafting>().craftItem(index);
        }
        
    }
}
