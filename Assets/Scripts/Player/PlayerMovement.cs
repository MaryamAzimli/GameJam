using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 startTouch;
    Vector2 endTouch;

    public float moveDistance = 0.5f;

void Update()
{
    Debug.Log("Running");
    HandleInput();
}

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endTouch = Input.mousePosition;

            Vector2 swipe = endTouch - startTouch;

            if (swipe.magnitude < 50f) return; // ignore tiny swipes

            swipe.Normalize();

            MovePlayer(swipe);
        }
    }

    void MovePlayer(Vector2 direction)
    {
        Vector3 move = new Vector3(direction.x, direction.y, 0f);
        transform.position += move * moveDistance;
    }
}