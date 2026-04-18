using UnityEngine;

public class VectorArrow : MonoBehaviour
{
    [Header("Arrow Visuals")]
    public float maxDragDistance = 5f;
    public Color arrowColor = Color.white;
    public Color dangerColor = Color.red;
    public float arrowHeadSize = 0.3f;
    public float lineWidth = 0.06f;

    private LineRenderer shaft;
    private LineRenderer head;

    void Awake()
    {
        // --- Shaft ---
        shaft = gameObject.AddComponent<LineRenderer>();
        SetupLine(shaft);
        shaft.positionCount = 2;

        // --- Arrowhead (V shape on a child object) ---
        GameObject headObj = new GameObject("ArrowHead");
        headObj.transform.SetParent(transform);
        head = headObj.AddComponent<LineRenderer>();
        SetupLine(head);
        head.positionCount = 3; // left wing → tip → right wing

        shaft.enabled = false;
        head.enabled = false;
    }

    void SetupLine(LineRenderer lr)
    {
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.useWorldSpace = true;
        lr.sortingOrder = 10;
    }

    public void ShowArrow(Vector3 from, Vector3 to)
    {
        Vector3 dir = to - from;
        float dist = dir.magnitude;
        float t = Mathf.Clamp01(dist / maxDragDistance);
        Vector3 clampedTo = from + dir.normalized * Mathf.Min(dist, maxDragDistance);

        Color c = Color.Lerp(arrowColor, dangerColor, t);

        // Draw shaft
        shaft.enabled = true;
        shaft.startColor = c;
        shaft.endColor = c;
        shaft.SetPosition(0, from);
        shaft.SetPosition(1, clampedTo);

        // Draw arrowhead V
        head.enabled = true;
        head.startColor = c;
        head.endColor = c;

        Vector3 forward = dir.normalized;
        Vector3 right = new Vector3(-forward.y, forward.x, 0f); // perpendicular

        Vector3 tip = clampedTo;
        Vector3 leftWing  = tip - forward * arrowHeadSize + right * arrowHeadSize * 0.5f;
        Vector3 rightWing = tip - forward * arrowHeadSize - right * arrowHeadSize * 0.5f;

        head.SetPosition(0, leftWing);
        head.SetPosition(1, tip);
        head.SetPosition(2, rightWing);
    }

    public void HideArrow()
    {
        shaft.enabled = false;
        head.enabled = false;
    }

    public Vector3 GetClampedTarget(Vector3 from, Vector3 rawTo)
    {
        Vector3 dir = rawTo - from;
        float dist = Mathf.Min(dir.magnitude, maxDragDistance);
        return from + dir.normalized * dist;
    }
}