using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> thrustersUp;
    public List<GameObject> thrustersDown;
    public List<GameObject> thrustersLeft;
    public List<GameObject> thrustersRight;
    public List<GameObject> torqueWheels;
    public float forceUp;
    public float forceDown;
    public float forceLeft;
    public float forceRight;
    public float rotateForce;
    public Rigidbody2D rb2d;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpacePlayerController.inShip) {
            if (Input.GetKey(KeyCode.W)) {
                rb2d.AddForce(transform.up * forceUp);
            }
            if (Input.GetKey(KeyCode.A)) {
                rb2d.AddForce(-transform.right * forceLeft);
            }
            if (Input.GetKey(KeyCode.S)) {
                rb2d.AddForce(-transform.up * forceDown);
            }
            if (Input.GetKey(KeyCode.D)) {
                rb2d.AddForce(transform.right * forceRight);
            }

            if (Input.GetKey(KeyCode.Q)) {
                rb2d.AddTorque(rotateForce);
            }
            if (Input.GetKey(KeyCode.E)) {
                rb2d.AddTorque(-rotateForce);
            }

        }
    }
}
