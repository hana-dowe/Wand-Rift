using System.Collections;
using UnityEngine;
using TMPro;

public class DeadEndTextEvent : MonoBehaviour
{
    public CellLookAtService cellLookAtService; 
    public TMP_Text quoteText;

    public float displayDuration = 5f; 
    public float fadeDuration = 2f;
    public float minimumLookAwayDuration = 1f; // time player should look away before new message can show

    private string[] quotes = {
        "Don't look behind",
        "Something's lurking...",
        "Can you feel the presence?",
    };

    private bool isQuoteDisplayed = false;
    private float lastLookedAtDeadEnd;
    private void Update()
    {
        if (!isQuoteDisplayed && Time.time - lastLookedAtDeadEnd > minimumLookAwayDuration && cellLookAtService.IsLookingAtDeadEnd()) // 50% chance to display quote
        {
            DisplayRandomQuote();
        }
        if (!cellLookAtService.IsLookingAtDeadEnd())
        {
            quoteText.text = "";
            isQuoteDisplayed = false;
            lastLookedAtDeadEnd = Time.time;
        }
    }

    public void DisplayRandomQuote()
    {
        isQuoteDisplayed = true;
        quoteText.text = GetRandomQuote();
        //fade in
        //fade out
    }

    private string GetRandomQuote()
    {
        return quotes[Random.Range(0, quotes.Length)];
    }
}
