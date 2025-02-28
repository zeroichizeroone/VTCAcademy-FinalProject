using System.Collections;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    private Light lightComponent;
    private AudioSource audioSource;

    [Header("Light Settings")]
    public int minBlinkCount = 1;
    public int maxBlinkCount = 5;
    public float minBlinkDuration = 0.05f;
    public float maxBlinkDuration = 0.3f;
    public float minRestTime = 1f;
    public float maxRestTime = 4f;

    [Header("Audio Settings")]
    public AudioClip flickerSound;
    public float minSoundDelay = 0.5f;  // Thời gian tối thiểu giữa các lần phát âm thanh
    public float maxSoundDelay = 2f;    // Thời gian tối đa giữa các lần phát âm thanh

    private bool canPlaySound = true; // Kiểm soát phát âm thanh

    void Start()
    {
        lightComponent = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Cấu hình AudioSource
        audioSource.clip = flickerSound;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 0.5f;
        audioSource.maxDistance = 5f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        while (true)
        {
            int blinkTimes = Random.Range(minBlinkCount, maxBlinkCount);

            for (int i = 0; i < blinkTimes; i++)
            {
                lightComponent.enabled = false;
                TryPlaySound(); // Cố gắng phát âm thanh khi nhấp nháy
                yield return new WaitForSeconds(Random.Range(minBlinkDuration, maxBlinkDuration));

                lightComponent.enabled = true;
                yield return new WaitForSeconds(Random.Range(minBlinkDuration, maxBlinkDuration));
            }

            yield return new WaitForSeconds(Random.Range(minRestTime, maxRestTime));
        }
    }

    private void TryPlaySound()
    {
        if (canPlaySound && flickerSound != null)
        {
            audioSource.Play();
            canPlaySound = false;
            StartCoroutine(ResetSoundCooldown());
        }
    }

    private IEnumerator ResetSoundCooldown()
    {
        yield return new WaitForSeconds(Random.Range(minSoundDelay, maxSoundDelay));
        canPlaySound = true;
    }
}
