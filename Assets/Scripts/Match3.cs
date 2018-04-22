using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour {
    Tower[,] towers = new Tower[20, 16];

    List<Tower> selector;

    private void Start() {
        selector = new List<Tower>();
    }


    Vector2Int LocalPos(Vector2Int pos) {
        return new Vector2Int(pos.x + 20 / 2, pos.y + 16 / 2);
    }

    Vector2Int LocalPos(int x, int y) {
        return new Vector2Int(x + 20 / 2, y + 16 / 2);
    }

    public void AddTower(Tower t) {
        Vector2Int pos = LocalPos(t.x, t.y);
        if (towers[pos.x, pos.y] != null) {
            Debug.LogError("Tower already at position " + pos);
            return;
        }

        towers[pos.x, pos.y] = t;
    }

    public List<Tower> CheckFrom(Vector2Int pos) {
        pos = LocalPos(pos);

        Debug.Assert(!OutOfBounds(pos.x, pos.y));

        selector.Clear();
        Tower match = towers[pos.x, pos.y];
        if (match == null) return null;
        if (match.level >= 2) return null;

        int count = Check(pos.x, pos.y, match);        
        if (count >= 3) {
            Debug.Log("MATCH" + count + " at " + pos);
            foreach(Tower t in selector) {
                if (t == match) continue;
                Vector2Int lp = LocalPos(t.x, t.y);
                towers[lp.x, lp.y] = null;
            }
            return selector;
        }

        return null;
    }


    bool OutOfBounds(int x, int y) {
        return x < 0 || x >= 20 || y < 0 || x >= 16;
    }

    int Check(int x, int y, Tower match) {
        if (match == null) return 0;
        if (OutOfBounds(x, y)) return 0;
        if (towers[x, y] == null) return 0;
        if (towers[x, y].type != match.type || towers[x, y].level > match.level) return 0;
        if (selector.Contains(towers[x, y])) return 0;

        selector.Add(towers[x, y]);

        return Check(x + 1, y     , match) +
               Check(x    , y  + 1, match) +
               Check(x - 1, y     , match) +
               Check(x    , y  - 1, match) + 1;
    }
}
