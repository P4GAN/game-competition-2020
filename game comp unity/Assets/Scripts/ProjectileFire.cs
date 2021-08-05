using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    public static void MouseFireProjectile(GameObject projectile, GameObject startObject, float projectileSpeed = 5000f) {
        Vector3 mousePos = Input.mousePosition;
        float angle = Mathf.Atan2((mousePos.y - Screen.height/2), (mousePos.x - Screen.width/2)) * Mathf.Rad2Deg ;
        Vector2 direction = (mousePos - new Vector3(Screen.width/2, Screen.height/2, 0)).normalized;
        GameObject projectileInstance = Instantiate(projectile, startObject.transform.position, Quaternion.Euler(0, 0, angle));
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        projectileInstance.GetComponent<ProjectileControl>().player = startObject;
        //startObject.GetComponent<Rigidbody2D>().AddForce(-direction * projectileSpeed);
    }

    public static void DirectionFireProjectile(GameObject projectile, Vector2 direction, GameObject startObject, float projectileSpeed = 5000f) {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject projectileInstance = Instantiate(projectile, startObject.transform.position, Quaternion.Euler(0, 0, angle));
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        //startObject.GetComponent<Rigidbody2D>().AddForce(-direction * projectileSpeed);
    }

}
