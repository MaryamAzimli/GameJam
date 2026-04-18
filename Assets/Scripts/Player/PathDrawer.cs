using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class PathDrawer : MonoBehaviour
{
    [Header("Path Settings")]
    public float minPointDistance = 0.15f; // how often to sample drag points
    public int maxPathPoints = 100;         // cap to avoid huge paths

    [Header("Line Appearance")]
    public float lineWidth = 0.08f;
    public Color lineColor = new Color(1f, 1f, 1f, 0.8f);
    public Material lineMaterial;          // assign a Sprites/Default material

    private LineRenderer lr;
    private List<Vector3> pathPoints = new List<Vector3>();
    public bool IsDrawing { get; private set; }

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth * 0.4f; // tapers toward end
        lr.material = lineMaterial;
        lr.startColor = lineColor;
        lr.endColor = new Color(lineColor.r, lineColor.g, lineColor.b, 0.2f);
        lr.positionCount = 0;
        lr.useWorldSpace = true;
        lr.sortingOrder = 10;
    }

    public void StartPath(Vector3 startPos)
    {
        pathPoints.Clear();
        pathPoints.Add(startPos);
        IsDrawing = true;
        UpdateLine();
    }

    public void AddPoint(Vector3 newPoint)
    {
        if (!IsDrawing) return;
        if (pathPoints.Count >= maxPathPoints) return;

        Vector3 last = pathPoints[pathPoints.Count - 1];
        if (Vector3.Distance(last, newPoint) >= minPointDistance)
        {
            pathPoints.Add(newPoint);
            UpdateLine();
        }
    }

    public List<Vector3> EndPath()
    {
        IsDrawing = false;
        List<Vector3> result = new List<Vector3>(pathPoints);
        ClearLine();
        return result;
    }

    void UpdateLine()
    {
        lr.positionCount = pathPoints.Count;
        lr.SetPositions(pathPoints.ToArray());
    }

    void ClearLine()
    {
        pathPoints.Clear();
        lr.positionCount = 0;
    }
}