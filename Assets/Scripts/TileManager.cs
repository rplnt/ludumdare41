using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public GameObject map;
    Tilemap level;

    public Vector2Int limitsLow, limitsHigh;

    public Tile grass;
    public Tile[] towers;
    public Color[] colors;
    int c = 0;


    void Start () {
        level = map.transform.Find("Base").GetComponent<Tilemap>();
    }
	

    void Update () {
        
        if (Input.GetMouseButtonDown(0)) {

            PlaceTowerAtMousePosition(0, colors[c % colors.Length]);
            c++;

        }

    }


    public Vector3 HoveringTilePos(Vector3 currentPos) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = level.WorldToCell(pos);

        if (tilePos.x < limitsLow.x || tilePos.x > limitsHigh.x || tilePos.y < limitsLow.y || tilePos.y > limitsHigh.y) {
            return currentPos;
        }

        return level.GetCellCenterWorld(tilePos);
    }


    public Vector3 PlaceTowerAt(int upgrade, Color color, Vector3Int pos) {
        level.SetTile(pos, towers[upgrade]);

        return level.GetCellCenterWorld(pos);
    }

    public Vector3 PlaceTowerAtMousePosition(int upgrade, Color color) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = level.WorldToCell(pos);

        return PlaceTowerAt(upgrade, color, tilePos);
    }
}