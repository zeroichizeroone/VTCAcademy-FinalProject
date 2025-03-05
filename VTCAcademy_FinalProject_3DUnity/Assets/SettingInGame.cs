using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingInGame : MonoBehaviour
{
    public Button buttonExit;

    // Start is called before the first frame update
    void Start()
    {
        buttonExit.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
