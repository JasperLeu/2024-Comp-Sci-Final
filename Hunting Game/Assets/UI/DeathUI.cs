using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    public bool isDead;
    public Color targetColor;
    public float fadeTime;
    public int score;
    public TextMeshProUGUI scoreText;   
    float fadeTimer = 0;
    public GameObject[] elements;

    void Start()
    {
        foreach (GameObject element in elements)
        {
            element.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            if (fadeTimer < fadeTime)
            {
                fadeTimer += Time.deltaTime;
                targetColor.a = Mathf.Lerp(0, 1, fadeTimer / fadeTime);
                GetComponent<Image>().color = targetColor;
            }
            else
            {
                ActivateScreen();
            }
        }
    }
    void ActivateScreen()
    {
        scoreText.text = "Score: " + score.ToString("F0");
        foreach (GameObject element in elements)
        {
            element.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
