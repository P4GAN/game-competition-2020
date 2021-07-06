using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> thrusters;
    public List<GameObject> rotationWheels;
    public List<float> thrusterForces; //0, 1, 2, 3 represent 0, 90, 180, 270 rotation
    public float rotateForce;
    public Rigidbody2D rb2d;
    public bool shipCreated = false;
    public bool shipSelected = false;
    public GameObject cockpit;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (SpacePlayerController.playerInShip && shipSelected) {
            WorldBuilder.player.transform.localPosition = cockpit.transform.localPosition;
            WorldBuilder.player.transform.localRotation = Quaternion.identity;
            if (Input.GetKey(KeyCode.W)) {
                rb2d.AddForce(transform.up * thrusterForces[0]);
            }
            if (Input.GetKey(KeyCode.A)) {
                rb2d.AddForce(-transform.right * thrusterForces[1]);
            }
            if (Input.GetKey(KeyCode.S)) {
                rb2d.AddForce(-transform.up * thrusterForces[2]);
            }
            if (Input.GetKey(KeyCode.D)) {
                rb2d.AddForce(transform.right * thrusterForces[3]);
            }

            if (Input.GetKey(KeyCode.Q)) {
                rb2d.AddTorque(rotateForce);
            }
            if (Input.GetKey(KeyCode.E)) {
                rb2d.AddTorque(-rotateForce);
            }

        }
    }

    public void CreateShip(GameObject cockpitGameObject) {
        shipCreated = true;
        cockpit = cockpitGameObject;

        GetComponent<AsteroidBlockControl>().isShip = true;

//ship selected
        shipSelected = true;
        SpacePlayerController.playerInShip = true;
        WorldBuilder.player.transform.SetParent(transform);
        WorldBuilder.player.GetComponent<BoxCollider2D>().enabled = false;
        WorldBuilder.player.transform.localPosition = cockpit.transform.localPosition + new Vector3(0, 1, 0);
        WorldBuilder.player.transform.localRotation = Quaternion.identity;
    }
    public void AddShipBlock(GameObject block) {
        Item item = block.GetComponent<ItemData>().item;
        if (item.itemID == "basic ion thruster") {
            thrusters.Add(block);
            Debug.Log(block.transform.eulerAngles);
            Debug.Log(block.transform.eulerAngles.z);
            Debug.Log(block.transform.eulerAngles.z/90);
            Debug.Log(Mathf.Round(block.transform.eulerAngles.z/90));
            Debug.Log((int)Mathf.Round(block.transform.eulerAngles.z/90) % 4);
            thrusterForces[(int)Mathf.Round(block.transform.eulerAngles.z/90) % 4] += 3000;
        }

    }

    public void RemoveShipBlock(GameObject block) {
        Item item = block.GetComponent<ItemData>().item;
        if (item.itemID == "basic ion thruster") {
            thrusters.Remove(block);
            thrusterForces[(int)Mathf.Round(block.transform.eulerAngles.z/90) % 4] += 3000;
        }
    }

}
