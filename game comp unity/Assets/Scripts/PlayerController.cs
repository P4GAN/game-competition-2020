using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public GameObject groundCheck;
    public float movementSpeed = 3f;
    public float jumpForce = 10f;
    public bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = groundCheck.GetComponent<GroundCheck>().grounded;

        var movement = Input.GetAxis("Horizontal") * movementSpeed;
        rb2d.velocity = new Vector2(movement, rb2d.velocity.y);

        if (Input.GetKey(KeyCode.W) && grounded ) {
            rb2d.AddForce(new Vector2(0f, jumpForce));

        }
    }
}
