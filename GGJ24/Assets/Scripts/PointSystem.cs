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
    private bool isLevelComplete = false;

    private int currentLevelIndex;

    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

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
        happiness += 2;
    }

    public void LevelComplete()
    {
        isLevelComplete = true;
        SaveTime(timer, currentLevelIndex); // Save the time for the current level
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            // All levels are complete, you can display the saved times
            DisplaySavedTimes();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int milliseconds = Mathf.FloorToInt((timer * 1000f) % 100f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void SaveTime(float time, int levelIndex)
    {
        string key = "LevelTime_" + levelIndex;
        PlayerPrefs.SetFloat(key, time);
        PlayerPrefs.Save();
    }

    private void DisplaySavedTimes()
    {
        float totalTime = 0f;

        // Iterate through each level and add its saved time to totalTime
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string key = "LevelTime_" + i;
            float savedTime = PlayerPrefs.GetFloat(key, -1f); // -1f indicates no time saved
            if (savedTime >= 0)
            {
                Debug.Log("Level " + i + " Time: " + savedTime);
                totalTime += savedTime;
            }
        }

        // Display the total time
        int totalMinutes = Mathf.FloorToInt(totalTime / 60f);
        int totalSeconds = Mathf.FloorToInt(totalTime % 60f);
        int totalMilliseconds = Mathf.FloorToInt((totalTime * 1000f) % 100f);
        Debug.Log("Total Time: " + string.Format("{0:00}:{1:00}:{2:00}", totalMinutes, totalSeconds, totalMilliseconds));
    }

}
