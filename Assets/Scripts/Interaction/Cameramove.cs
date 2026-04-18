using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    void LateUpdate()
    {
        // Auto-find player if missing (after restart etc.)
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;

            return;
        }

        // Desired position (follow player)
        Vector3 desired = new Vector3(target.position.x, target.position.y, -10f);

        // Camera size
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        // Map bounds (PascalCase ✔)
        float minX = MapBounds.instance.MinX;
        float maxX = MapBounds.instance.MaxX;
        float minY = MapBounds.instance.MinY;
        float maxY = MapBounds.instance.MaxY;

        // Clamp camera so it never shows outside map
        desired.x = Mathf.Clamp(desired.x, minX + camWidth, maxX - camWidth);
        desired.y = Mathf.Clamp(desired.y, minY + camHeight, maxY - camHeight);

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
    }
}