using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{


    public Dictionary<Vector2, GameObject> blocks;
    public GameObject startingBlock;
    public GameObject blockInstance;
    public System.Random random = new System.Random();

    public List<GameObject> blockList;
    public Vector2 mousePos;

    public Vector2 blockPos;
    

    // Start is called before the first frame update


    // Update is called once per frame


    public void PlaceBlock(int itemID, Vector2 position ) {
        position = new Vector2 (Mathf.Ceil(position.x) - 0.5f, Mathf.Ceil(position.y) - 0.5f);
        if (!blocks.ContainsKey(position)) {
            blockInstance = Instantiate(blockList[itemID], position, Quaternion.identity);
            blocks.Add(position, blockInstance);    
        }
    }
    public void RemoveBlock(Vector2 position ) {
        position = new Vector2 (Mathf.Ceil(position.x) - 0.5f, Mathf.Ceil(position.y) - 0.5f);
        if (blocks.ContainsKey(position)) {
            Destroy(blocks[position]);
            blocks.Remove(position);
        }
    }

    void Start() {

        var startingBlockPos = new Vector2(startingBlock.transform.position.x, startingBlock.transform.position.y);
        blocks = new Dictionary<Vector2, GameObject>{
            [startingBlockPos] = startingBlock
        };
        for (int x=-20; x<20; x++) {
            blockPos = new Vector2 (x - 0.5f, -0.5f);
            PlaceBlock(0, blockPos);
        }
        for (int x=-20; x<20; x++) {
            for (int y=-1; y>-10; y--) {
                blockPos = new Vector2 (x - 0.5f, y - 0.5f);
                int index = random.Next(1, 3);
                PlaceBlock(index, blockPos);
            }
        }

    }
}
    /*void Update()
    {

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blockPos = new Vector2 (Mathf.Ceil(mousePos.x) - 0.5f, Mathf.Ceil(mousePos.y) - 0.5f);
        if (Input.GetMouseButtonDown(0)) {
            //timer += Time.deltaTime;
            //Debug.Log(timer);
            //if (blocks.ContainsKey(blockPos) && timer > breakBlockTime) {
            if (blocks.ContainsKey(blockPos)) {
                //Vector2 distance = player.transform.position - blocks[blockPos].transform.position;
                //Debug.Log(distance.magnitude);

                //if (distance.magnitude < playerReach) {
                    //timer = 0;
                    inventoryScript.RemoveItem(inventoryIndex, 1);
                    Destroy(blocks[blockPos]);
                    blocks.Remove(blockPos);
                //}
            }
            
        }


        if (Input.GetMouseButtonDown(1)) {

        
            if (!blocks.ContainsKey(blockPos)) {
                inventoryScript.AddItem(inventoryIndex, 1);
                blockInstance = Instantiate(inventory[inventoryIndex], blockPos, Quaternion.identity);
                blocks.Add(blockPos, blockInstance);    
            }
            
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
        
    }
}*/
