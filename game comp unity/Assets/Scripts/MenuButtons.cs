using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject setPauseMenu;
    public static GameObject pauseMenu;
    public static bool paused = false;
    public void Awake() {
        pauseMenu = setPauseMenu;
    }
    public void Play() {
        SceneManager.LoadScene("MainGame");
    }
    public void Credits() {
        SceneManager.LoadScene("Credits");
    }
    public static void Pause() {
        WorldBuilder.SaveWorld(WorldBuilder.asteroidGameObjectList, WorldBuilder.player, WorldBuilder.seed);
        Time.timeScale = 0f;
        paused = true;
        pauseMenu.SetActive(true);
    }
    public static void Continue() {
        Time.timeScale = 1f;
        paused = false;
        pauseMenu.SetActive(false);
    }
    public void TitlePage() {
        SceneManager.LoadScene("Start");
    }    
    public void Quit() {
        Application.Quit();
    }
}
