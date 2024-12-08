using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSuggestionManager : MonoBehaviour
{
    public static ButtonSuggestionManager Instance { get; private set; }

    // Suggest button show
    public GameObject goSuggestButton_E;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowButtonSuggestion(string buttonName)
    { 
        switch (buttonName)
        {
            case "E":
                goSuggestButton_E.SetActive(true);
                break;
        }
    }

    public void HideButtonSuggestion(string buttonName)
    { 
        switch(buttonName)
        {
            case "E":
                goSuggestButton_E.SetActive(false);
                break;
        }
    }
}
