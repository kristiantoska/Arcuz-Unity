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

    private float horizontal;
    private float vertical;
    private bool isFacingRight = true;

    private Transform attackingPoint;
    public Transform attackPointDown;
    public Transform attackPointUp;
    public Transform attackPointRight;
    public Transform attackPointLeft;

    public LayerMask enemyLayers;
    public float attackRange;
    private bool attacking = false;

    void Update () {
        if (attacking) {
            return;
        }
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
            Attack ();
        }
    }

    void FixedUpdate () {
        if (attacking) {
            return;
        }
        Vector2 moveVector = new Vector2 (horizontal, vertical);
        moveVector = Vector2.ClampMagnitude (moveVector, 1f);

        rb.MovePosition (rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }

    void Attack () {
        animator.SetTrigger ("SwordAttack");
        attacking = true;
        Invoke ("AttackEnd", 0.5f);

        float lastDirection = animator.GetFloat ("LastDirection");
        if (lastDirection == TopDirection) {
            attackingPoint = attackPointUp;
        } else if (lastDirection == BottomDirection) {
            attackingPoint = attackPointDown;
        } else if (isFacingRight) {
            attackingPoint = attackPointRight;
        } else {
            attackingPoint = attackPointLeft;
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll (attackingPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies) {
            Debug.Log ("Hit enemy" + enemy.name);
        }
    }

    void AttackEnd () {
        attacking = false;
    }

    void OnDrawGizmos () {
        if (attackingPoint == null || !attacking) {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (attackingPoint.position, attackRange);
    }
}