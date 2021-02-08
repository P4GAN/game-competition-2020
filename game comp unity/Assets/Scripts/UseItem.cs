using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{

    public GameObject itemControlGameObject;

    public Vector2 mousePos;
    
    public ItemControl ItemControlScript;

    public Inventory InventoryScript;
    // Start is called before the first frame update
    void Start()
    {
        InventoryScript = GetComponent<Inventory>();
        ItemControlScript = itemControlGameObject.GetComponent<ItemControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            UseItemLeftClickHold();
        }
        if (Input.GetMouseButtonDown(0)) {
            UseItemLeftClickDown();
        }
        if (Input.GetMouseButtonUp(0)) {
            UseItemLeftClickUp();
        }
        if (Input.GetMouseButton(1)) {
            UseItemRightClickHold();
        }
        if (Input.GetMouseButtonDown(1)) {
            UseItemRightClickDown();
        }
        if (Input.GetMouseButtonUp(1)) {
            UseItemRightClickUp();
        }
    }


    public void UseItemLeftClickDown() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        string itemType = ItemControlScript.itemList[InventoryScript.InventoryItems[inventoryIndex]].GetComponent<ItemData>().itemType;  

        if (itemType == "projectileGun") {
            ProjectileFire projectileFireScript = GetComponent<ProjectileFire>(); 
            int bulletIndex = InventoryScript.InventoryItems.IndexOf(16);
            if (bulletIndex != -1 && InventoryScript.InventoryAmounts[bulletIndex] > 0) {
                projectileFireScript.FireProjectile(ItemControlScript.itemList[16]);
                InventoryScript.RemoveItemAtIndex(bulletIndex, 1);
            }
        }

        if (itemType == "laserGun") {
            GetComponent<LaserFire>().StartLaser();
        }
        

        /*
        code to break block
        */

        if (itemType == "") {
            AsteroidBlockControl AsteroidBlockControlScript = ItemControlScript.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            GameObject block = AsteroidBlockControlScript.RemoveBlock(mousePos);
            if (block) {
                InventoryScript.AddItem(block.GetComponent<ItemData>().itemID, 1);
            }

        }

    }

    public void UseItemLeftClickHold() {
        GetComponent<LaserFire>().ControlLaser();

    }
    public void UseItemLeftClickUp() {
        GetComponent<LaserFire>().DestroyLaser();
    }

    public void UseItemRightClickDown() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        string itemType = ItemControlScript.itemList[InventoryScript.InventoryItems[inventoryIndex]].GetComponent<ItemData>().itemType;  

        if (itemType == "block") {
            AsteroidBlockControl AsteroidBlockControlScript = ItemControlScript.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            if (InventoryScript.InventoryAmounts[inventoryIndex] >= 1) {
                if (AsteroidBlockControlScript.PlaceBlock(InventoryScript.InventoryItems[inventoryIndex], mousePos, true)) {
                    InventoryScript.RemoveItemAtIndex(inventoryIndex, 1);
                }

            }

        }
    }
    public void UseItemRightClickHold() {
        GetComponent<LaserFire>().ControlLaser();

    }
    public void UseItemRight ClickUp() {
        GetComponent<LaserFire>().DestroyLaser();
    }
}
