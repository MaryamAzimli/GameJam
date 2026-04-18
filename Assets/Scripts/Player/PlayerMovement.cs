using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxDragDistance = 5f;
    public float dragStartRadius = 0.8f;

    private bool isDragging = false;
    private bool isMoving = false;
    private Vector3 dragStartWorld;

    void Update()
    {
        if (!isMoving)
            HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;

            float dist = Vector3.Distance(worldPos, transform.position);
            if (dist <= dragStartRadius)
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

            if (dist > 0.1f)
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
            FindObjectOfType<GameManager>().Win();
        if (other.CompareTag("Trap"))
            FindObjectOfType<GameManager>().Lose();
    }
}