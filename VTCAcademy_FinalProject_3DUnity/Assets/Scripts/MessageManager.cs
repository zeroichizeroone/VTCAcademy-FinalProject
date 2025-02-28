using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance { get; private set; }

    public GameObject formMessage;
    public Text textMessage;

    public GameObject messageWarningObject;

    // Private Attributes
    private string fullMessage;
    private int crrCharIndex;
    private int charPerPage = 110;
    private Coroutine typewriterCoroutine;
    private float typewriterSpeed = 20f;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && formMessage.activeSelf)
        {
            if (crrCharIndex < fullMessage.Length)
            {
                if (typewriterCoroutine != null)
                {
                    StopCoroutine(typewriterCoroutine);
                }
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

        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        typewriterCoroutine = StartCoroutine(TypeText(pageText));

        crrCharIndex = endIndex;
    }

    private IEnumerator TypeText(string text)
    {
        textMessage.text = "";
        float delay = 1f / typewriterSpeed;

        for (int i = 0; i < text.Length; i++)
        {
            textMessage.text += text[i];
            yield return new WaitForSeconds(delay);
        }
    }

    public void HideMessage()
    {
        formMessage.SetActive(false);
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
    }

    public void ShowMessageWarning(string message)
    { 
        messageWarningObject.GetComponent<Text>().text = message;
        messageWarningObject.SetActive(true);
    }
}