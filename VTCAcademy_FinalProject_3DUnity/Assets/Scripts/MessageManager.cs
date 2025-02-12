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
    private int charPerPage = 110;

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
        if (crrCharIndex >= fullMessage.Length)
        {
            formMessage.SetActive(false);
            return;
        }

        int endIndex = crrCharIndex + charPerPage;
        if (endIndex >= fullMessage.Length)
        {
            endIndex = fullMessage.Length;
        }
        else
        {
            endIndex = fullMessage.LastIndexOf(' ', endIndex);
            if (endIndex <= crrCharIndex)
            {
                endIndex = crrCharIndex + charPerPage;
            }
        }

        string pageText = fullMessage.Substring(crrCharIndex, endIndex - crrCharIndex);

        if (endIndex < fullMessage.Length)
        {
            pageText += "...";
        }

        textMessage.text = pageText;
        crrCharIndex = endIndex;
    }

    public void HideMessage()
    {
        formMessage.SetActive(false);
    }
}