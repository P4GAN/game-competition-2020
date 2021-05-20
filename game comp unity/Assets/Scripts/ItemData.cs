using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item {
    public int itemID;
    public string itemName;
    public int itemAmount;
    public string itemType; // block, weapon, tool etc affects item usage
    [TextArea]
    public string itemCaptionString;
    public float mass;
    public Item Clone() {
        string json = JsonUtility.ToJson(this);
        return JsonUtility.FromJson<Item>(json);
    
    }
}
public class ItemData : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Item item;
    

}
