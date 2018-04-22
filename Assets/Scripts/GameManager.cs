using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TowerData {
    public bool nil;
    public int type;
    public Color color;
    public float damage;
    public float delay;
    public float charge;
}

public class GameManager : MonoBehaviour {

    TileManager tm;

    public GameObject placer;
    [Range(0.0f, 1.0f)]
    public float placerAlpha;
    public bool placing;
    Vector3 placingPos;

    public int Score { get; protected set; }
    public int Cash { get; protected set; }
    public int Health { get; protected set; }

    public TowerData[] towers;
    TowerData[] next = new TowerData[2];

    public GameObject towerPrefab;

    int chanceOfSingle = 1;

    Match3 match3;


    void Start () {
        tm = this.GetComponent<TileManager>();
        match3 = FindObjectOfType<Match3>();
        Next();
    }


    void Update () {
        placer.SetActive(placing);

        if (placing) {
            placingPos  = tm.HoveringTilePos(placingPos);
            placer.transform.position = placingPos;

            bool canPlace = tm.CanPlaceTileAtPosition(placingPos, next[1].nil ? 1 : 2);
            canPlace &= true; // CASH
            TogglePlacerStatus(canPlace);

            if (canPlace && Input.GetMouseButtonDown(0)) {
                Place();
                Next();
             }

        }
	}

    bool placerStatus = false;
    void TogglePlacerStatus(bool show) {
        if (placerStatus == show) return;
        if (placer.activeSelf == false) return;

        for (int i = 0; i < placer.transform.childCount; i++) {
            if (i > 1) break;

            Transform t = placer.transform.GetChild(i);
            if (next[i].nil) {
                t.gameObject.SetActive(false);
                break;
            }
            t.gameObject.SetActive(true);

            SpriteRenderer placerGem = t.transform.Find("Gem").GetComponent<SpriteRenderer>();
            placerGem.color = next[i].color;

            SpriteRenderer placerTower = t.transform.Find("Tower").GetComponent<SpriteRenderer>();
            placerTower.color = show ? new Color(1.0f, 1.0f, 1.0f, placerAlpha) : new Color(1.0f, 0.0f, 0.0f, placerAlpha);

            SpriteRenderer placerRadius = t.transform.Find("Radius").GetComponent<SpriteRenderer>();
            placerRadius.enabled = show;
        }

        placerStatus = show;
    }


    void Place() {
        Vector3 posA, posB;
        int cost = 0;

        // first tower
        posA = tm.PlaceTowerAtMousePosition(0);
        SpawnCrystal(posA, next[0]);
        cost++;

        // second tower
        if (!next[1].nil) {
            posB = tm.PlaceTowerBellowMousePosition(0);
            SpawnCrystal(posB, next[1]);
            cost++;

            UpgradeTowers(match3.CheckFrom(tm.WorldToLevel(posB)));
        }

        match3.CheckFrom(tm.WorldToLevel(posA));

        // prepare next
        Next();
        

        // substract cash
        Cash -= cost;
    }


    int UpgradeTowers(List<Tower> towers) {
        if (towers == null) return 0;
        Debug.Assert(towers.Count >= 3);

        // save first for upgrade
        Tower up = towers[0];

        up.Upgrade();
        tm.PlaceTowerAtPosition(up.level, up.transform.position);

        foreach (Tower t in towers) {
            if (t == up) continue;

            t.Collapse();
            tm.ClearSpot(t.transform.position);
        }

        int count = towers.Count;

        return count + UpgradeTowers(match3.CheckFrom(tm.WorldToLevel(up.transform.position)));
    }


    void SpawnCrystal(Vector3 pos, TowerData td) {
        GameObject tower = Instantiate(towerPrefab, pos, Quaternion.identity);
        Tower t = tower.GetComponent<Tower>();
        Vector2Int p = tm.WorldToLevel(pos);
        t.SetProperties(td, p.x, p.y);

        match3.AddTower(t);
    }


    void Next() {
        next[0] = towers[Random.Range(0, towers.Length)];

        if (Random.Range(0, chanceOfSingle) <= 1) {
            next[1] = new TowerData { nil = true };
            chanceOfSingle++;
        } else {
            next[1] = towers[Random.Range(0, towers.Length)];
        }
    }

 }
