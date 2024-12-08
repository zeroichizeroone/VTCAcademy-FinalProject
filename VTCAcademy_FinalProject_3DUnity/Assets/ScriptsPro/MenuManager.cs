using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Assign these in the Inspector
    public Button startButton;
    public Button continueButton;
    public Button settingsButton;
    public Button exitButton;

    private void Start()
    {
        // Ensure all buttons are assigned
        if (startButton) startButton.onClick.AddListener(OnStartButtonClicked);
        if (continueButton) continueButton.onClick.AddListener(OnContinueButtonClicked);
        if (settingsButton) settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        if (exitButton) exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start Game clicked");
        //SceneManager.LoadScene("GameScene");
    }

    private void OnContinueButtonClicked()
    {
        Debug.Log("Continue Game clicked");
        LoadGame(); // Implement your logic here
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings clicked");
        OpenSettingsMenu(); // Implement your settings logic here
    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Game clicked");
        //Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Ensure it works in the Editor
#endif
    }

    private void LoadGame()
    {
        Debug.Log("Loading saved game...");
        // Add your load logic here
    }

    private void OpenSettingsMenu()
    {
        Debug.Log("Opening settings menu...");
        // Add your settings menu logic here
    }
}
