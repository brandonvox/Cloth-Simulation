using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>(); 
    }

    public void UpdatePositions(Vector3 a, Vector3 b)
    {
        _lineRenderer.SetPosition(0, a); 
        _lineRenderer.SetPosition(1, b);    
    }
}
