using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class meshtest : MonoBehaviour
{

    public Dictionary<Vector2, string> blocks = new Dictionary<Vector2, string>();
    public Dictionary<Vector2, BoxCollider2D> colliders = new Dictionary<Vector2, BoxCollider2D>();
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public float blockSize = 1;

    void Start()
    {
        meshRenderer.material = SpriteSheetCreator.spriteSheetMaterial;        
        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                if (Random.value < 0.5f) {
                    blocks[new Vector2(x, y)] = "stone";
                    
                }
            }
        }

        GenerateMesh();



    }


    void GenerateMesh() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * blocks.Count];

        int[] triangles = new int[6 * blocks.Count];

        Vector2[] uv = new Vector2[4 * blocks.Count];

        int verticesCount = 0;

        foreach (Vector2 position in blocks.Keys) {
            vertices[verticesCount] = position + new Vector2(-blockSize/2, -blockSize/2);
            vertices[verticesCount + 1] = position + new Vector2(blockSize/2, -blockSize/2);
            vertices[verticesCount + 2] = position + new Vector2(-blockSize/2, blockSize/2);
            vertices[verticesCount + 3] = position + new Vector2(blockSize/2, blockSize/2);

            //index is number of vertices / 4 then multiplied by 6
            triangles[verticesCount * 3 / 2] = verticesCount;
            triangles[verticesCount * 3 / 2 + 1] = verticesCount + 2;
            triangles[verticesCount * 3 / 2 + 2] = verticesCount + 1;
            triangles[verticesCount * 3 / 2 + 3] = verticesCount + 2;
            triangles[verticesCount * 3 / 2 + 4] = verticesCount + 3;
            triangles[verticesCount * 3 / 2 + 5] = verticesCount + 1;

            Rect blockUV = SpriteSheetCreator.blockUV[blocks[position]];

            uv[verticesCount] = new Vector2(blockUV.x, blockUV.y);
            uv[verticesCount + 1] = new Vector2(blockUV.x + blockUV.width, blockUV.y);
            uv[verticesCount + 2] = new Vector2(blockUV.x, blockUV.y + blockUV.height);
            uv[verticesCount + 3] = new Vector2(blockUV.x + blockUV.width, blockUV.y + blockUV.height);

            verticesCount += 4;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}