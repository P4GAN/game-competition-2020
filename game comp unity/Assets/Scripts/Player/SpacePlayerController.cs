using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpacePlayerController : MonoBehaviour
{
    public Camera camera;
    public Slider healthSlider;
    public Rigidbody2D rb2d;
    public float health = 100f;
    public float maxHealth = 100f;
    public float moveForce = 1f;
    public float rotateForce = 1f;
    public static bool playerInShip = false;
    public Vector2 y = new Vector2(0, 1);
    public Vector2 x = new Vector2(1, 0);
    public GameObject laserProjectile;
    public List<TextMeshProUGUI> resourceTextList;
    public Dictionary<string, int> resources = new Dictionary<string, int>() {
        ["cobalt ore"] = 0,
        ["aluminum ore"] = 0,
        ["titanium ore"] = 0,
        ["uranium ore"] = 0,
        ["galacium ore"] = 0,
    };
    public ParticleSystem particleSystem;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        rb2d = GetComponent<Rigidbody2D>();
        healthSlider = canvas.GetComponentInChildren<Slider>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        for (int i = 0; i < resourceTextList.Count; i++) {
            resourceTextList[i] = canvas.transform.Find("Resource" + i.ToString()).gameObject.GetComponent<TextMeshProUGUI>();
            Debug.Log(canvas.transform.Find("Resource" + i.ToString()).gameObject);
        }
        pauseMenu = GameObject.Find("Canvas").transform.Find("Pause Menu").gameObject;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (MenuButtons.paused) {
                MenuButtons.Continue();
            }
            else {
                MenuButtons.Pause();
            }
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        if (!playerInShip) {
            /*if (Input.GetKey(KeyCode.W)) {
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
            }*/

            Vector3 mousePos = Input.mousePosition;
            float angle = Mathf.Atan2((mousePos.y - Screen.height/2), (mousePos.x - Screen.width/2)) * Mathf.Rad2Deg ;

            transform.rotation = Quaternion.Euler(0, 0, angle);

            var emission = particleSystem.emission;
            if (Input.GetKey(KeyCode.Space)) {
                rb2d.AddForce(transform.right * moveForce);
                emission.rateOverTime = 100;
            }
            else {
                emission.rateOverTime = 0;
            }

            if (Input.GetMouseButtonDown(0)) {
                ProjectileFire.MouseFireProjectile(laserProjectile, gameObject);
            }

            if (Input.mouseScrollDelta.y > 0) {
                camera.orthographicSize -= 0.1f;
            }
            if (Input.mouseScrollDelta.y < 0) {
                camera.orthographicSize += 0.1f;
            }

            /*if (Input.GetKey(KeyCode.Q)) {
                rb2d.AddTorque(rotateForce);
            }
            if (Input.GetKey(KeyCode.E)) {
                rb2d.AddTorque(-rotateForce);
            }

            if (Input.GetKey(KeyCode.G)) {
                Time.timeScale = 0f;
            }*/

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
    public bool TakeDamage(float damage) { // returns if the damage killed enemy or not
        health -= damage;
        if (health < 0) {
            Destroy(gameObject);
            MenuButtons.Pause();
            Destroy(MenuButtons.pauseMenu.transform.Find("Continue").gameObject);
            MenuButtons.pauseMenu.transform.Find("Paused").gameObject.GetComponent<TextMeshProUGUI>().text = "Game Over! Start a new game!";
            File.Delete(Application.persistentDataPath + "world.json");
        
            return true;
        }
        return false;
    }

    public void AddItem(string item, int amount) {
        if (resources.ContainsKey(item)) {
            resources[item] += amount;
        }
        resourceTextList[0].text = "Cobalt: " + resources["cobalt ore"].ToString() + "/100";
        resourceTextList[1].text = "Aluminum: " + resources["aluminum ore"].ToString() + "/100";
        resourceTextList[2].text = "Titanium: " + resources["titanium ore"].ToString() + "/20";
        resourceTextList[3].text = "Uranium: " + resources["uranium ore"].ToString() + "/20";
        resourceTextList[4].text = "Riftsteel: " + resources["galacium ore"].ToString() + "/5";
        if (resources["cobalt ore"] >= 100 && resources["aluminum ore"] >= 100 && resources["titanium ore"] >= 20 && resources["uranium ore"] >= 20 && resources["galacium ore"] >= 5) {
            MenuButtons.Pause();
            Destroy(MenuButtons.pauseMenu.transform.Find("Continue").gameObject);
            MenuButtons.pauseMenu.transform.Find("Paused").gameObject.GetComponent<TextMeshProUGUI>().text = "Congratulations! You built the engine and escaped the explosion!";
            File.Delete(Application.persistentDataPath + "world.json");

        }
    }
}
