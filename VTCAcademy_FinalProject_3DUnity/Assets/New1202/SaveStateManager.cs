using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static string savePath => Path.Combine(Application.persistentDataPath, "snapshot.save");

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        byte[] saveData = SceneManager.GetActiveScene().GetRootGameObjects().Serialize();
        formatter.Serialize(file, saveData);
        file.Close();

        Debug.Log("Game Saved!");
    }

    public static void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(savePath, FileMode.Open);
        byte[] saveData = (byte[])formatter.Deserialize(file);
        file.Close();

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Deserialize(saveData);

        Debug.Log("Game Loaded!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SaveGame();
        if (Input.GetKeyDown(KeyCode.F9)) LoadGame();
    }
}
