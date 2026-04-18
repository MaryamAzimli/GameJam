using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxDragDistance = 5f;

    [Header("References")]
    public VectorArrow vectorArrow;

    private Vector3 dragStart;
    private bool isDragging = false;
    private bool isMoving = false;

    void Update()
    {
        if (!isMoving)
            HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStart = transform.position;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;
            vectorArrow.ShowArrow(dragStart, worldPos);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            vectorArrow.HideArrow();

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;
            Vector3 target = vectorArrow.GetClampedTarget(dragStart, worldPos);

            StartCoroutine(MoveToTarget(target));
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
            FindObjectOfType<GameManager>().Win();
        if (other.CompareTag("Trap"))
            FindObjectOfType<GameManager>().Lose();
    }
}