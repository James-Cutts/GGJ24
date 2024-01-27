using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public int score = 0;
    public int happiness = 0;
    public int maxHappiness = 100;

    private void Update()
    {
       if (happiness >= maxHappiness)
        {
            LevelComplete();
        }
    }
    public void IncreaseScore()
    {
        score = score + 100;
        happiness = happiness + 2;
    }

    public void ResetScore() 
    { 
        happiness = 0;
    }

    public void LevelComplete()
    {
        
    }
}
