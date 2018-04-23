using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    public float delay;
    public float damage;
    public float chargingSpeed;
    public Color color;

    public int type;
    public int level;

    public Sprite[] gems;

    public int x, y;

    float radius;

    Queue<Unit> targets;

    float lastShot = 0.0f;
    LineRenderer lr;

    public AnimationCurve laserCharge;


    void Start() {
        targets = new Queue<Unit>();
        radius = transform.GetComponent<CircleCollider2D>().radius;

        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        lr.useWorldSpace = true;
        lr.SetPosition(0, new Vector3(transform.position.x, transform.position.y + 0.25f));
        lr.startColor = color;
        lr.endColor = color;
    }


    public void SetProperties(TowerData data, int posx, int posy) {
        damage = data.damage;
        delay = data.delay;
        chargingSpeed = data.charge;
        color = data.color;
        type = data.type;
        level = 0;

        x = posx;
        y = posy;

        GetComponentInChildren<SpriteRenderer>().color = color;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Enemy")) return;
        Unit u = collision.attachedRigidbody.GetComponent<Unit>();
        targets.Enqueue(u);
    }

    private void Update() {
        if (Time.time > lastShot + delay) {
            lastShot = Time.time;
            Fire();
        }
    }

    void Fire() {
        if (targets.Count == 0) return;

        Unit target = targets.Peek();
        if (target == null || target.gameObject.activeSelf == false || Vector3.Distance(transform.position, target.transform.position) > radius) {
            targets.Dequeue();
            return;
        }

        StartCoroutine(Laser(target));

        // score, money, etc
    }

    public void Upgrade() {
        level++;
        damage *= 1.5f;
        chargingSpeed *= 1.2f;
        delay += 0.1f;

        transform.GetComponent<CircleCollider2D>().radius = transform.GetComponent<CircleCollider2D>().radius * 1.5f;
        GetComponentInChildren<SpriteRenderer>().sprite = gems[level];
    }


    public void Collapse() {
        Destroy(gameObject);
    }


    IEnumerator Laser(Unit target) {
        float elapsed = 0.0f;

        lr.startWidth = 0.0f;
        lr.endWidth = 0.0f;
        lr.enabled = true;

        while (elapsed < chargingSpeed) {
            elapsed += Time.deltaTime;

            yield return null;
            if (target == null || target.gameObject.activeSelf == false) {
                lr.enabled = false;
                yield break;
            }

            lr.SetPosition(1, target.transform.position);
            float width = laserCharge.Evaluate(elapsed / chargingSpeed) * (damage / 10.0f);
            lr.startWidth = width;
            lr.endWidth = width;
        }

        bool kill = target.TakeDamage(damage);
        lr.enabled = false;

        yield return null;
    }



}
