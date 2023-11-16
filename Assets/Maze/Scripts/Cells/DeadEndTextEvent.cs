using System.Collections;
using UnityEngine;
using TMPro;

public class DeadEndTextEvent : MonoBehaviour
{
    public CellLookAtService cellLookAtService;
    public PathProgressService pathProgressService; 
    public TMP_Text quoteText;

    public float displayDuration = 5f; 
    public float fadeDuration = 2f;
    public float minimumLookAwayDuration = 1f;

    private string[] quotes = {
        "You shouldn't be here...",
        "Something's lurking...",
        "Can you feel the presence?",
        "'. . .'",
        "",
        "leave",
        "go back",
        "you're not welcome",
        "you're not supposed to be here",
        "death awaits you",
        "you're not safe",
        "shhhh"
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
