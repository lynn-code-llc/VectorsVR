using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CrossProductAreaMesh : MonoBehaviour
{
    public Vector3 vectorA;
    public Vector3 vectorB;

    private Vector3 lastVectorA;
    private Vector3 lastVectorB;

    private MeshFilter _meshFilter;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        
        CreateMesh();
    }

    void Update()
    {
        // Check if either vectorA or vectorB has changed since the last frame
        if (vectorA != lastVectorA || vectorB != lastVectorB)
        {
            CreateMesh(); // Recreate the mesh with the new vectors
            lastVectorA = vectorA;
            lastVectorB = vectorB;
        }
    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();

        // Assuming p0, p1, p2, and p3 are defined as before
        Vector3 p0 = Vector3.zero;
        Vector3 p1 = vectorA;
        Vector3 p2 = vectorB;
        Vector3 p3 = vectorA + vectorB;

        mesh.vertices = new Vector3[] { p0, p1, p3, p3, p2, p0,  // Front face
            p0, p3, p1, p0, p2, p3}; // Back face (inverted)

        // Triangles - duplicate and reverse for double-sided
        mesh.triangles = new int[] { 0, 1, 2, 3, 4, 5,  // Front face
            6, 7, 8, 9, 10, 11}; // Back face

        mesh.RecalculateNormals();

        _meshFilter.mesh = mesh;

    }
}