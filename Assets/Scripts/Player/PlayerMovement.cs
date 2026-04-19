using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 9f;
    public float touchRadius = 1.5f;
    private Vector2Int currentGridPos;

    [Header("Puzzle Setup")]
    public GameObject[] carriedFoodVisuals; // 0: Bal, 1: Muz, 2: Havu�
    public AudioClip grabSound;
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
    private Vector3 startWorldPos; // Fail durumunda d�n�lecek d�nya koordinat?

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // Fail durumunda ilk ba?lad??? yere d�nmesi i�in
        startWorldPos = transform.position;
    }

    void Start()
    {
        // Ba?lang?� pozisyonunu (1,1) h�cresine setle
        currentGridPos = new Vector2Int(1, 1);
        transform.position = GridToWorld(currentGridPos);
    }

    void Update()
    {
        // Hareket etmiyorken, oyun bitmemi?ken ve Riddle paneli kapal?yken input al
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
            if (dir.magnitude < 0.2f) return; // �ok k���k kayd?rmalar? yoksay

            Vector2Int moveDir;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                moveDir = dir.x > 0 ? Vector2Int.right : Vector2Int.left;
            else
                moveDir = dir.y > 0 ? Vector2Int.up : Vector2Int.down;

            Vector2Int targetGrid = currentGridPos + moveDir;

            // S?n?r kontrol�
            if (targetGrid.x >= 0 && targetGrid.x < Gridofthemap.instance.width &&
                targetGrid.y >= 0 && targetGrid.y < Gridofthemap.instance.height)
            {
                if (Gridofthemap.instance.IsWalkable(targetGrid))
                {
                    currentGridPos = targetGrid;
                    StopAllCoroutines();
                    StartCoroutine(MoveTo(GridToWorld(targetGrid)));
                }
                else
                {
                    Debug.Log("Yol kapal?!");
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

    // --- �ARPI?MA VE OYUN MANTI?I ---
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player touched something: " + other.gameObject.name + " with tag: " + other.tag);

        if (GameManager.instance == null || GameManager.instance.isGameOver) return;
        // 1. YEMEK TOPLAMA
        // 1. YEMEK TOPLAMA
        // 1. YEMEK TOPLAMA (Food Pickup)
// 1. YEMEK TOPLAMA
if (other.CompareTag("Food1") || other.CompareTag("Food2") || other.CompareTag("Food3"))
{
    int foodID = int.Parse(other.tag.Substring(4));

    if (!GameManager.instance.hasFood && foodID == GameManager.instance.currentStep)
    {
        GameManager.instance.activeFoodID = foodID;
        GameManager.instance.hasFood = true;
        
        other.gameObject.SetActive(false);
        
        // Use the actual foodID here!
        UpdateFoodVisual(foodID, true); 
        
        Debug.Log("Pickup successful. Visual updated for ID: " + foodID);
    }
}
        // 2. HAYVAN BESLEME
        // Inside OnTriggerEnter2D...
        // 2. HAYVAN BESLEME
        else if (other.CompareTag("Animal1") || other.CompareTag("Animal2") || other.CompareTag("Animal3"))
{
    // Extract ID (e.g., "Animal1" -> 1)
    int animalID = int.Parse(other.tag.Substring(6));

    // Check if this animal matches the current step AND the player is holding the right food
    if (animalID == GameManager.instance.currentStep && 
        GameManager.instance.hasFood && 
        GameManager.instance.activeFoodID == animalID)
    {
        if (successSound != null) audioSource.PlayOneShot(successSound);

        // Remove food visual from player's hands
        UpdateFoodVisual(GameManager.instance.activeFoodID - 1, false);
        GameManager.instance.hasFood = false;

        // Drop the next item near this animal
        UnlockNextItem(GameManager.instance.currentStep, other.transform.position);

        // Advance the game state
        GameManager.instance.currentStep++;

        // Hide the animal so the path is clear
        other.gameObject.SetActive(false); 

        if (GameManager.instance.currentStep > 3)
            GameManager.instance.Win();
    }
    else if (GameManager.instance.hasFood && GameManager.instance.activeFoodID != animalID)
    {
        Debug.Log("The animal sniffed the food but didn't like it. Wrong item!");
    }
}
        else if (other.CompareTag("Trap")) { HandleFail(); }
    }
   void UnlockNextItem(int completedStep, Vector3 animalPosition)
{
    GameObject nextFood = null;
    Vector3 dropOffset = Vector3.zero;

    // Step 1: You fed the Monkey (Animal1) the Banana (Food1)
    if (completedStep == 1) 
    {
        nextFood = GameObject.FindWithTag("Food2"); // The Carrot appears
        dropOffset = new Vector3(Gridofthemap.instance.cellSize, 0, 0); // Drop to the right
    }
    // Step 2: You fed the Bear (Animal2) the Carrot (Food2)
    else if (completedStep == 2)
    {
        nextFood = GameObject.FindWithTag("Food3"); // The Honey appears
        dropOffset = new Vector3(0, -Gridofthemap.instance.cellSize, 0); // Drop below
    }

    if (nextFood != null)
{
    // 1. Move it to the new spot
    nextFood.transform.position = animalPosition + dropOffset;

    // 2. Make sure the object itself is Active
    nextFood.SetActive(true);

    // 3. FORCE the SpriteRenderer to turn on (since we unchecked it in Inspector)
    SpriteRenderer sr = nextFood.GetComponent<SpriteRenderer>();
    if (sr != null) 
    {
        sr.enabled = true; 
        // Optional: Set sorting order high to make sure it's above the grid
        sr.sortingOrder = 5; 
    }

    Debug.Log($"{nextFood.name} is now visible at {nextFood.transform.position}");
}
}
    void HandleFail()
    {
        if (failSound != null) audioSource.PlayOneShot(failSound);
        StopAllCoroutines();
        isMoving = false;
        if (anim != null) anim.SetBool("isMoving", false);

        // Grid ve D�nya pozisyonunu ba?a sar
        transform.position = startWorldPos;
        currentGridPos = WorldToGrid(startWorldPos);

        GameManager.instance.Lose();
    }
void UpdateFoodVisual(int foodID, bool state)
{
    // First, hide everything to be safe
    foreach (var v in carriedFoodVisuals) 
    {
        if (v != null) v.SetActive(false);
    }

    // foodID 1 = Banana (Element 0)
    // foodID 2 = Carrot (Element 1)
    // foodID 3 = Honey  (Element 2)
    int index = foodID - 1; 

    if (state && index >= 0 && index < carriedFoodVisuals.Length)
    {
        if (carriedFoodVisuals[index] != null)
        {
            carriedFoodVisuals[index].SetActive(true);
            Debug.Log($"Showing visual for Food ID: {foodID} at index {index}");
        }
    }
}

    bool IsTouchingPlayer(Vector3 worldPos)
    {
        if (col != null && col.OverlapPoint(worldPos)) return true;
        return Vector3.Distance(worldPos, transform.position) < touchRadius;
    }
}