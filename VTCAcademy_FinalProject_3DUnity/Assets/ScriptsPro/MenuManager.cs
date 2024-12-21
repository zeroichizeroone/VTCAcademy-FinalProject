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
    public GameObject menuSettingUI;
    public GameObject settingTile;
    public GameObject menuSettingOptions;
    public GameObject menuSettingContents;

    // Tille button for setting game
    public Button graphicsButton;
    public Button audioButton;
    public Button controlsButton;
    public Button generalButton;
    public Button applyButton;

    public Button[] buttonsNeedChangeColor;

    // Content show for setting game
    public GameObject graphicsContent;
    public GameObject audioContent;
    public GameObject controlsContent;
    public GameObject generalContent;

    // Set Alpha value for each status
    private Color activeColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 30f / 255f);
    private Color inactiveColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f / 255f);

    private Color activeTextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 255f / 255f);
    private Color inactiveTextColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);


    private void Start()
    {
        // Control button Alpha value
        buttonsNeedChangeColor = new Button[] { graphicsButton, audioButton, controlsButton, generalButton};

        // Ensure all buttons are assigned
        if (startButton) startButton.onClick.AddListener(OnStartButtonClicked);
        if (continueButton) continueButton.onClick.AddListener(OnContinueButtonClicked);
        if (settingsButton) settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        if (exitButton) exitButton.onClick.AddListener(OnExitButtonClicked);

        if (graphicsButton) graphicsButton.onClick.AddListener(GraphicButtonClicked);
        if (audioButton) audioButton.onClick.AddListener(AudioButtonClicked);
        if (controlsButton) controlsButton.onClick.AddListener(ControlButtonClicked);
        if (generalButton) generalButton.onClick.AddListener(GeneralButtonClicked);
        if (applyButton) applyButton.onClick.AddListener(ApplyButtonClicked);
    }

    //Main Menu Button Manager
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
        if (titleGame.activeSelf == true && mainMenu.activeSelf == true && menuSettingUI.activeSelf == false)
        {
            Debug.Log("Settings clicked");

            // Make Title Game and Main Menu disappear when Setting Button is clicked
            titleGame.SetActive(false);
            mainMenu.SetActive(false);
            menuSettingUI.SetActive(true);

            // Open Setting Menu
            Debug.Log("Opening settings menu...");

            //Make sure all of objects in setting menu in active state
            if (menuSettingUI.activeSelf == true)
            {
                GraphicButtonClicked();
            }
        }        
    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Game clicked");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Ensure it works in the Editor
#endif
    }

    //Setting Menu Button Manager
    public void GraphicButtonClicked()
    {
        Debug.Log("graphicButtonClicked");
        ActivateContent(graphicsContent);
        SetActiveButton(graphicsButton);
    }

    public void AudioButtonClicked()
    {
        Debug.Log("audioButtonClicked");
        ActivateContent(audioContent);
        SetActiveButton(audioButton);
    }

    public void ControlButtonClicked()
    {
        Debug.Log("controlButtonClicked");
        ActivateContent(controlsContent);
        SetActiveButton(controlsButton);
    }

    public void GeneralButtonClicked()
    {
        Debug.Log("generalButtonClicked");
        ActivateContent(generalContent);
        SetActiveButton(generalButton);
    }

    public void ApplyButtonClicked()
    {
        Debug.Log("applyButtonClicked");
    }

    private void LoadGame()
    {
        Debug.Log("Loading saved game...");
    }

    private void ActivateContent(GameObject contentToActivate)
    {
        // Deactivate all content panels
        graphicsContent.SetActive(false);
        audioContent.SetActive(false);
        controlsContent.SetActive(false);
        generalContent.SetActive(false);

        // Activate the selected content panel
        contentToActivate.SetActive(true);
    }

    // Change Alpha Value when button actived
    public void SetActiveButton(Button activeButton)
    {
        foreach (var button in buttonsNeedChangeColor)
        {
            var image = button.GetComponent<Image>();
            var text = button.GetComponentInChildren<Text>();

            if (image != null)
            {
                // Set status for button
                image.color = button == activeButton ? activeColor : inactiveColor;
                text.color = button == activeButton ? activeTextColor : inactiveTextColor;
            }
        }
    }
}
