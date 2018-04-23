using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject[] units;

    public string[] waves;
    public float waveDelay;
    public float unitDelay;

    int currentWave = 0;
    int currentUnit = 0;
    int[] wave;

    bool intermission = true;
    float lastUnitSpawn = 0.0f;

    GameManager gm;
    UIManager ui;

    int totalUnits = 0;

    private void Start() {
        gm = FindObjectOfType<GameManager>();
        ui = FindObjectOfType<UIManager>();
        FindObjectOfType<UIManager>().NextWave(currentWave + 1, waves.Length);
    }
    
    private void Update() {
        if (intermission) return;
        if (wave == null) return;

        if (Time.time > lastUnitSpawn + unitDelay) {
            if (!SpawnNext()) {
                intermission = true;
            } else {
                lastUnitSpawn = Time.time;
            }
        }
    }


    public void NextWave() {
        ui.NextWave(currentWave + 1, waves.Length);
        ui.DisableGOButton();
        intermission = false;
        gm.RestoreHealth();
        wave = LoadWave();
        currentWave++;
        if (currentWave > waves.Length) {
            gm.Win();
        }
    }


    bool SpawnNext() {
        if (wave == null) return false;
        if (currentUnit >= wave.Length) return false;

        //Debug.Log(String.Format("Spawning unit {0} of type '{1}' from wave {2}", currentUnit, wave[currentUnit], currentWave));
        GameObject u =  Instantiate(units[wave[currentUnit]], transform.position, transform.rotation);

        BallAnimator ba = u.GetComponentInChildren<BallAnimator>();
        Unit unit = u.GetComponent<Unit>();

        ba.speed += (currentWave * 3);
        unit.speed += (currentWave * (1 / 13.3f));

        unit.navigator = GetComponent<Navigation>();
        unit.name = String.Format("Ball_{0}_{1}", currentWave, currentUnit);

        currentUnit++;
        totalUnits++;
        return true;
    }

    public void SubUnit() {
        totalUnits--;
        if (totalUnits == 0) {
            ui.EnableGOButton();
            gm.Intermission();
        }
    }


    int[] LoadWave() {
        if (currentWave >= waves.Length) return null;
        string[] u = waves[currentWave].Split(',');
        int[] w = Array.ConvertAll(u, s => int.Parse(s));

        Debug.Log("Loaded wave " + currentWave + " with " + w.Length + " units.");
        currentUnit = 0;

        return w;
    }
}
