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

    public float scroll_speed = 0.5f;
    private float offset;
    private float rotate;

    private MeshRenderer renderer;
    private bool initGameEnd;
    private float stoppingObstacleSpeed;

    void Awake() {
        GenerateGrid(grid_size_x, grid_size_z, tile_size);
        renderer = GetComponent<MeshRenderer>();
        renderer.material.renderQueue = 3001;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        offset += (Time.deltaTime * Time.timeScale * scroll_speed) / 25.0f;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));

        if (GameController.getGameState() == GameController.State.GAMEOVER) {

            if (!initGameEnd) {
                initGameEnd = true;
                stoppingObstacleSpeed = scroll_speed;
            }
            else {
                stoppingObstacleSpeed *= (0.99f);
            }

            scroll_speed = stoppingObstacleSpeed;
        }
    }

    private void GenerateGrid(int xSize, int zSize, float tileSize) {

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Ocean Grid";

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        for(int i = 0, z = 0; z <= zSize; z++) {
            for(int x = 0; x <= xSize; x++, i++) {

                float xPos = (x - xSize / 2) * tileSize;
                float zPos = (z - zSize / 2) * tileSize;

                vertices[i] = new Vector3(xPos, transform.position.y, zPos);
                uv[i] = new Vector2(x / (xSize*1.0f), z / (zSize*1.0f));
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;

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

        calculateMeshTangents(mesh);
    }

    public static void calculateMeshTangents(Mesh mesh) {
        //speed up math by copying the mesh arrays
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uv = mesh.uv;
        Vector3[] normals = mesh.normals;

        //variable definitions
        int triangleCount = triangles.Length;
        int vertexCount = vertices.Length;

        Vector3[] tan1 = new Vector3[vertexCount];
        Vector3[] tan2 = new Vector3[vertexCount];

        Vector4[] tangents = new Vector4[vertexCount];

        for (long a = 0; a < triangleCount; a += 3) {
            long i1 = triangles[a + 0];
            long i2 = triangles[a + 1];
            long i3 = triangles[a + 2];

            Vector3 v1 = vertices[i1];
            Vector3 v2 = vertices[i2];
            Vector3 v3 = vertices[i3];

            Vector2 w1 = uv[i1];
            Vector2 w2 = uv[i2];
            Vector2 w3 = uv[i3];

            float x1 = v2.x - v1.x;
            float x2 = v3.x - v1.x;
            float y1 = v2.y - v1.y;
            float y2 = v3.y - v1.y;
            float z1 = v2.z - v1.z;
            float z2 = v3.z - v1.z;

            float s1 = w2.x - w1.x;
            float s2 = w3.x - w1.x;
            float t1 = w2.y - w1.y;
            float t2 = w3.y - w1.y;

            float r = 1.0f / (s1 * t2 - s2 * t1);

            Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;
        }


        for (long a = 0; a < vertexCount; ++a) {
            Vector3 n = normals[a];
            Vector3 t = tan1[a];

            //Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
            //tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
            Vector3.OrthoNormalize(ref n, ref t);
            tangents[a].x = t.x;
            tangents[a].y = t.y;
            tangents[a].z = t.z;

            tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
        }

        mesh.tangents = tangents;
    }

    private void OnDrawGizmos() {
        if(vertices == null) {
            return;
        }

        Gizmos.color = Color.black;
        for(int i = 0; i < vertices.Length; i++) {
          //  Gizmos.DrawSphere(transform.position + vertices[i], 0.1f);
        }
    }
}
