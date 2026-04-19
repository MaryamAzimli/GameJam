using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private Vector3 lastPosition;

    void Awake()
    {
        anim = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        float velocityX = (transform.position.x - lastPosition.x) / Time.deltaTime;
        float velocityY = (transform.position.y - lastPosition.y) / Time.deltaTime;
        lastPosition = transform.position;

        if (Mathf.Abs(velocityX) < 0.1f) velocityX = 0f;
        if (Mathf.Abs(velocityY) < 0.1f) velocityY = 0f;

        // If moving more horizontally, zero out Y so horizontal animation wins
        if (Mathf.Abs(velocityX) >= Mathf.Abs(velocityY))
            velocityY = 0f;
        else
            velocityX = 0f;

        anim.SetFloat("velocityX", velocityX);
        anim.SetFloat("velocityY", velocityY);
    }
}