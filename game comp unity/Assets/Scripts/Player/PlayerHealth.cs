using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public float health = 100f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TakeDamage(float damage) {
        health -= damage;
        if (health < 0) {
            Debug.Log("dead L");
            return true;
        }
        return false;
    }
}
