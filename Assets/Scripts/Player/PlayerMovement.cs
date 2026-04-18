using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float maxDragDistance = 5f;
    public float touchRadius = 1.5f;

    private bool isDragging = false;
    private bool isMoving = false;

    private Vector2 dragStartScreen;
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

    // ---------------- TOUCH DETECTION ----------------
    bool IsTouchingPlayer(Vector3 worldPos)
    {
        if (col != null && col.OverlapPoint(worldPos))
            return true;

        float dist = Vector3.Distance(worldPos, transform.position);
        return dist <= touchRadius;
    }

    // ---------------- INPUT ----------------
    void HandleInput()
    {
        // START DRAG
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;

            if (IsTouchingPlayer(worldPos))
            {
                isDragging = true;
                dragStartScreen = Input.mousePosition;
            }
        }

        // END DRAG
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            Vector2 dragEndScreen = Input.mousePosition;
            Vector2 dragVector = dragEndScreen - dragStartScreen;

            float screenDistance = dragVector.magnitude;

            if (screenDistance > 0.1f)
            {
                Vector2 direction = dragVector.normalized;

                // Screen → world movement scaling
                float worldDistance = Mathf.Min(screenDistance * 0.01f, maxDragDistance);

                Vector3 target = transform.position +
                                 new Vector3(direction.x, direction.y, 0f) * worldDistance;

                // ---------------- BOUNDS ----------------
                target.x = Mathf.Clamp(target.x, MapBounds.instance.MinX, MapBounds.instance.MaxX);
                target.y = Mathf.Clamp(target.y, MapBounds.instance.MinY, MapBounds.instance.MaxY);

                StartCoroutine(MoveToTarget(target));
            }
        }
    }

    // ---------------- MOVEMENT ----------------
    IEnumerator MoveToTarget(Vector3 target)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    // ---------------- GAME LOGIC ----------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
            FindObjectOfType<GameManager>().Win();

        if (other.CompareTag("Trap"))
            FindObjectOfType<GameManager>().Lose();
    }
}