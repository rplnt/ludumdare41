using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [Header("Next")]
    public Image gemA, gemB;
    public GameObject B;

    [Header("Header")]
    public GameObject top;

    [Header("...")]
    public GameObject lifebar;
    public GameObject gameOver;
    public GameObject mainMenu;
    public Text cash;
    public Text score;
    public Text wave;
    public Text finalScore;
    public Button GO;

    private void Start() {
        gameOver.SetActive(false);
        mainMenu.SetActive(true);
    }


    public void UpdateNext(TowerData a, TowerData b) {
        gemA.color = a.color;
        gemB.color = b.color;
        B.SetActive(b.nil == false);
    }

    public void ShowTopPanel() {
        top.SetActive(true);
    }

    public void HideTopPanel() {
        top.SetActive(false);
    }

    public void UpdateHealth(int hp) {
        lifebar.transform.GetChild(0).gameObject.SetActive(hp > 2);
        lifebar.transform.GetChild(1).gameObject.SetActive(hp > 1);
        lifebar.transform.GetChild(2).gameObject.SetActive(hp > 0);
    }

    public void ShowGameOver(int score) {
        gameOver.SetActive(true);
        finalScore.text = string.Format("Score: <b>{0}</b>", score);
    }

    public void MainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HideMainMenu() {
        mainMenu.SetActive(false);
    }

    public void UpdateCash(int c) {
        cash.text = c.ToString();
    }

    public void UpdateScore(int s) {
        score.text = s.ToString();
    }

    public void NextWave(int w, int total) {
        wave.text = string.Format("{0}/{1}", w, total);
    }

    public void DisableGOButton() {
        GO.interactable = false;
    }

    public void EnableGOButton() {
        GO.interactable = true;
    }
}
