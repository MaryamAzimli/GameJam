using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            else
                return;
        }

        if (MapBounds.instance == null)
        {
            Debug.LogError("MapBounds instance is missing in scene!");
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("No camera tagged as MainCamera!");
            return;
        }

        Vector3 desired = new Vector3(target.position.x, target.position.y, -10f);

        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        float minX = MapBounds.instance.MinX;
        float maxX = MapBounds.instance.MaxX;
        float minY = MapBounds.instance.MinY;
        float maxY = MapBounds.instance.MaxY;

        desired.x = Mathf.Clamp(desired.x, minX + camWidth, maxX - camWidth);
        desired.y = Mathf.Clamp(desired.y, minY + camHeight, maxY - camHeight);

        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            smooth * Time.deltaTime
        );
    }
}