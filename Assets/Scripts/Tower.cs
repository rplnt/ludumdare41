using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        Debug.Log("COLLISISON " + other.transform.name);
    }
}
