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
        "armor block"
        /*
        ,"galacium ore",
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


    // Start is called before the first frame update
    void Awake()
    {
        CreateSpriteSheet();
    }
    
    public void CreateSpriteSheet() {
        spriteSheet = new Texture2D(2048, 2048);
        spriteSheetMaterial = new Material(Shader.Find("Unlit/Transparent"));
        x = spriteSheetMaterial;

        for (int i = 0; i < blockNames.Count; i++) {
            Texture2D blockTexture = (Texture2D)Resources.Load("ItemSprites/" + blockNames[i]);
            blockTextures[i] = blockTexture;
        }
        Rect[] uvs = spriteSheet.PackTextures(blockTextures, 0, 2048);
        for (int i = 0; i < uvs.Length; i++) {
            blockUV.Add(blockNames[i], uvs[i]);
        }

        spriteSheetMaterial.mainTexture = spriteSheet;
    }

}
