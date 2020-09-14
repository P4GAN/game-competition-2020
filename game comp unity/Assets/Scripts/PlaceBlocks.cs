using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlocks : MonoBehaviour
{

    public GameObject block;
    public GameObject blockInstance;
    public GameObject player;
    public Vector2 mousePos;

    public Vector2 blockPos;
    public List<GameObject> inventory;
    public int inventoryIndex;

    public Dictionary<Vector2, GameObject> blocks;

    public GameObject startingBlock;
    public float timer = 0;
    public float breakBlockTime = 2.5f;
    public float playerReach = 5f;
    public System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        var startingBlockPos = new Vector2(startingBlock.transform.position.x, startingBlock.transform.position.y);
        blocks = new Dictionary<Vector2, GameObject>{
            [startingBlockPos] = startingBlock
        };

        for (int x=-20; x<20; x++) {
            for (int y=0; y>-10; y--) {
                blockPos = new Vector2 (x - 0.5f, y - 0.5f);
                int index = random.Next(0, inventory.Count);
                blockInstance = Instantiate(inventory[index], blockPos, Quaternion.identity);
                blocks.Add(blockPos, blockInstance);
            }
        }

    }

    // Update is called once per frame
    void Update()
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
                    Destroy(blocks[blockPos]);
                    blocks.Remove(blockPos);
                //}
            }
            
        }


        if (Input.GetMouseButtonDown(1)) {

        
            if (!blocks.ContainsKey(blockPos)) {
                blockInstance = Instantiate(inventory[inventoryIndex], blockPos, Quaternion.identity);
                blocks.Add(blockPos, blockInstance);    
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            inventoryIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            inventoryIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            inventoryIndex = 2;
        }

        /*if (Input.GetMouseButtonDown(1)) {

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
        }*/
        
    }
}
