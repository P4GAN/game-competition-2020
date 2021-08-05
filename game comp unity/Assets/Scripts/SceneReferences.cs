using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneReferences : MonoBehaviour
{
    public static List<GameObject> hotbarSlots;
    public static Slider healthSlider;
    public static Slider powerSlider;
    public static GameObject canvas;
    public static GameObject controlGameObject;

    public static GameObject CraftingMenuRefiner;
    public static GameObject CraftingMenuAssembler;
    public static GameObject CraftingMenuPlayer;
    public static GameObject HotbarIndicator;
    public static GameObject CurrentHeldGameObject;
    public static GameObject itemCaption;

    //set references

    public List<GameObject> setHotbarSlots;
    public Slider setHealthSlider;
    public Slider setPowerSlider;
    public GameObject setCanvas;
    public GameObject setControlGameObject;

    public GameObject setCraftingMenuRefiner;
    public GameObject setCraftingMenuAssembler;
    public GameObject setCraftingMenuPlayer;
    public GameObject setHotbarIndicator;
    public GameObject setCurrentHeldGameObject;
    public GameObject setItemCaption;

    public void Awake() {

        Debug.Log("SDKLNJKNS");

        hotbarSlots = setHotbarSlots;
        healthSlider = setHealthSlider;
        powerSlider = setPowerSlider;
        canvas = setCanvas;
        controlGameObject = setControlGameObject;
        CraftingMenuRefiner = setCraftingMenuRefiner;
        CraftingMenuAssembler = setCraftingMenuAssembler;
        CraftingMenuPlayer = setCraftingMenuPlayer;
        HotbarIndicator = setHotbarIndicator;
        CurrentHeldGameObject = setCurrentHeldGameObject;
        itemCaption = setItemCaption;

    }
}
