using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 9f;
    public float touchRadius = 1.5f;
    private Vector2Int currentGridPos;

    [Header("Puzzle Setup")]
    public GameObject[] carriedFoodVisuals; // 0: Honey, 1: Banana, 2: Carrot
    public AudioClip pickSound; // Pick sound for items
    public AudioClip bikSound;  // Health decrease sound
    public AudioClip successSound;
    public AudioClip failSound;

    [Header("Riddle UI")]
    public GameObject riddlePanel;

    private AudioSource audioSource;
    private bool isDragging = false;
    private bool isMoving = false;
    private Vector3 dragStartWorld;
    private Animator anim;
    private Collider2D col;
    private Vector3 startWorldPos;

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // Save starting position to return on fail
        startWorldPos = transform.position;
    }

    void Start()
    {
        // Set starting grid position to (1,1)
        currentGridPos = new Vector2Int(1, 1);
        transform.position = GridToWorld(currentGridPos);
    }

    void Update()
    {
        // Input check if not moving and game is active
        if (!isMoving && GameManager.instance != null && !GameManager.instance.isGameOver && (riddlePanel == null || !riddlePanel.activeSelf))
        {
            HandleInput();
        }
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
            if (dir.magnitude < 0.2f) return;

            Vector2Int moveDir;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                moveDir = dir.x > 0 ? Vector2Int.right : Vector2Int.left;
            else
                moveDir = dir.y > 0 ? Vector2Int.up : Vector2Int.down;

            Vector2Int targetGrid = currentGridPos + moveDir;

            // Boundary check
            if (targetGrid.x >= 0 && targetGrid.x < Gridofthemap.instance.width &&
                targetGrid.y >= 0 && targetGrid.y < Gridofthemap.instance.height)
            {
                if (Gridofthemap.instance.IsWalkable(targetGrid))
                {
                    currentGridPos = targetGrid;
                    StopAllCoroutines();
                    StartCoroutine(MoveTo(GridToWorld(targetGrid)));
                }
            }
        }
    }

    // ---------------- MOVEMENT ----------------

    IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;
        if (anim != null) anim.SetBool("isMoving", true);

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        if (anim != null) anim.SetBool("isMoving", false);
        isMoving = false;
    }

    // --- RIDDLE UI ---
    public void OpenRiddle()
    {
        if (riddlePanel != null)
        {
            riddlePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void CloseRiddle()
    {
        if (riddlePanel != null)
        {
            riddlePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // --- COLLISION AND LOGIC ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance == null || GameManager.instance.isGameOver) return;

        // 1. PICKING FOOD
        if (other.CompareTag("Food1") || other.CompareTag("Food2") || other.CompareTag("Food3"))
        {
            if (!GameManager.instance.hasFood)
            {
                if (other.CompareTag("Food1")) GameManager.instance.activeFoodID = 1;
                else if (other.CompareTag("Food2")) GameManager.instance.activeFoodID = 2;
                else if (other.CompareTag("Food3")) GameManager.instance.activeFoodID = 3;

                GameManager.instance.hasFood = true;

                // Play pick sound
                if (pickSound != null) audioSource.PlayOneShot(pickSound);

                other.gameObject.SetActive(false);
                UpdateFoodVisual(GameManager.instance.activeFoodID - 1, true);
            }
        }
        // 2. FEEDING ANIMALS
        else if (other.CompareTag("Animal1") || other.CompareTag("Animal2") || other.CompareTag("Animal3"))
        {
            int animalID = int.Parse(other.tag.Substring(6));

            if (animalID == GameManager.instance.currentStep && GameManager.instance.hasFood)
            {
                if (successSound != null) audioSource.PlayOneShot(successSound);

                UpdateFoodVisual(GameManager.instance.activeFoodID - 1, false);
                GameManager.instance.hasFood = false;

                UnlockNextItem(GameManager.instance.currentStep, other.transform.position);
                GameManager.instance.currentStep++;

                if (GameManager.instance.currentStep > 3)
                    GameManager.instance.Win();
            }
            else
            {
                HandleFail();
            }
        }
        else if (other.CompareTag("Trap")) { HandleFail(); }
    }

    void UnlockNextItem(int completedStep, Vector3 animalPosition)
    {
        GameObject nextFood = null;
        Vector3 dropOffset = Vector3.zero;

        if (completedStep == 1)
        {
            nextFood = GameObject.FindWithTag("Food2");
            dropOffset = new Vector3(0, -Gridofthemap.instance.cellSize, 0);
        }
        else if (completedStep == 2)
        {
            nextFood = GameObject.FindWithTag("Food3");
            dropOffset = new Vector3(Gridofthemap.instance.cellSize, 0, 0);
        }

        if (nextFood != null)
        {
            nextFood.transform.position = animalPosition + dropOffset;
            nextFood.SetActive(true);
            SpriteRenderer sr = nextFood.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = true;
        }
    }

    void HandleFail()
    {
        // Play health decrease sound
        if (bikSound != null) audioSource.PlayOneShot(bikSound);

        StopAllCoroutines();
        isMoving = false;
        if (anim != null) anim.SetBool("isMoving", false);

        // Reset grid and world position
        transform.position = startWorldPos;
        currentGridPos = WorldToGrid(startWorldPos);

        GameManager.instance.Lose();
    }

    void UpdateFoodVisual(int index, bool state)
    {
        foreach (var v in carriedFoodVisuals) if (v != null) v.SetActive(false);
        if (state && index >= 0 && index < carriedFoodVisuals.Length && carriedFoodVisuals[index] != null)
            carriedFoodVisuals[index].SetActive(true);
    }

    bool IsTouchingPlayer(Vector3 worldPos)
    {
        if (col != null && col.OverlapPoint(worldPos)) return true;
        return Vector3.Distance(worldPos, transform.position) < touchRadius;
    }
}