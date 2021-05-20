﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePlayerController : MonoBehaviour
{

    public Rigidbody2D rb2d;
    public float moveForce = 1f;
    public float rotateForce = 1f;
    public static bool inShip = false;
    public static GameObject ship;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    { 
        if (!inShip) {
            if (Input.GetKey(KeyCode.W)) {
                rb2d.AddForce(transform.up * moveForce);
            }
            if (Input.GetKey(KeyCode.A)) {
                rb2d.AddForce(-transform.right * moveForce);
            }
            if (Input.GetKey(KeyCode.S)) {
                rb2d.AddForce(-transform.up * moveForce);
            }
            if (Input.GetKey(KeyCode.D)) {
                rb2d.AddForce(transform.right * moveForce);
            }

            if (Input.GetKey(KeyCode.Q)) {
                rb2d.AddTorque(rotateForce);
            }
            if (Input.GetKey(KeyCode.E)) {
                rb2d.AddTorque(-rotateForce);
            }

            if (Input.GetKey(KeyCode.G)) {
                Time.timeScale = 0f;
            }

            /*if (Input.GetKey(KeyCode.W)) {
                moveAngle = transform.eulerAngles.z + 90f;
                rb2d.AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * moveAngle), Mathf.Sin(Mathf.Deg2Rad * moveAngle)) * moveForce);
            }
            if (Input.GetKey(KeyCode.A)) {
                moveAngle = transform.eulerAngles.z + 180f;
                rb2d.AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * moveAngle), Mathf.Sin(Mathf.Deg2Rad * moveAngle)) * moveForce);
            }
            if (Input.GetKey(KeyCode.S)) {
                moveAngle = transform.eulerAngles.z + 270f;
                rb2d.AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * moveAngle), Mathf.Sin(Mathf.Deg2Rad * moveAngle)) * moveForce);
            }

            if (Input.GetKey(KeyCode.D)) {
                moveAngle = transform.eulerAngles.z ;
                rb2d.AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * moveAngle), Mathf.Sin(Mathf.Deg2Rad * moveAngle)) * moveForce);
            }*/
        }

    }
}
