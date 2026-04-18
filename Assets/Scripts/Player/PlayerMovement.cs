using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float maxDragDistance = 10f;
    public float touchRadius = 1.5f;    // extra radius around player

    private bool isDragging = false;
    private bool isMoving = false;
    private Vector3 dragStartWorld;
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isMoving)
            HandleInput();
    }

    bool IsTouchingPlayer(Vector3 worldPos)
    {
        // Check 1 - directly on the collider
        if (col.OverlapPoint(worldPos))
            return true;

        // Check 2 - within radius around player
        float dist = Vector3.Distance(worldPos, transform.position);
        if (dist <= touchRadius)
            return true;

        return false;
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;

            if (IsTouchingPlayer(worldPos))
            {
                isDragging = true;
                dragStartWorld = worldPos;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;

            Vector3 dir = worldPos - dragStartWorld;
            float dist = Mathf.Min(dir.magnitude, maxDragDistance);

            if (dist > 0.05f)
            {
                Vector3 target = transform.position + dir.normalized * dist;
                StartCoroutine(MoveToTarget(target));
            }
        }
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
            GameManager.instance.Win();

        if (other.CompareTag("Trap"))
            GameManager.instance.Lose();
    }
}