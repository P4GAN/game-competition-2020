using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ItemControl : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> setItemlist;
    public static GameObject selectedAsteroid;
    public static AsteroidBlockControl AsteroidBlockControlScript;
    public GameObject itemIcon;
    public GameObject droppedItem;

    public static ProjectileFire projectileFireScript;
    public static Dictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();
    public static List<string> itemNameList = new List<string>() {
        "empty",
        "stone",
        "coal ore",
        "aluminum ore",
        "iron ore",
        "titanium ore",
        "copper ore",
        "magnesium ore",
        "cobalt ore",
        "uranium ore",
        "galacium ore",
        "coal",
        "aluminum",
        "iron",
        "titanium",
        "copper",
        "magnesium",
        "cobalt",
        "uranium",
        "silicon",
        "ceramic",
        "galacium",
        "steel",
        "copper wire",
        "explosive",
        "basic battery",
        "advanced battery",
        "electromagnet",
        "computer",
        "graphene",
        "silicone",
        "riftsteel",
        "bullet",
        "explosive bullet",
        "railgun projectile",
        "missile",
        "pickaxe",
        "drill",
        "handheld rifle",
        "handheld laser rifle",
        "handheld railgun",
        "handheld missile launcher",
        "basic oxygen tank",
        "basic jetpack",
        "basic power pack",
        "basic solar pack",
        "steel armor",
        "titanium armor",
        "graphene armor",
        "riftsteel armor",
        "assembler",
        "refiner",
        "storage crate",
        "basic power cell",
        "basic solar array",
        "oxygen refiner",
        "cockpit",
        "basic ion thruster",
        "light armor block",
        "light armor block half",
    };
    void Awake()
    {
        for (int i = 0; i < itemNameList.Count; i++) {
            GameObject x = (GameObject)Resources.Load("ItemGameObjects/" + itemNameList[i]);
            x.GetComponent<ItemData>().item.itemAmount = 0;
            x.GetComponent<ItemData>().item.itemID = x.GetComponent<ItemData>().item.itemName;
            itemDictionary.Add(itemNameList[i], (GameObject)Resources.Load("ItemGameObjects/" + itemNameList[i]));
        }
        /*for (int i = 0; i < itemList.Count; i++) {
            GameObject itemIconInstance = Instantiate(itemIcon);
            itemIconInstance.GetComponent<Image>().sprite = itemList[i].GetComponent<SpriteRenderer>().sprite;
            ItemData itemIconScript = itemList[i].GetComponent<ItemData>();
            ItemData itemIconDataScript = itemIconInstance.GetComponent<ItemData>();

            itemIconDataScript.itemID = itemIconScript.itemID;
            itemIconDataScript.itemType = itemIconScript.itemType;
            itemIconDataScript.itemCaptionString = itemIconScript.itemCaptionString;
            itemIconDataScript.mass = itemIconScript.mass;

        }*/
        projectileFireScript = GetComponent<ProjectileFire>();
    }

    // Update is called once per frame
    void Update()
    {
    }

}
