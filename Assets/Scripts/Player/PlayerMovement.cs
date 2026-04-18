using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float minPathDistance = 0.1f; // min dist between path points

    [Header("References")]
    public PathDrawer pathDrawer;

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
            pathDrawer.StartPath(transform.position);
        }

        if (Input.GetMouseButton(0) && pathDrawer.IsDrawing)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;
            pathDrawer.AddPoint(worldPos);
        }

        if (Input.GetMouseButtonUp(0) && pathDrawer.IsDrawing)
        {
            List<Vector3> path = pathDrawer.EndPath();
            if (path.Count > 1)
                StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator FollowPath(List<Vector3> path)
    {
        isMoving = true;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 target = path[i];

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
        }

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