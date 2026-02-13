using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText1;
    [SerializeField] TextMeshProUGUI timerText2;
    float elapsedTime;

    bool timerOn;

    void Update()
    {
        if (timerOn)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText1.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText2.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
    }

    public void GameStart()
    {
        timerOn = true;
    }
    public void GameEnd()
    {
        timerOn = false;
    }
}
