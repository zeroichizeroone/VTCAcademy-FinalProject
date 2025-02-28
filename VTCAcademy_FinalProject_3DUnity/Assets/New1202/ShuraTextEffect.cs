using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShuraTextEffect : MonoBehaviour
{
    [Header("Effect Options")]
    public bool blinkEffect = false;
    public bool colorChangeEffect = false;
    public bool shakeEffect = false;
    public bool disappearEffect = false;

    [Header("Effect Settings")]
    public float blinkInterval = 1f;
    public Color[] colors;
    public float shakeIntensity = 5f;
    public float disappearTime = 5f;

    private Text targetText;
    private Coroutine blinkCoroutine;
    private Coroutine colorChangeCoroutine;
    private Coroutine shakeCoroutine;
    private Coroutine disappearCoroutine;

    private void Awake()
    {
        targetText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartEffects();
    }

    private void OnDisable()
    {
        StopEffects();
    }

    private void StartEffects()
    {
        if (blinkEffect)
        {
            blinkCoroutine = StartCoroutine(FadeBlinkText());
        }
        if (colorChangeEffect)
        {
            colorChangeCoroutine = StartCoroutine(ChangeTextColor());
        }
        if (shakeEffect)
        {
            shakeCoroutine = StartCoroutine(ShakeText());
        }
        if (disappearEffect)
        {
            disappearCoroutine = StartCoroutine(DisappearEffect());
        }
    }

    private void StopEffects()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        if (colorChangeCoroutine != null) StopCoroutine(colorChangeCoroutine);
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        if (disappearCoroutine != null) StopCoroutine(disappearCoroutine);
    }

    private IEnumerator FadeBlinkText()
    {
        while (true)
        {
            yield return StartCoroutine(FadeText(1f, 0f, blinkInterval / 2));
            yield return StartCoroutine(FadeText(0f, 1f, blinkInterval / 2));
        }
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color textColor = targetText.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            textColor.a = alpha;
            targetText.color = textColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textColor.a = endAlpha;
        targetText.color = textColor;
    }

    private IEnumerator ChangeTextColor()
    {
        int colorIndex = 0;
        while (true)
        {
            targetText.color = colors[colorIndex];
            colorIndex = (colorIndex + 1) % colors.Length;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator ShakeText()
    {
        Vector3 originalPosition = targetText.rectTransform.localPosition;
        while (true)
        {
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            targetText.rectTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            yield return null;
        }
    }

    private IEnumerator DisappearEffect()
    {
        yield return new WaitForSeconds(disappearTime);
        targetText.gameObject.SetActive(false);
    }
}