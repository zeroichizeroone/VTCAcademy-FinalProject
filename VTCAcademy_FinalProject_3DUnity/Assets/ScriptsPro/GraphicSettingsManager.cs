using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GraphicsSettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Dropdown resolutionDropdown;
    public Toggle windowedToggle;
    public Toggle borderlessWindowedToggle;
    public Toggle fullscreenToggle;
    public Toggle antiAliasingToggle;
    public Toggle vSyncToggle;
    public Toggle lowQualityToggle;
    public Toggle mediumQualityToggle;
    public Toggle highQualityToggle;
    public Toggle ultraQualityToggle;
    public Toggle frameRate30Toggle;
    public Toggle frameRate60Toggle;
    public Toggle frameRate120Toggle;
    public Toggle frameRateUnlimitedToggle;

    private Resolution[] resolutions;
    private string settingsPath;

    [System.Serializable]
    public class GraphicsSettingsData
    {
        public int resolutionIndex;
        public bool isWindowed;
        public bool isBorderlessWindowed;
        public bool isFullscreen;
        public int qualityLevel;
        public bool isAntiAliasingOn;
        public bool isVSyncOn;
        public int frameRateLimit; // 30, 60, 120, -1 (Unlimited)
    }

    private GraphicsSettingsData currentSettings;

    void Start()
    {
        settingsPath = Path.Combine(Application.persistentDataPath, "graphicsSettings.json");

        // Initialize resolutions dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            resolutionDropdown.options.Add(new Dropdown.OptionData(option));
        }

        LoadSettings();
    }

    public void ApplySettings()
    {
        currentSettings = new GraphicsSettingsData
        {
            resolutionIndex = resolutionDropdown.value,
            isWindowed = windowedToggle.isOn,
            isBorderlessWindowed = borderlessWindowedToggle.isOn,
            isFullscreen = fullscreenToggle.isOn,
            qualityLevel = GetQualityLevel(),
            isAntiAliasingOn = antiAliasingToggle.isOn,
            isVSyncOn = vSyncToggle.isOn,
            frameRateLimit = GetFrameRateLimit()
        };

        // Apply Resolution
        Resolution selectedResolution = resolutions[currentSettings.resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, currentSettings.isFullscreen);

        // Apply Quality
        QualitySettings.SetQualityLevel(currentSettings.qualityLevel);

        // Apply Anti-Aliasing
        QualitySettings.antiAliasing = currentSettings.isAntiAliasingOn ? 4 : 0;

        // Apply VSync
        QualitySettings.vSyncCount = currentSettings.isVSyncOn ? 1 : 0;

        // Apply Frame Rate Limit
        Application.targetFrameRate = currentSettings.frameRateLimit;

        SaveSettings();
    }

    private void LoadSettings()
    {
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            currentSettings = JsonUtility.FromJson<GraphicsSettingsData>(json);

            // Load UI values
            resolutionDropdown.value = currentSettings.resolutionIndex;
            windowedToggle.isOn = currentSettings.isWindowed;
            borderlessWindowedToggle.isOn = currentSettings.isBorderlessWindowed;
            fullscreenToggle.isOn = currentSettings.isFullscreen;
            antiAliasingToggle.isOn = currentSettings.isAntiAliasingOn;
            vSyncToggle.isOn = currentSettings.isVSyncOn;

            SetQualityToggles(currentSettings.qualityLevel);
            SetFrameRateToggles(currentSettings.frameRateLimit);
        }
        else
        {
            Debug.LogWarning("No settings file found. Using default settings.");
        }
    }

    private void SaveSettings()
    {
        string json = JsonUtility.ToJson(currentSettings, true);
        File.WriteAllText(settingsPath, json);
        Debug.Log("Settings saved to JSON.");
    }

    private int GetQualityLevel()
    {
        if (lowQualityToggle.isOn) return 0;
        if (mediumQualityToggle.isOn) return 1;
        if (highQualityToggle.isOn) return 2;
        if (ultraQualityToggle.isOn) return 3;
        return 1; // Default to Medium
    }

    private void SetQualityToggles(int level)
    {
        lowQualityToggle.isOn = level == 0;
        mediumQualityToggle.isOn = level == 1;
        highQualityToggle.isOn = level == 2;
        ultraQualityToggle.isOn = level == 3;
    }

    private int GetFrameRateLimit()
    {
        if (frameRate30Toggle.isOn) return 30;
        if (frameRate60Toggle.isOn) return 60;
        if (frameRate120Toggle.isOn) return 120;
        if (frameRateUnlimitedToggle.isOn) return -1;
        return 60; // Default to 60 FPS
    }

    private void SetFrameRateToggles(int limit)
    {
        frameRate30Toggle.isOn = limit == 30;
        frameRate60Toggle.isOn = limit == 60;
        frameRate120Toggle.isOn = limit == 120;
        frameRateUnlimitedToggle.isOn = limit == -1;
    }
}
