using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSimulation : MonoBehaviour
{
    public enum RenderMode
    {
        Point, Mesh
    }


    public GameObject pointPrefab;
    public Point[, ] points;
    public List<Stick> sticks;
    public float gravity = 9.8f;

    public int rows = 3;
    public int columns = 3;
    public float spacing = 1f;

    public MeshFilter meshFilter;

    public RenderMode renderMode;
    private MeshData _meshData;
    private Mesh _mesh; 

    void Start()
    {
        points = CreatePoints();
        sticks = CreateSticks(); 

        _mesh = _meshData.CreateMesh();
        ApplyMeshToMeshFilter(_mesh);
    }

    public List<Stick> CreateSticks()
    {
        var result = new List<Stick>();


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns - 1; j++)
            {
                var newStick = new Stick(points[i, j], points[i, j + 1], spacing); 
                result.Add(newStick);
            }
        }

        for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var newStick = new Stick(points[i, j], points[i + 1, j], spacing);
                result.Add(newStick);
            }
        }

        return result;
    }
    public Point[, ] CreatePoints()
    {
        var result = new Point[rows, columns];
        _meshData = new MeshData(rows, columns);
        var random = new System.Random(); 

        float x;
        float y = 6f;
        int vertexIndex = 0;

        for(int i = 0; i < rows; i++)
        {
            x = -columns * spacing / 2f;
            for(int j = 0; j < columns; j++)
            {
                var position = new Vector3(x, y, 0);
                var randomPosition = new Vector3(random.Next(-10, 10), random.Next(-10, 10), random.Next(-10, 10));
                var pointObject = Instantiate(pointPrefab, transform);
                var newPoint = new Point(position, randomPosition, pointObject.transform, i == 0?true:false);
                if(renderMode == RenderMode.Point)
                {
                    newPoint.UpdateTransform();
                }
                else
                {
                    pointObject.SetActive(false); 
                }
                result[i, j] = newPoint;

                _meshData.verticesPosition[vertexIndex] = position; 
                _meshData.uvs[vertexIndex] = new Vector2(j /(float) columns, i / (float) rows);

                if(i < rows - 1 && j < columns - 1)
                {
                    _meshData.AddTriangles(vertexIndex, vertexIndex + columns + 1, vertexIndex + columns);
                    _meshData.AddTriangles(vertexIndex + columns + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++; 
                x += spacing;
            }
            y -= spacing;
        }

        return result; 
    }
    
    public void ApplyMeshToMeshFilter(Mesh mesh)
    {
        meshFilter.mesh = mesh;
    }

    
    void FixedUpdate()
    {
        UpdatePointsPosition();
        UpdatePointsTransform();
    }
    private void UpdatePointsTransform()
    {

        if(renderMode == RenderMode.Point)
        {
            for (int i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var point = points[i, j];
                    if (point.locked == false)
                    {
                        point.UpdateTransform();
                    }
                }
            }
        }

        else
        {
            int vertexIndex = 0;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    _meshData.verticesPosition[vertexIndex] = points[i, j].position;
                    vertexIndex++;
                }
            }

            _mesh = _meshData.UpdateMesh();
            ApplyMeshToMeshFilter(_mesh);

        }


    }
    private void UpdatePointsPosition()
    {
        for(int i = 0; i < rows; i++)
        {
            for(var j = 0; j < columns; j++)
            {
                var point = points[i, j];
                if (point.locked == false)
                {
                    var oldPosition = point.position;
                    point.position += point.position - point.previousPosition;
                    point.position.y -= gravity * Time.fixedDeltaTime * Time.fixedDeltaTime * 4;
                    point.transform.position = point.position;
                    point.previousPosition = oldPosition;
                }
            }
        }

        for(int i = 0; i< 5; i++)
        {
            foreach (var stick in sticks)
            {
                var stickCenterPosition = (stick.pointA.position + stick.pointB.position) / 2f;
                var stickDirection = (stick.pointA.position - stick.pointB.position).normalized;
                if (stick.pointA.locked == false)
                {
                    stick.pointA.position = stickCenterPosition + stick.length / 2f * stickDirection;
                }
                if (stick.pointB.locked == false)
                {
                    stick.pointB.position = stickCenterPosition - stick.length / 2f * stickDirection;
                }
            }
        }

    }
}
