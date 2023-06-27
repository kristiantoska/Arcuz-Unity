using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour {
    private static float TopDirection = 0;
    private static float RightDirection = 1;
    private static float BottomDirection = 2;

    public SpriteRenderer sp;
    public Rigidbody2D rb;
    public Animator animator;

    public float moveSpeed = 5f;
    public float horizontal;
    public float vertical;

    private bool isFacingRight = true;

    void Update () {
        horizontal = Input.GetAxisRaw ("Horizontal");
        vertical = Input.GetAxisRaw ("Vertical");

        animator.SetFloat ("Horizontal", horizontal);
        animator.SetFloat ("Vertical", vertical);
        animator.SetFloat ("AbsMove", Mathf.Abs (horizontal) + Mathf.Abs (vertical));

        if (vertical > 0) {
            animator.SetFloat ("LastDirection", TopDirection);
        } else if (vertical < 0) {
            animator.SetFloat ("LastDirection", BottomDirection);
        } else if (Mathf.Abs (horizontal) > 0) {
            animator.SetFloat ("LastDirection", RightDirection);
        }

        if (horizontal != 0) {
            isFacingRight = horizontal >= 0 ? true : false;
            sp.flipX = !isFacingRight;
        }

        if (Input.GetKeyDown (KeyCode.J)) {
            animator.SetTrigger ("SwordAttack");
        }
    }

    void FixedUpdate () {
        Vector2 moveVector = new Vector2 (horizontal, vertical);
        moveVector = Vector2.ClampMagnitude (moveVector, 1f);

        rb.MovePosition (rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }
}