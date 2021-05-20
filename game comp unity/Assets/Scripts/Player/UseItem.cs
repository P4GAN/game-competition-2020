using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{

    public Vector2 mousePos;
    
    public Inventory InventoryScript;

    public PlayerInventory PlayerInventoryScript;
    // Start is called before the first frame update
    void Start()
    {
        InventoryScript = GetComponent<Inventory>();
        PlayerInventoryScript = GetComponent<PlayerInventory>();
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

        string itemType = InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemType;  

        Debug.Log(itemType);
        Debug.Log(InventoryScript.InventoryItems[2].itemType);

        if (itemType == "projectileGun") {
            ProjectileFire projectileFireScript = ItemControl.projectileFireScript; 
            int bulletIndex = InventoryScript.InventoryItems.FindIndex(x => x.itemName == "bullet");
            if (bulletIndex != -1 && InventoryScript.InventoryItems[bulletIndex].itemAmount > 0) {
                projectileFireScript.MouseFireProjectile(ItemControl.itemList[32], gameObject);
                InventoryScript.RemoveItemAtIndex(bulletIndex, 1);
            }
        }

        if (itemType == "laserGun") {
            GetComponent<LaserFire>().StartLaser();
        }
        

        /*
        code to break block
        */

        if (itemType == "tool") {
            AsteroidBlockControl AsteroidBlockControlScript = ItemControl.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            
            GameObject block = AsteroidBlockControlScript.RemoveBlock(mousePos);
            Debug.Log(block);
            if (block) {
                InventoryScript.AddItem(block.GetComponent<ItemData>().item, 1);
            }

        }

    }

    public void UseItemLeftClickHold() {
        string itemType = InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemType;  

        if (itemType == "laserGun") {
            GetComponent<LaserFire>().ControlLaser();
        }

    }
    public void UseItemLeftClickUp() {
        string itemType = InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemType;  

        if (itemType == "laserGun") {

            GetComponent<LaserFire>().DestroyLaser();
        }
    }

    public void UseItemRightClickDown() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        string itemType = InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemType;  

        if (itemType == "block") {
            AsteroidBlockControl AsteroidBlockControlScript = ItemControl.selectedAsteroid.GetComponent<AsteroidBlockControl>();;
            if (InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemAmount >= 1) {
                if (AsteroidBlockControlScript.PlaceBlockPlayer(InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemID, mousePos, true)) {
                    InventoryScript.RemoveItemAtIndex(PlayerInventoryScript.inventoryIndex, 1);
                }

            }

        }
    }
    public void UseItemRightClickHold() {
        string itemType = InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemType;  


    }
    public void UseItemRightClickUp() {
        string itemType = InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemType;  


    }
}
