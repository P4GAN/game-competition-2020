using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour {

    public GameObject laser;
    public GameObject laserInstance;
    public Vector3 mousePos;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	public void StartLaser () {

        if (laserInstance == null) {
            laserInstance = Instantiate(laser, transform.position, Quaternion.identity);
        }
    }
    public void ControlLaser () {

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x)) * Mathf.Rad2Deg;
        
        Vector2 direction = (mousePos - transform.position).normalized;
        int layerMask =~ LayerMask.GetMask("Player");
        RaycastHit2D hit = Physics2D.Linecast(transform.position, mousePos, layerMask);

        Vector2 position = new Vector2();

        if (hit.collider != null) {
            position = new Vector2((transform.position.x + hit.point.x)/2, (transform.position.y + hit.point.y)/2);
        }
        else {
            position = new Vector2((transform.position.x + mousePos.x)/2, (transform.position.y + mousePos.y)/2);
        }

        laserInstance.transform.position = position; 
        laserInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
        laserInstance.transform.localScale = new Vector2(Vector2.Distance(transform.position, position) * 2, 1);
    
    }
    public void DestroyLaser() {
        Destroy(laserInstance);
        
	}

}
