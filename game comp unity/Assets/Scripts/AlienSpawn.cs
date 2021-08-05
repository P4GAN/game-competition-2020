using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlienSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public float spawnTimer;
    public float spawnCooldown = 5f;
    public GameObject warningText;
    public List<GameObject> enemyList;

    void Start()
    {
        warningText = GameObject.Find("Canvas").transform.Find("Warning Text").gameObject;
        warningText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyList.Count > 0) {
            warningText.SetActive(true);
        }
        else {
            warningText.SetActive(false);
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnCooldown) {
            spawnTimer = 0f;
            int numEnemies = Random.Range(1, 5);
            for (int i = 0; i < numEnemies; i ++) {
                float randomAngle = Random.Range(0f, 2 * Mathf.PI);
                Vector2 position = (Vector2)transform.position + new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * 24f;
                if (!Physics2D.OverlapBox(position, new Vector2(1, 1), 0)) {
                    GameObject enemyInstance = Instantiate(enemy, position, Quaternion.identity);
                    enemyInstance.GetComponent<Enemy>().player = gameObject;
                    enemyList.Add(enemyInstance);
                }
            }
        }
    }
}
