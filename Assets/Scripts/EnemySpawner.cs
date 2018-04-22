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

    float lastUnitSpawn = 0.0f;
    float lastWaveFinished = 0.0f;

    private void Start() {
        wave = LoadWave();
    }


    private void Update() {
        if (wave == null) return;

        if (Time.time > lastWaveFinished + waveDelay && Time.time > lastUnitSpawn + unitDelay) {
            if (!SpawnNext()) {
                lastWaveFinished = Time.time;
                currentWave++;
                wave = LoadWave();
            } else {
                lastUnitSpawn = Time.time;
            }
        }

    }


    bool SpawnNext() {
        if (wave == null) return false;
        if (currentUnit >= wave.Length) return false;

        Debug.Log(String.Format("Spawning unit {0} of type '{1}' from wave {2}", currentUnit, wave[currentUnit], currentWave));
        GameObject u =  Instantiate(units[wave[currentUnit]], transform.position, transform.rotation);

        Unit unit = u.GetComponent<Unit>();
        unit.navigator = this.GetComponent<Navigation>();
        unit.name = String.Format("Ball_{0}_{1}", currentWave, currentUnit);

        currentUnit++;
        return true;
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
