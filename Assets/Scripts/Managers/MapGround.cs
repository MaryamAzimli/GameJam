using UnityEngine;

public class MapBounds : MonoBehaviour
{
    public static MapBounds instance { get; private set; }

    public Transform bottomLeft;
    public Transform topRight;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void OnEnable()
    {
        instance = this;
    }

    public float MinX => bottomLeft != null ? bottomLeft.position.x : 0;
    public float MaxX => topRight != null ? topRight.position.x : 0;
    public float MinY => bottomLeft != null ? bottomLeft.position.y : 0;
    public float MaxY => topRight != null ? topRight.position.y : 0;

    void OnDrawGizmos()
    {
        if (bottomLeft == null || topRight == null) return;

        Gizmos.color = Color.green;
        Vector3 center = (bottomLeft.position + topRight.position) / 2f;
        Vector3 size = topRight.position - bottomLeft.position;
        Gizmos.DrawWireCube(center, size);
    }
}