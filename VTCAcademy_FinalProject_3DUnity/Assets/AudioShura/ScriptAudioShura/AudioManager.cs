using System.Collections;
using UnityEngine;

// Script quản lý âm thanh tập trung - đặt vào một GameObject duy nhất
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton

    public AudioSource audioSource;
    public AudioClip normalMusic;
    public float normalVolume = 1f;
    public float fadeDuration = 2f;

    private AudioClip currentRequestedMusic;
    private float currentRequestedVolume;
    private bool isChangingMusic = false;

    void Awake()
    {
        // Thiết lập Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Phát nhạc mặc định ban đầu
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.volume = normalVolume;
        audioSource.Play();

        currentRequestedMusic = normalMusic;
        currentRequestedVolume = normalVolume;
    }

    public void RequestMusicChange(AudioClip newClip, float newVolume)
    {
        // Chỉ thay đổi nếu yêu cầu nhạc mới khác với yêu cầu hiện tại
        if (newClip != currentRequestedMusic)
        {
            currentRequestedMusic = newClip;
            currentRequestedVolume = newVolume;

            // Dừng coroutine hiện tại nếu có
            StopAllCoroutines();

            // Bắt đầu coroutine mới
            StartCoroutine(FadeMusicRoutine(newClip, newVolume));
        }
    }

    // Phương thức để trở về nhạc mặc định
    public void RestoreDefaultMusic()
    {
        RequestMusicChange(normalMusic, normalVolume);
    }

    IEnumerator FadeMusicRoutine(AudioClip newClip, float targetVolume)
    {
        isChangingMusic = true;
        float startVolume = audioSource.volume;
        float timeElapsed = 0;

        // Fade out
        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0;

        // Đổi nhạc
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        timeElapsed = 0;

        // Fade in
        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0, targetVolume, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
        isChangingMusic = false;
    }
}

// Script gắn vào từng vùng trigger riêng biệt
public class MusicTriggerZone : MonoBehaviour
{
    public AudioClip zoneMusic; // Nhạc riêng cho vùng này
    public float zoneVolume = 0.5f; // Âm lượng cho vùng này

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Yêu cầu AudioManager thay đổi nhạc
            AudioManager.Instance.RequestMusicChange(zoneMusic, zoneVolume);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Khôi phục nhạc mặc định khi ra khỏi vùng
            AudioManager.Instance.RestoreDefaultMusic();
        }
    }
}