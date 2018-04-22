using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAnimator : MonoBehaviour {
    public float speed;

    private void Update() {
        this.transform.Rotate(Vector3.back, speed * Time.deltaTime);
    }
}
