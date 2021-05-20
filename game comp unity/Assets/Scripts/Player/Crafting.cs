using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CraftingRecipe {
    public int CraftingResult;
    public string CraftingResultName;
    public int CraftingResultAmount;
    public List<int> CraftingIngredients;
    public List<string> CraftingIngredientNames;
    public List<int> CraftingIngredientAmounts;
}

[Serializable]
public class CraftingRecipeObjects
{
    public List<CraftingRecipe> CraftingRecipeList;
}


public class Crafting : MonoBehaviour
{
    public string CraftingMenuGameObjectName;
    public GameObject CraftingRecipeGameObject;
    public int craftingRecipeDistance = 25;
    public int itemIconDistance = 20;
    public Vector2 itemIconSize;

    public List<GameObject> CraftingRecipeGameObjectList;

    public Inventory InventoryScript;

    public List<CraftingRecipe> CraftingRecipeList;

    public GameObject emptyItem;

    public string fileName;


    // Start is called before the first frame update
    void Start()
    {
        
        string json = File.ReadAllText(fileName);
        CraftingRecipeList = JsonUtility.FromJson<CraftingRecipeObjects>(json).CraftingRecipeList;

        //turns item names into item ids, delete for actual game, keep now to edit crafting recipe json easily using names
        CraftingRecipeObjects playerRecipes = new CraftingRecipeObjects();
        playerRecipes.CraftingRecipeList = CraftingRecipeList;

        for (int i = 0; i < CraftingRecipeList.Count; i++) {
            playerRecipes.CraftingRecipeList[i].CraftingResult = ItemControl.itemList.FindIndex(x => x.GetComponent<ItemData>().item.itemName == playerRecipes.CraftingRecipeList[i].CraftingResultName);
            playerRecipes.CraftingRecipeList[i].CraftingIngredients = new List<int>();
            for (int j = 0; j < playerRecipes.CraftingRecipeList[i].CraftingIngredientNames.Count; j++) {
                playerRecipes.CraftingRecipeList[i].CraftingIngredients.Add(ItemControl.itemList.FindIndex(x => x.GetComponent<ItemData>().item.itemName == playerRecipes.CraftingRecipeList[i].CraftingIngredientNames[j]));
            }
        }
        json = JsonUtility.ToJson(playerRecipes, true);
        File.WriteAllText(fileName, json);

        json = File.ReadAllText(fileName);

        CraftingRecipeList = JsonUtility.FromJson<CraftingRecipeObjects>(json).CraftingRecipeList;

        InventoryScript = GetComponent<Inventory>();
        for (int i = 0; i < CraftingRecipeList.Count; i++) {
            Vector2 craftingRecipePos = new Vector2(0, -i * craftingRecipeDistance + 475f);
            GameObject CraftingRecipeGameObjectCopy = Instantiate(CraftingRecipeGameObject, craftingRecipePos, Quaternion.identity);
            CraftingRecipeGameObjectCopy.transform.SetParent(GameObject.Find("Canvas").transform.Find("CraftingPlayerContainer").transform.Find("CraftingPlayer").transform.Find("Content"), false);
            CraftingRecipeSlot CraftingRecipeSlotScript = CraftingRecipeGameObjectCopy.GetComponent<CraftingRecipeSlot>();
            CraftingRecipeSlotScript.playerGameObject = gameObject;
            CraftingRecipeSlotScript.index = i;

            List<int> itemIndexList = new List<int>(CraftingRecipeList[i].CraftingIngredients);
            itemIndexList.Sort();
            for (int j = 0; j < itemIndexList.Count; j++) {
                Vector2 itemIconPos = new Vector2(j * itemIconDistance - 75f, 0);
                GameObject itemIcon = Instantiate(emptyItem, itemIconPos, Quaternion.identity);
                itemIcon.GetComponent<Image>().sprite = ItemControl.itemList[itemIndexList[j]].GetComponent<SpriteRenderer>().sprite;
                itemIcon.GetComponent<RectTransform>().sizeDelta = itemIconSize;
                itemIcon.GetComponentInChildren<Text>().text = CraftingRecipeList[i].CraftingIngredientAmounts[j].ToString();
                if (CraftingRecipeList[i].CraftingIngredientAmounts[j] == 0 || CraftingRecipeList[i].CraftingIngredientAmounts[j] == 1) {
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
            for (int j = 0; j < CraftingRecipeList[i].CraftingIngredients.Count; j++) {
                if (!InventoryScript.ItemInInventory(CraftingRecipeList[i].CraftingIngredients[j], CraftingRecipeList[i].CraftingIngredientAmounts[j])) {
                    craftable = false;
                }
            }
            CraftingRecipeGameObjectList[i].GetComponent<CraftingRecipeSlot>().craftable = craftable;
            if (craftable) {
                CraftingRecipeGameObjectList[i].GetComponent<Image>().color = Color.white;
            }
            else {
                CraftingRecipeGameObjectList[i].GetComponent<Image>().color = Color.grey;
            }

        }

    }

    public void craftItem(int craftingSlotIndex) {

        for (int i = 0; i < CraftingRecipeList[craftingSlotIndex].CraftingIngredients.Count; i++) {
            InventoryScript.RemoveItem(CraftingRecipeList[craftingSlotIndex].CraftingIngredients[i], CraftingRecipeList[craftingSlotIndex].CraftingIngredientAmounts[i]);
        }

        InventoryScript.AddItem(CraftingRecipeList[craftingSlotIndex].CraftingResult, CraftingRecipeList[craftingSlotIndex].CraftingResultAmount);
        InventoryScript.UpdateInventoryUI();
        updateCraftingRecipes();
    }
}
