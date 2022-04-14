using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStuff : MonoBehaviour
{
    public static UIStuff instance;

    public Text scoreText;
    public Text highscoreText;
    public Text timeText;

    public int score = 0;
    public int highscore = 0;
    public float time = 0;

    private void Awake()
    {
        instance = this;  
    }

    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = "SCORE: " +  score.ToString();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    void Update()
    {
        time += Time.deltaTime;
        int timeSec = Mathf.RoundToInt(time) % 60;
        int timeMin = Mathf.RoundToInt(time) / 60;
        if(timeSec < 10)
            timeText.text = "TIME: " + timeMin + ":0" + timeSec;
        else
            timeText.text = "TIME: " + timeMin + ":" + timeSec;
    }

    public void AddPoints(int amount)
    {
        score += amount;
        scoreText.text = "SCORE: " + score.ToString();
        if (highscore < score)
            PlayerPrefs.SetInt("highscore", score);
    }
}
