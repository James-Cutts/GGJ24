using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PointSystem : MonoBehaviour
{
    public int happiness = 0;
    public int maxHappiness = 100;
    public TextMeshProUGUI happyText;

    private float timer = 0f;
    public TextMeshProUGUI timerText;
    public bool isLevelComplete = false;
    private void Update()
    {
        if (!isLevelComplete)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }

        if (happiness >= maxHappiness)
        {
            LevelComplete();
        }
        happyText.text = happiness + "%";
    }
    public void IncreaseScore()
    {
        happiness = happiness + 2;
    }

    public void LevelComplete()
    {
        isLevelComplete = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int milliseconds = Mathf.FloorToInt((timer * 1000f) % 100f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

}
