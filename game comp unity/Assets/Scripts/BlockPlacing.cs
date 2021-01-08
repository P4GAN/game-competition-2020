using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacing : MonoBehaviour
{


    public Dictionary<Vector2, GameObject> blocks = new Dictionary<Vector2, GameObject>{
        
    };
    public GameObject blockInstance;

    public List<GameObject> itemList;
    public Vector2 mousePos;
    public int inventoryIndex = 1;

    

    // Start is called before the first frame update


    // Update is called once per frame


    public GameObject PlaceBlock(int itemID, Vector2 position ) {
        position = new Vector2 (Mathf.Floor(position.x + 0.5f) , Mathf.Floor(position.y + 0.5f));
        if (!blocks.ContainsKey(position)) {
            blockInstance = Instantiate(blockInstance, position, Quaternion.identity);
            blocks.Add(position, blockInstance);    
            return blockInstance;
        }
        return null;
    }
    public void RemoveBlock(Vector2 position ) {
        position = new Vector2 (Mathf.Floor(position.x + 0.5f) , Mathf.Floor(position.y + 0.5f));
        if (blocks.ContainsKey(position)) {
            Destroy(blocks[position]);
            blocks.Remove(position);
        }
    }

    void Start() {


        for (int x=-20; x<20; x++) {
            Vector2 blockPos = new Vector2 (x, 0);
            PlaceBlock(0, blockPos);
        }
        for (int x=-20; x<20; x++) {
            for (int y=-1; y>-10; y--) {
                Vector2 blockPos = new Vector2 (x, y);
                int index = Random.Range(1, 3);
                PlaceBlock(index, blockPos);
            }
        }



    }

    void Update()
    {

        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 blockPos = mousePos;
        if (Input.GetMouseButtonDown(0)) {
            //timer += Time.deltaTime;
            //Debug.Log(timer);
            //if (blocks.ContainsKey(blockPos) && timer > breakBlockTime) {
                //Vector2 distance = player.transform.position - blocks[blockPos].transform.position;
                //Debug.Log(distance.magnitude);

                //if (distance.magnitude < playerReach) {
                    //timer = 0;
                    //InventoryScript.RemoveItem(inventoryIndex, 1);
                    RemoveBlock(mousePos);
                //}
            
        }


        if (Input.GetMouseButtonDown(1)) {

        
            PlaceBlock(2, mousePos);
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            inventoryIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            inventoryIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            inventoryIndex = 3;
        }
        /*
        if (Input.GetMouseButtonDown(1)) {

            else {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                blockPos = new Vector2 (Mathf.Ceil(mousePos.x) - 0.5f, Mathf.Ceil(mousePos.y) - 0.5f);
                if (Time.time - mouseStart > breakBlockTime) {
                    if (blocks.ContainsKey(blockPos)) {
                        Destroy(blocks[blockPos]);
                        blocks.Remove(blockPos);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            mouseDown = false;
            if (Time.time - mouseStart < 0.25f) {
                if (!blocks.ContainsKey(blockPos)) {
                    blockInstance = Instantiate(block, blockPos, Quaternion.identity);
                    blocks.Add(blockPos, blockInstance);
                }
            }
        }
        */
    }
}
    