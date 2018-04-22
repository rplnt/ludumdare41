using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    TileManager tm;

    public GameObject placer;
    public bool placing;
    Vector3 placingPos;


    void Start () {
        placing = false;
        tm = this.GetComponent<TileManager>();
        placer.SetActive(false);
	}


    void Update () {
        placer.SetActive(placing);

        if (placing) {
            placer.transform.position = tm.HoveringTilePos(placingPos);
        }
	}



}
