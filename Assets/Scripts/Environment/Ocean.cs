using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Ocean : MonoBehaviour {

    public int grid_size_x;
    public int grid_size_z;

    public float tile_size;

    public Material ocean_material;

    private Mesh mesh;
    private Vector3[] vertices;

    void Awake() {
        GenerateGrid(grid_size_x, grid_size_z, tile_size);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void GenerateGrid(int xSize, int zSize, float tileSize) {

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Ocean Grid";

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        for(int i = 0, z = 0; z <= zSize; z++) {
            for(int x = 0; x <= xSize; x++, i++) {

                float xPos = transform.position.x + ((x - xSize / 2) * tileSize);
                float zPos = transform.position.z + ((z - zSize / 2) * tileSize);

                vertices[i] = new Vector3(xPos, transform.position.y, zPos);
                uv[i] = new Vector2(x / xSize, z / zSize);

            }
        }

        mesh.vertices = vertices;

        int[] triangles = new int[xSize * zSize * 6];
        for(int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++) {
            for(int x = 0; x < xSize; x++, ti += 6, vi++) {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos() {
        if(vertices == null) {
            return;
        }

        Gizmos.color = Color.black;
        for(int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
