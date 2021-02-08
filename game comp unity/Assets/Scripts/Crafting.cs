using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public GameObject CraftingPanel;
    public GameObject CraftingMenuGameObject;
    public GameObject CraftingRecipeGameObject;
    public int craftingRecipeDistance = 25;
    public int itemIconDistance = 20;
    public Vector2 itemIconSize;
    public List<Dictionary<int, int>> CraftingRecipeList = new List<Dictionary<int, int>>(){
        new Dictionary<int, int>(){
            {4, 1}

        },
    };

    public List<int> CraftingResultList;
    public List<int> CraftingResultAmountList;


    public List<GameObject> CraftingRecipeGameObjectList;

    public Inventory InventoryScript;
    public ItemControl ItemControlScript;

    public GameObject itemControlGameObject;


    // Start is called before the first frame update
    void Start()
    {
        InventoryScript = GetComponent<Inventory>();
        ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();
        Debug.Log(CraftingRecipeList.Count);
        for (int i = 0; i < CraftingRecipeList.Count; i++) {
            Vector2 craftingRecipePos = new Vector2(0, -i * craftingRecipeDistance + 135f);
            GameObject CraftingRecipeGameObjectCopy = Instantiate(CraftingRecipeGameObject, craftingRecipePos, Quaternion.identity);
            CraftingRecipeGameObjectCopy.transform.SetParent(CraftingMenuGameObject.transform, false);
            CraftingRecipeSlot CraftingRecipeSlotScript = CraftingRecipeGameObjectCopy.GetComponent<CraftingRecipeSlot>();
            CraftingRecipeSlotScript.playerGameObject = gameObject;
            CraftingRecipeSlotScript.index = i;

            List<int> itemIndexList = new List<int>(CraftingRecipeList[i].Keys);
            itemIndexList.Sort();

            for (int j = 0; j < itemIndexList.Count; j++) {
                Vector2 itemIconPos = new Vector2(j * itemIconDistance - 75f, 0);
                GameObject itemIcon = Instantiate(ItemControlScript.itemIconList[itemIndexList[j]], itemIconPos, Quaternion.identity);
                itemIcon.GetComponent<RectTransform>().sizeDelta = itemIconSize;
                itemIcon.GetComponentInChildren<Text>().text = CraftingRecipeList[i][itemIndexList[j]].ToString();
                if (CraftingRecipeList[i][itemIndexList[j]] == 0 || CraftingRecipeList[i][itemIndexList[j]] == 1) {
                    itemIcon.GetComponentInChildren<Text>().text = "";
                }
                itemIcon.transform.SetParent(CraftingRecipeGameObjectCopy.transform, false);
            }
            CraftingRecipeGameObjectList.Add(CraftingRecipeGameObjectCopy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCraftingRecipes() {

        for (int i = 0; i < CraftingRecipeList.Count; i++) {
            bool craftable = true;
            foreach (int itemID in CraftingRecipeList[i].Keys) {
                if (!InventoryScript.ItemInInventory(itemID, CraftingRecipeList[i][itemID])) {
                    craftable = false;
                }
            }
            CraftingRecipeGameObjectList[i].GetComponent<CraftingRecipeSlot>().craftable = craftable;
            if (craftable) {
                CraftingRecipeGameObjectList[i].GetComponent<Image>().color = Color.white;
            }
            else {
                CraftingRecipeGameObjectList[i].GetComponent<Image>().color = Color.black;

            }

        }

    }

    public void craftItem(int craftingSlotIndex) {
        Debug.Log("L");
        foreach (int itemID in CraftingRecipeList[craftingSlotIndex].Keys) {
            InventoryScript.RemoveItem(itemID, CraftingRecipeList[craftingSlotIndex][itemID]);
        }
        InventoryScript.AddItem(CraftingResultList[craftingSlotIndex], CraftingResultAmountList[craftingSlotIndex]);
        InventoryScript.UpdateInventoryUI();
        updateCraftingRecipes();
    }
}
