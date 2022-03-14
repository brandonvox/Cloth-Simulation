using UnityEngine;

public class Point
{
    public Vector3 position;
    public Vector3 previousPosition;
    public Transform transform;
    public bool locked;

    public Point(Vector3 position, Vector3 previousPosition, Transform transform, bool locked)
    {
        this.position = position;
        this.previousPosition = previousPosition;
        this.transform = transform;
        this.locked = locked;
    }

    public void UpdateTransform()
    {
        transform.position = position;
    }
}
