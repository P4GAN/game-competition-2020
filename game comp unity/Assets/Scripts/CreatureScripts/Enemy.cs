using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public float health = 100f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TakeDamage(float damage) { // returns if the damage killed enemy or not
        health -= damage;
        if (health < 0) {
            Destroy(gameObject);
            return true;
        }
        return false;
    }


}
