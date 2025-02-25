using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip normalMusic;
    public AudioClip scaryMusic;

    // Âm lượng riêng cho từng bản nhạc (có thể điều chỉnh ở Inspector)
    public float normalVolume = 1f;
    public float scaryVolume = 0.5f;

    public float fadeDuration = 2f; // Thời gian chuyển đổi nhạc

    void Start()
    {
        // Khởi tạo với nhạc bình thường và thiết lập âm lượng tương ứng
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.volume = normalVolume;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines(); // Ngừng các hiệu ứng fade đang chạy
            StartCoroutine(FadeMusic(scaryMusic));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeMusic(normalMusic));
        }
    }

    IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVolume = audioSource.volume;
        // Fade out: giảm dần âm lượng của nhạc hiện tại
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0;

        // Đổi nhạc
        audioSource.clip = newClip;
        audioSource.Play();

        // Xác định âm lượng mục tiêu dựa trên clip mới
        float targetVolume = (newClip == normalMusic) ? normalVolume : (newClip == scaryMusic ? scaryVolume : 1f);

        // Fade in: tăng dần âm lượng từ 0 đến targetVolume
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}
