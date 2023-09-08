using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public Animator animator;
    public float maxHealth = 3;

    private float health;

    void Start () {
        health = maxHealth;
    }

    void Update () {

    }

    public void OnGetAttacked (float damage) {
        health -= damage;
        animator.SetTrigger ("Hit");

        if (health <= 0) {
            Die ();
        }
    }

    void Die () {
        animator.SetBool ("Dead", true);
        GetComponent<CircleCollider2D> ().enabled = false;
        this.enabled = false;
    }
}