using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{

    public Vector2 mousePos;
    
    public Inventory InventoryScript;

    public PlayerInventory PlayerInventoryScript;
    public GameObject indicatorBlockPrefab;
    public static GameObject indicatorBlock;
    public SpriteRenderer spriteRenderer;
    public int rotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        InventoryScript = GetComponent<Inventory>();
        PlayerInventoryScript = GetComponent<PlayerInventory>();
        indicatorBlock = Instantiate(indicatorBlockPrefab, transform.position, Quaternion.identity);
        indicatorBlock.SetActive(false);
        spriteRenderer = indicatorBlock.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemType == "block" && 
            ItemControl.selectedAsteroid != null &&
            ItemControl.AsteroidBlockControlScript.IsOccupied(Camera.main.ScreenToWorldPoint(Input.mousePosition)) == false &&
            ItemControl.AsteroidBlockControlScript.isAdjacent(Camera.main.ScreenToWorldPoint(Input.mousePosition)) == true) {
                if (!indicatorBlock.activeSelf) {
                    indicatorBlock.SetActive(true);
                }

                indicatorBlock.transform.position = ItemControl.AsteroidBlockControlScript.gridPositionToGamePosition(ItemControl.AsteroidBlockControlScript.gamePositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                indicatorBlock.transform.rotation = Quaternion.Euler(0, 0, rotation + ItemControl.selectedAsteroid.transform.eulerAngles.z);
                spriteRenderer.sprite = ItemControl.itemDictionary[InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemID].GetComponent<SpriteRenderer>().sprite; 
            
            
        }
        else {
            if (indicatorBlock.activeSelf) {
                indicatorBlock.SetActive(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.R)) {
            rotation += 90;
            rotation = rotation % 360;
        }

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

        if (ItemControl.AsteroidBlockControlScript.IsOccupied(mousePos)) {
            if (ItemControl.AsteroidBlockControlScript.asteroidBlocks[ItemControl.AsteroidBlockControlScript.gamePositionToGridPosition(mousePos)].GetComponent<BlockClick>() != null) {
                ItemControl.AsteroidBlockControlScript.asteroidBlocks[ItemControl.AsteroidBlockControlScript.gamePositionToGridPosition(mousePos)].GetComponent<BlockClick>().BlockClicked();
            }
        }


        string itemType = InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemType;  

        Debug.Log(itemType);
        Debug.Log(InventoryScript.InventoryItems[2].itemType);

        if (itemType == "projectileGun") {
            ProjectileFire projectileFireScript = ItemControl.projectileFireScript; 
            int bulletIndex = InventoryScript.InventoryItems.FindIndex(x => x.itemName == "bullet");
            if (bulletIndex != -1 && InventoryScript.InventoryItems[bulletIndex].itemAmount > 0) {
                projectileFireScript.MouseFireProjectile(ItemControl.itemDictionary["bullet"], gameObject);
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
            
            GameObject block = ItemControl.AsteroidBlockControlScript.RemoveBlock(mousePos);
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
            if (InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemAmount >= 1) {
                GameObject placedBlock = ItemControl.AsteroidBlockControlScript.PlaceBlockPlayer(InventoryScript.InventoryItems[PlayerInventoryScript.inventoryIndex].itemID, mousePos, true, rotation);
                if (placedBlock != null) {
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
