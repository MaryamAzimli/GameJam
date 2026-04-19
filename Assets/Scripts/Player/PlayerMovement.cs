using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float maxDragDistance = 10f;
    public float touchRadius = 1.5f;

    [Header("Puzzle Setup")]
    public GameObject[] carriedFoodVisuals; // 0: Bal, 1: Muz, 2: Havuç
    public AudioClip grabSound;
    public AudioClip successSound;
    public AudioClip failSound;

    [Header("Riddle UI")]
    public GameObject riddlePanel; // Unity'den RiddlePanel'i buraya sürükle

    private AudioSource audioSource;
    private bool isDragging = false;
    private bool isMoving = false;

    private Vector3 dragStartWorld;
    private Animator anim;
    private Collider2D col;
    public Vector2 gridOrigin = Vector2.zero;
    private Vector2Int currentGridPos;
    private Vector3 startPos;
    private Animator anim;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // Oyun ba?lad??? andaki pozisyonunu "güvenli bölge" olarak kaydet
        startPos = transform.position;
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
        // Panel aç?kken veya oyun bittiyse hareket etme
        if (!isMoving && GameManager.instance != null && !GameManager.instance.isGameOver && (riddlePanel == null || !riddlePanel.activeSelf))
        {
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
            if (col.OverlapPoint(worldPos) || Vector3.Distance(worldPos, transform.position) <= touchRadius)
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
        isMoving = true;
        if (anim != null) anim.SetBool("isMoving", true);

        while (Vector3.Distance(transform.position, target) > 0.05f)
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
        if (anim != null) anim.SetBool("isMoving", false);
    }

    // --- RIDDLE UI FONKS?YONLARI ---
    public void OpenRiddle()
    {
        if (riddlePanel != null)
        {
            riddlePanel.SetActive(true);
            Time.timeScale = 0f; // Oyunu durdur
        }
    }

    public void CloseRiddle()
    {
        if (riddlePanel != null)
        {
            riddlePanel.SetActive(false);
            Time.timeScale = 1f; // Oyunu devam ettir
        }
    }

    // --- ÇARPI?MA KONTROLLER? ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance == null || GameManager.instance.isGameOver) return;

        // 1. YEMEK TOPLAMA
        if (other.CompareTag("Food1") || other.CompareTag("Food2") || other.CompareTag("Food3"))
        {
            if (!GameManager.instance.hasFood)
            {
                if (other.CompareTag("Food1")) GameManager.instance.activeFoodID = 1;
                else if (other.CompareTag("Food2")) GameManager.instance.activeFoodID = 2;
                else if (other.CompareTag("Food3")) GameManager.instance.activeFoodID = 3;

                GameManager.instance.hasFood = true;
                if (grabSound != null) audioSource.PlayOneShot(grabSound);
                other.gameObject.SetActive(false);
                UpdateFoodVisual(GameManager.instance.activeFoodID - 1, true);
            }
        }
        // 2. HAYVAN BESLEME
        else if (other.CompareTag("Animal1") || other.CompareTag("Animal2") || other.CompareTag("Animal3"))
        {
            string targetAnimalTag = "Animal" + GameManager.instance.currentStep;
    bool IsTouchingPlayer(Vector3 worldPos)
    {
        if (col != null && col.OverlapPoint(worldPos))
            return true;

            if (other.CompareTag(targetAnimalTag) && GameManager.instance.hasFood && GameManager.instance.activeFoodID == GameManager.instance.currentStep)
            {
                if (successSound != null) audioSource.PlayOneShot(successSound);
                UpdateFoodVisual(GameManager.instance.activeFoodID - 1, false);
                GameManager.instance.hasFood = false;
                GameManager.instance.currentStep++;
                if (GameManager.instance.currentStep > 3) GameManager.instance.Win();
            }
            else { HandleFail(); }
        }
        else if (other.CompareTag("Trap")) { HandleFail(); }
    }

    void HandleFail()
    {
        if (failSound != null) audioSource.PlayOneShot(failSound);
        StopAllCoroutines();
        isMoving = false;
        if (anim != null) anim.SetBool("isMoving", false);
        transform.position = startPos;
        GameManager.instance.Lose();
    }

    void UpdateFoodVisual(int index, bool state)
    {
        foreach (var v in carriedFoodVisuals) if (v != null) v.SetActive(false);
        if (state && index >= 0 && index < carriedFoodVisuals.Length && carriedFoodVisuals[index] != null)
            carriedFoodVisuals[index].SetActive(true);
        return Vector3.Distance(worldPos, transform.position) < 1.5f;
    }
}