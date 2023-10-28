using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{
    public Text leaderboardText;
    private List<float> scores = new List<float>();
    private int maxScoresToDisplay = 10;

    void Start()
    {
        leaderboardText = GetComponent<Text>();
    }

    public void AddScore(float time)
    {
        scores.Add(time);
        scores.Sort();
        if (scores.Count > maxScoresToDisplay)
        {
            scores.RemoveAt(scores.Count - 1);
        }
        UpdateLeaderboardUI();
    }

    void UpdateLeaderboardUI()
    {
        leaderboardText.text = "Leaderboard:\n";
        for (int i = 0; i < scores.Count; i++)
        {
            leaderboardText.text += (i + 1) + ": " + FormatTime(scores[i]) + "\n";
        }
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        float milliseconds = (time * 1000) % 1000;

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
