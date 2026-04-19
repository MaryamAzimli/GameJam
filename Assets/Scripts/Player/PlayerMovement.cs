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
    public AudioClip successSound;
    public AudioClip failSound;

    private AudioSource audioSource;
    private bool isDragging = false;
    private bool isMoving = false;
    private Animator anim;
    private Vector3 dragStartWorld;
    private Collider2D col;
    private Vector3 startPos;

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        startPos = transform.position;
    }

    void Update()
    {
        if (!isMoving && !GameManager.instance.isGameOver)
            HandleInput();
    }

    bool IsTouchingPlayer(Vector3 worldPos)
    {
        if (col.OverlapPoint(worldPos)) return true;
        float dist = Vector3.Distance(worldPos, transform.position);
        return dist <= touchRadius;
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;
            if (IsTouchingPlayer(worldPos)) { isDragging = true; dragStartWorld = worldPos; }
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
                StopAllCoroutines();
                StartCoroutine(MoveToTarget(target));
            }
        }
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        anim.SetBool("isMoving", true);
        isMoving = true;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
        isMoving = false;
        anim.SetBool("isMoving", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance == null || GameManager.instance.isGameOver) return;

        // 1. HERHANG? B?R YEME?? TOPLAMA
        if (other.CompareTag("Food1") || other.CompareTag("Food2") || other.CompareTag("Food3"))
        {
            if (!GameManager.instance.hasFood)
            {
                if (other.CompareTag("Food1")) GameManager.instance.activeFoodID = 1;
                else if (other.CompareTag("Food2")) GameManager.instance.activeFoodID = 2;
                else if (other.CompareTag("Food3")) GameManager.instance.activeFoodID = 3;

                GameManager.instance.hasFood = true;
                other.gameObject.SetActive(false);
                UpdateFoodVisual(GameManager.instance.activeFoodID - 1, true);
            }
            return;
        }

        // 2. HAYVAN BESLEME (S?raya Göre)
        if (other.CompareTag("Animal1") || other.CompareTag("Animal2") || other.CompareTag("Animal3"))
        {
            string targetAnimalTag = "Animal" + GameManager.instance.currentStep;

            // E?er elindeki yemek o anki hayvana aitse (Örn: Bal(1) -> Ay?(1))
            if (other.CompareTag(targetAnimalTag) && GameManager.instance.hasFood && GameManager.instance.activeFoodID == GameManager.instance.currentStep)
            {
                if (audioSource != null && successSound != null) audioSource.PlayOneShot(successSound);
                UpdateFoodVisual(GameManager.instance.activeFoodID - 1, false);
                GameManager.instance.hasFood = false;
                GameManager.instance.currentStep++;
                if (GameManager.instance.currentStep > 3) GameManager.instance.Win();
            }
            else
            {
                if (audioSource != null && failSound != null) audioSource.PlayOneShot(failSound);
                HandleWrongMove();
            }
        }
        else if (other.CompareTag("Trap"))
        {
            if (audioSource != null && failSound != null) audioSource.PlayOneShot(failSound);
            HandleWrongMove();
        }
    }

    void UpdateFoodVisual(int index, bool state)
    {
        foreach (var v in carriedFoodVisuals) v.SetActive(false);
        if (state && index >= 0 && index < carriedFoodVisuals.Length)
            carriedFoodVisuals[index].SetActive(true);
    }

    void HandleWrongMove()
    {
        StopAllCoroutines();
        isMoving = false;
        anim.SetBool("isMoving", false);
        transform.position = startPos;
        GameManager.instance.Lose();
    }
}