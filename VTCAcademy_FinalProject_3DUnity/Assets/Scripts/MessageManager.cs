using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance { get; private set; }

    public GameObject formMessage;
    public Text textMessage;

    // Private Attibutes
    private string fullMessage;
    private int crrCharIndex;
    private int charPerPage = 100;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (formMessage != null)
        {
            textMessage = formMessage.GetComponentInChildren<Text>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && formMessage.activeSelf)
        {
            if (crrCharIndex < fullMessage.Length)
            {
                ShowNextPage();
            }
            else
            {
                formMessage.SetActive(false);
            }
        }
    }

    public void ShowTextMessage(string mess)
    {
        formMessage.SetActive(true);
        fullMessage = mess;
        crrCharIndex = 0;
        ShowNextPage();
    }

    private void ShowNextPage()
    {
        int charsToShow = Mathf.Min(charPerPage, fullMessage.Length - crrCharIndex);
        textMessage.text = fullMessage.Substring(crrCharIndex, charsToShow);
        crrCharIndex += charsToShow;
    }

    public void HideMessage()
    {
        formMessage.SetActive(false);
    }
}
