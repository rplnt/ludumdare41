using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    bool placerStatus = false;

    public int Score { get; protected set; }
    public int Cash { get; protected set; }
    public int Health { get; protected set; }

    public TowerData[] towers;
    TowerData[] next = new TowerData[2];
    TowerData[] nextnext = new TowerData[2];

    public GameObject towerPrefab;

    int chanceOfSingle = 1;

    Match3 match3;
    UIManager ui;

    public void RestoreHealth() {
        Health = 3;
    }

    public void Intermission() {
        placing = true;
        ui.ShowTopPanel();
    }

    public void RunGame() {
        Health = 3;
        ui.UpdateHealth(Health);
        placing = false;
    }

    public void Damage() {
        Health -= 1;
        ui.UpdateHealth(Health);
        if (Health <= 0) {
            placing = false;
            ui.ShowGameOver(Score);
        }
    }

    public void Win() {

    }


    void Start () {
        tm = this.GetComponent<TileManager>();
        match3 = FindObjectOfType<Match3>();
        ui = FindObjectOfType<UIManager>();

        nextnext[0] = towers[0];
        nextnext[1] = new TowerData { nil = true };
        Next();

        Cash = 5;
        ui.UpdateCash(Cash);
        ui.UpdateScore(Score);
    }


    void Update () {
        placer.SetActive(placing);

        if (placing) {
            placingPos  = tm.HoveringTilePos(placingPos);
            placer.transform.position = placingPos;

            bool canPlace = tm.CanPlaceTileAtPosition(placingPos, next[1].nil ? 1 : 2);
            canPlace &= Cash >= (next[1].nil ? 1 : 2);

            if (placerStatus != canPlace) {
                TogglePlacerStatus(canPlace);
            }

            if (canPlace && Input.GetMouseButtonDown(0)) {
                EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
                if (!es.IsPointerOverGameObject()) {
                    Place();
                    Next();
                }
            }

            if (next[1].nil == false && Input.GetAxis("Mouse ScrollWheel") != 0f) {
                TowerData swap = next[0];
                next[0] = next[1];
                next[1] = swap;
                TogglePlacerStatus(canPlace);
            }

        }
	}


    void TogglePlacerStatus(bool show) {
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

    public void CountKill() {
        Score++;
        Cash++;
        ui.UpdateCash(Cash);
        ui.UpdateScore(Score);
    }


    void Place() {
        Vector3 posA, posB;
        int up;

        // first tower
        Debug.Log("PLACING TOWER A");
        posA = tm.PlaceTowerAtMousePosition(0);
        Debug.Log("PLACED AT " + posA);
        SpawnCrystal(posA, next[0]);
        Cash--;

        // second tower
        if (!next[1].nil && Cash > 0) {
            Debug.Log("PLACING TOWER B");
            posB = tm.PlaceTowerBellowMousePosition(0);
            Debug.Log("PLACED AT " + posB);
            SpawnCrystal(posB, next[1]);
            Cash--;

            up = UpgradeTowers(match3.CheckFrom(tm.WorldToLevel(posB)));
            Score += up;
            Cash += up / 3;
        }

        up = UpgradeTowers(match3.CheckFrom(tm.WorldToLevel(posA)));
        Score += up;
        Cash += up / 3;

        ui.UpdateCash(Cash);
        ui.UpdateScore(Score);
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
        next[0] = nextnext[0];
        next[1] = nextnext[1];

        nextnext[0] = towers[Random.Range(0, towers.Length)];
        if (Random.Range(0, chanceOfSingle) <= 1) {
            nextnext[1] = new TowerData { nil = true };
            chanceOfSingle++;
        } else {
            nextnext[1] = towers[Random.Range(0, towers.Length)];
        }

        ui.UpdateNext(nextnext[0], nextnext[1]);
    }

 }
