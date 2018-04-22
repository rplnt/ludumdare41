using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    TileManager tm;

    public GameObject placer;
    [Range(0.0f, 1.0f)]
    public float placerAlpha;
    SpriteRenderer placerTower;
    SpriteRenderer placerGem;
    SpriteRenderer placerRadius;
    public bool placing;
    Vector3 placingPos;

    public int Score { get; protected set; }
    public int Cash { get; protected set; }
    public int Health { get; protected set; }

    public Color[] colors;
    Color[] next = new Color[2];


    void Start () {
        tm = this.GetComponent<TileManager>();
        placerTower = placer.transform.Find("Tower").GetComponent<SpriteRenderer>();
        placerRadius = placer.transform.Find("Radius").GetComponent<SpriteRenderer>();
        placer.SetActive(false);
    }


    void Update () {
        placer.SetActive(placing);

        if (placing) {
            placingPos  = tm.HoveringTilePos(placingPos);
            placer.transform.position = placingPos;

            if (tm.CanPlaceTileAtCursor(next[1] == null ? 1 : 2)) {
                placerRadius.enabled = true;
                placerTower.color = new Color(1.0f, 1.0f, 1.0f, placerAlpha);

                if (Input.GetMouseButtonDown(0)) {
                    Place();
                    Next();
                }

            } else {
                placerRadius.enabled = false;
                placerTower.color = new Color(1.0f, 0.0f, 0.0f, placerAlpha); 
            }
        }
	}


    void Place() {
        tm.PlaceTowerAtMousePosition(0);

        // spawn tower crystal(s)

        // prepare next

        // substract cash
    }


    void Next() {
        next[0] = colors[Random.Range(0, colors.Length)];
    }


}
