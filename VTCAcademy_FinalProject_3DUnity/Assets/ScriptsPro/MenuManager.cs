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

    // Claim GameObject in Main Menu Game
    public GameObject titleGame;
    public GameObject mainMenu;

    // Claim GameObject for Setting Menu Game
    public GameObject settingMenu;


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
        LoadGame();
    }

    private void OnSettingsButtonClicked()
    {
        if (titleGame.activeSelf == true && mainMenu.activeSelf == true && settingMenu.activeSelf == false)
        {
            Debug.Log("Settings clicked");

            // Make Title Game and Main Menu disappear when Setting Button is clicked
            titleGame.SetActive(false);
            mainMenu.SetActive(false);
            settingMenu.SetActive(true);

            // Open Setting Menu
            OpenSettingsMenu();

            if (OpenSettingsMenu() == 1)
            {
                titleGame.SetActive(true);
                mainMenu.SetActive(true);


            }
        }
            
        
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

    private int OpenSettingsMenu()
    {
        Debug.Log("Opening settings menu...");
        // Add your settings menu logic here
        return 1;
    }
}
