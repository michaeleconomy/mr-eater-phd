using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public Player player;
    public Text scoreText;

    public static bool paused = true;

    private int score;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        NewGame();
    }


    public void NewGame() {
        score = 0;
        paused = false;
        Tiler.instance.Initialize();
    }

    public void Score(int points) {
        score += points;
        scoreText.text = "" + score;
    }
}