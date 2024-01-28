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
    private void Update()
    {
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

    public void ResetScore() 
    { 
        happiness = 0;
    }

    public void LevelComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
