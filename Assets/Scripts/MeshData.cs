using UnityEngine; 


public class MeshData
{
    public Vector3[] verticesPosition;
    public Vector2[] uvs;
    public int[] triangles;
    private int _triangleIndex;

    private Mesh _mesh;

    public MeshData(int rows, int columns)
    {
        this.verticesPosition = new Vector3[rows * columns];
        this.uvs = new Vector2[rows * columns]; ;
        this.triangles = new int[ (rows-1)*(columns-1)*6];
    }

    public void AddTriangles(int a, int b, int c)
    {
        triangles[_triangleIndex] = a;
        triangles[_triangleIndex + 1] = b;
        triangles[_triangleIndex + 2] = c;
        _triangleIndex += 3;
    }

    public Mesh CreateMesh() { 
        _mesh = new Mesh();
        _mesh.vertices = verticesPosition;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals(); // Lighting
        return _mesh;
    }

    public Mesh UpdateMesh()
    {
        _mesh.vertices = verticesPosition;
        _mesh.RecalculateNormals(); 
        return _mesh; 
    }

    
}

