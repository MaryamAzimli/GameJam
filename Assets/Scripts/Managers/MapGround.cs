using UnityEngine;

public class MapBounds : MonoBehaviour
{
    public static MapBounds instance;

    public Transform bottomLeft;
    public Transform topRight;

    void Awake()
    {
        instance = this;
    }

    public float MinX => bottomLeft.position.x;
    public float MaxX => topRight.position.x;
    public float MinY => bottomLeft.position.y;
    public float MaxY => topRight.position.y;

    // Optional: visual debug box in Scene view
    void OnDrawGizmos()
    {
        if (bottomLeft == null || topRight == null) return;

        Gizmos.color = Color.green;

        Vector3 center = (bottomLeft.position + topRight.position) / 2f;
        Vector3 size = topRight.position - bottomLeft.position;

        Gizmos.DrawWireCube(center, size);
    }
}