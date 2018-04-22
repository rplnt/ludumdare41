using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public Tilemap level;

    public Vector2Int limitsLow, limitsHigh;

    public Tile grass;
    public Tile[] towers;

    void Start () {
    }
	

    void Update () {
        
    }


    public Vector3 HoveringTilePos(Vector3 currentPos) {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = level.WorldToCell(cursorPos);

        if (tilePos.x < limitsLow.x || tilePos.x > limitsHigh.x || tilePos.y < limitsLow.y || tilePos.y > limitsHigh.y) {
            return currentPos;
        }

        return level.GetCellCenterWorld(tilePos);
    }


    public bool CanPlaceTileAtPosition(Vector3 pos, int count=1) {
        Vector3Int tilePos = level.WorldToCell(pos);

        if (count == 1) {
            return CanPlaceTileAt(tilePos);
        } else {
            return CanPlaceTilesAt(tilePos);
        }
    }


    public bool CanPlaceTileAt(Vector3Int pos) {
        TileBase t = level.GetTile(pos);

        return t == null ? false : t.name == "grass";
    }

    public bool CanPlaceTilesAt(Vector3Int pos) {
        return CanPlaceTileAt(pos) && CanPlaceTileAt(new Vector3Int(pos.x, pos.y - 1, pos.z));
    }


    public Vector3 PlaceTowerAt(int upgrade, Vector3Int pos) {
        level.SetTile(pos, towers[upgrade]);

        return level.GetCellCenterWorld(pos);
    }

    public Vector3 PlaceTowerAtPosition(int upgrade, Vector3 pos) {
        Vector3Int tilePos = level.WorldToCell(pos);
        return PlaceTowerAt(upgrade, tilePos);
    }

    public Vector3 PlaceTowerAtMousePosition(int upgrade) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = level.WorldToCell(pos);

        return PlaceTowerAt(upgrade, tilePos);
    }

    public Vector3 PlaceTowerBellowMousePosition(int upgrade) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = level.WorldToCell(pos);

        return PlaceTowerAt(upgrade, new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z));
    }

    public Vector2Int WorldToLevel(Vector3 pos) {
        Vector3Int p = level.WorldToCell(pos);
        return new Vector2Int(p.x, p.y);
    }

    public void ClearSpot(Vector3 pos) {
        Vector3Int p = level.WorldToCell(pos);
        level.SetTile(p, grass);
    }
}