using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    private bool isDragging = false;
    private bool isMoving = false;

    private Vector3 dragStartWorld;
    private Animator anim;
    private Collider2D col;
    public Vector2 gridOrigin = Vector2.zero;
    private Vector2Int currentGridPos;

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }
void Start()
{
    // Directly set to the second column (1) of the second row from bottom (1)
    currentGridPos = new Vector2Int(1, 1);
    
    // Teleport the player to the center of that specific cell
    transform.position = GridToWorld(currentGridPos);

    Debug.Log($"Player started at designated Start Point: {currentGridPos}");
}
    void Update()
    {
        if (!isMoving)
            HandleInput();
    }

    // ---------------- GRID CONVERSION ----------------

    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        float cs = Gridofthemap.instance.cellSize;
        Vector2 origin = Gridofthemap.instance.gridOrigin;

        int x = Mathf.FloorToInt((worldPos.x - origin.x) / cs);
        int y = Mathf.FloorToInt((worldPos.y - origin.y) / cs);

        return new Vector2Int(x, y);
    }
    Vector3 GridToWorld(Vector2Int gridPos)
    {
        float cs = Gridofthemap.instance.cellSize;
        Vector2 origin = Gridofthemap.instance.gridOrigin;

        return new Vector3(
        gridPos.x * cs + origin.x + cs / 2f,
        gridPos.y * cs + origin.y + cs / 2f,
        0
    );
    }

    // ---------------- INPUT ----------------

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

            Vector2Int moveDir;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                moveDir = dir.x > 0 ? Vector2Int.right : Vector2Int.left;
            else
                moveDir = dir.y > 0 ? Vector2Int.up : Vector2Int.down;

            Vector2Int targetGrid = currentGridPos + moveDir;

            if (targetGrid.x < 0 || targetGrid.x >= Gridofthemap.instance.width ||
                targetGrid.y < 0 || targetGrid.y >= Gridofthemap.instance.height)
            {
                Debug.Log("Out of bounds!");
                return;
            }
            targetGrid.x = Mathf.Clamp(targetGrid.x, 0, Gridofthemap.instance.width - 1);
            targetGrid.y = Mathf.Clamp(targetGrid.y, 0, Gridofthemap.instance.height - 1);


            Debug.Log("Current: " + currentGridPos + " Target: " + targetGrid);
            if (Gridofthemap.instance.IsWalkable(targetGrid))
            {
                currentGridPos = targetGrid;
                StartCoroutine(MoveTo(GridToWorld(targetGrid)));
            }
            else
            {
                Debug.Log("Blocked!");
            }
        }
    }

    // ---------------- MOVEMENT ----------------

    IEnumerator MoveTo(Vector3 target)
    {
        anim.SetBool("isMoving", true);
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

        anim.SetBool("isMoving", false);
        isMoving = false;
    }

    // ---------------- TOUCH ----------------

    bool IsTouchingPlayer(Vector3 worldPos)
    {
        if (col != null && col.OverlapPoint(worldPos))
            return true;

        return Vector3.Distance(worldPos, transform.position) < 1.5f;
    }
}