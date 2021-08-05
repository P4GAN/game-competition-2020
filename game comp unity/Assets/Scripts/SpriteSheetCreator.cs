using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetCreator : MonoBehaviour
{

    public static Texture2D spriteSheet;
    public static Material spriteSheetMaterial;
    public Material x;
    public static Dictionary<string, Rect> blockUV = new Dictionary<string, Rect>();
    public static List<string> blockNames = new List<string> {
        "stone",
        "coal ore",
        "aluminum ore",
        "iron ore",
        "titanium ore",
        "copper ore",
        "magnesium ore",
        "cobalt ore",
        "uranium ore",
        "galacium ore",
        "armor block"
        /*
        
        "assembler",
        "refiner",
        "storage crate",
        "basic power cell",
        "basic solar array",
        "oxygen refiner",
        "cockpit",
        "basic ion thruster",
        "light armor block",
        "light armor block half",
        */
    };
    public Texture2D[] blockTextures = new Texture2D[blockNames.Count];
    public Shader shader;


    // Start is called before the first frame update
    void Awake()
    {
        CreateSpriteSheet();
    }
    
    public void CreateSpriteSheet() {
        spriteSheet = new Texture2D(2048, 2048);
        spriteSheet.filterMode = FilterMode.Point;
        spriteSheetMaterial = new Material(shader);
        x = spriteSheetMaterial;

        Rect[] uvs = spriteSheet.PackTextures(blockTextures, 0, 2048);
        for (int i = 0; i < uvs.Length; i++) {
            blockUV.Add(blockNames[i], uvs[i]);
            Debug.Log("S");
        }

        spriteSheetMaterial.mainTexture = spriteSheet;
    }

}
