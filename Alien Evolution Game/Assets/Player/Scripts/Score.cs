using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score = 0;
    float timer = 0;
    public TextMeshProUGUI scoreText;
    void Start()
    {
        
    }
    void Update()
    {
        scoreText.text = "Score: " + score.ToString("F0");
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            score += 1;
            timer = 0;
        }
    }
}
