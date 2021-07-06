using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour {

    public GameObject laser;
    public GameObject laserInstance;
    public Vector2 mousePos;
    public float maxRange;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame

    public void LaserFunction() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x)) * Mathf.Rad2Deg;
        
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        Debug.Log(direction);
        int layerMask =~ LayerMask.GetMask("Player");
        Vector2 maxPosition = (Vector2)transform.position + (direction * maxRange);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, maxPosition, layerMask);

        Vector2 position = new Vector2();

        if (hit.collider != null) {
            position = new Vector2((transform.position.x + hit.point.x), (transform.position.y + hit.point.y))/2;
        }
        else {
            Debug.Log(maxPosition);
            position = (maxPosition + (Vector2)transform.position)/2;
        }

        laserInstance.transform.position = position; 
        laserInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
        laserInstance.transform.localScale = new Vector2(Vector2.Distance(transform.position, position) * 2, 0.2f);
    }

	public void StartLaser () {

        if (laserInstance == null) {
            laserInstance = Instantiate(laser, transform.position, Quaternion.identity);
            LaserFunction();
        }
    }
    public void ControlLaser () {

        if (laserInstance != null) {
            LaserFunction();
        }
    }
    public void DestroyLaser() {
        if (laserInstance != null) {
            Destroy(laserInstance);
        }
        
	}

}
