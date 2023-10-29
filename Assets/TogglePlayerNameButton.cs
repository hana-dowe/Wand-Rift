using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TogglePlayerNameButton : MonoBehaviour
{
    public GameObject PlayerNamePanel;
    public TextMeshProUGUI EnterNameText;
    public TMP_InputField NameInputField;
    public Button SubmitButton;
    void Start()
    {
        PlayerNamePanel.SetActive(true);
    }

    // Update is called once per frame
    void SubmitPlayerName()
    {
        string playerName = NameInputField.text;

        PlayerNamePanel.SetActive(false);
    }
}
