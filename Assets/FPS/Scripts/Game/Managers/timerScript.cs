using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerScript : MonoBehaviour
{
    public Text timerText;
    private float startTime;
    private bool gameRunning = false;

    void Start()
    {
        timerText = GetComponent<Text>():
    }

    // Update is called once per frame
    void Update()
    {
       if (gameRunning)
       {
        float currentTime = Time.time - startTime;
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        int milliseconds = (int)((currentTime * 1000) % 1000);

        timerText.text = string.Format("{0:00}:{1:00}:{2.000}", minutes, seconds, milliseconds);
       } 
    }

    public void StartTimer()
    {
        startTime = Time.time;
        gameRunning = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    public float GetElapsedTime()
    {
        return Time.time - startTime;
    }
}
