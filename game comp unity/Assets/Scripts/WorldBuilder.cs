using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class BlockObject {
    public float gridX;
    public float gridY;
    public int blockID;
}

[Serializable]
public class AsteroidObject {
    public float x;
    public float y;
    public float angle;
    public List<BlockObject> blockList;

}


public class WorldObject {
    public List<AsteroidObject> asteroidList;
}



public class WorldBuilder : MonoBehaviour
{

    void SaveWorld(List<GameObject> AsteroidList) {
        foreach (GameObject asteroid in AsteroidList) {

        }
    }


    void BuildWorld(string fileName) {

    }

    void GenerateWorld() {

    }
}
