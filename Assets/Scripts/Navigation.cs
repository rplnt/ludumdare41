using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {

    List<Transform> waypoints;

    private void Start() {
        waypoints = new List<Transform>();
        LoadWaypoints();
    }

    void LoadWaypoints() {
        foreach(Transform wp in transform) {
            //Debug.Log("Loaded waypoint: " + wp.name);
            waypoints.Add(wp);
        }

        Debug.Log("Total: " + waypoints.Count);
    }


    public Transform GetWaypoint(int index) {
        if (index >= waypoints.Count) return null;

        return waypoints[index];
    }
}
