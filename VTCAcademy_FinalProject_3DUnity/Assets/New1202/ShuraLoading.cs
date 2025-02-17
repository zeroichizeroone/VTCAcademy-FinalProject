using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShuraLoading : MonoBehaviour
{
    public static string sceneName = "Map1";
    public Text citingText;
    public List<string> citingList;

    private void Start()
    {
        string randomCiting = citingList[Random.Range(0, citingList.Count)];
        StartCoroutine(TypeText(randomCiting));
    }

    private IEnumerator TypeText(string text)
    {
        citingText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            citingText.text += letter;
            yield return new WaitForSeconds(0.12f);
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}
