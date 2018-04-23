using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public float health;
    public float speed;

    int targetIndex = 0;
    Transform target;

    [HideInInspector]
    public Navigation navigator;

    private void Start() {
        target = transform;
    }

    private void Update() {
        if (Vector3.Distance(transform.position, target.position) < Vector3.kEpsilon) {
            target = navigator.GetWaypoint(targetIndex++);
        }

        if (target == null) {
            Finish();
            return;
        }

        Move();
    }

    void Finish() {
        FindObjectOfType<GameManager>().Damage();
        FindObjectOfType<EnemySpawner>().SubUnit();
        Destroy(gameObject);
    }

    void Move() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public bool TakeDamage(float damage) {
        health -= damage;
        if (health <= 0.0f) {
            Die();
            return true;
        }

        return false;
    }

    void Die() {
        // play explosion
        FindObjectOfType<GameManager>().CountKill();
        FindObjectOfType<EnemySpawner>().SubUnit();
        Destroy(gameObject);
    }
}
