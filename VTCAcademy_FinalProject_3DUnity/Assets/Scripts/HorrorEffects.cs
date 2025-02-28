using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HorrorEffects : MonoBehaviour
{
    public static HorrorEffects Instance;

    [Header("Hiệu ứng đèn chớp")]
    public int numberOfFlashes = 5;          // Số lần đèn chớp
    public float flashDuration = 0.1f;       // Thời gian mỗi lần chớp
    public Light pointLight;                 // Tham chiếu đến Point Light

    [Header("Âm thanh")]
    public AudioClip horrorSound;            // Âm thanh kinh dị
    private AudioSource audioSource;

    [Header("Hiệu ứng nhòe (Motion Blur)")]
    // Ta vẫn giữ hai biến này để cấu hình, nhưng sẽ override chúng khi “siêu nhòe”
    public float motionBlurDuration = 1.0f;  // Thời gian nhòe bình thường
    public float motionBlurIntensity = 0.9f; // Cường độ nhòe (0 đến 1)

    // Các thông số “siêu nhòe”
    public float superBlurAngle = 2000f;      // Góc nhòe vượt quá 360 để “rất mờ”
    public float superBlurTime = 5f;         // Thời gian duy trì trạng thái siêu nhòe

    private PostProcessVolume postProcessVolume;
    private MotionBlur motionBlur;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của HorrorEffects
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Thiết lập AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Tìm PostProcessVolume (Global) trong scene
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out motionBlur);
        }

        // Đảm bảo Point Light tắt ban đầu
        if (pointLight != null)
        {
            pointLight.enabled = false;
        }
    }

    public void TriggerHorrorEffect()
    {
        // 1. Chớp đèn
        StartCoroutine(FlashLight());

        // 2. Phát âm thanh kinh dị
        if (horrorSound != null)
        {
            audioSource.clip = horrorSound;
            audioSource.Play();
        }

        // 3. Kích hoạt “Siêu nhòe” màn hình trong 5 giây
        StartCoroutine(SuperBlur());
    }

    private IEnumerator FlashLight()
    {
        if (pointLight == null)
        {
            Debug.LogWarning("Point Light chưa được gán trong Inspector!");
            yield break;
        }

        for (int i = 0; i < numberOfFlashes; i++)
        {
            pointLight.enabled = true;
            yield return new WaitForSeconds(flashDuration / 2f);
            pointLight.enabled = false;
            yield return new WaitForSeconds(flashDuration / 2f);
        }
    }

    /// <summary>
    /// Hàm tạo “siêu nhòe” (shutterAngle > 360) và duy trì 5 giây
    /// </summary>
    private IEnumerator SuperBlur()
    {
        if (motionBlur != null)
        {
            // Lưu lại giá trị cũ để khôi phục
            float originalAngle = motionBlur.shutterAngle.value;
            bool originalActive = motionBlur.active;

            // Đẩy góc nhòe lên cao (vd 720)
            motionBlur.shutterAngle.value = superBlurAngle;
            motionBlur.active = true;

            // Duy trì trạng thái này 5 giây
            yield return new WaitForSeconds(superBlurTime);

            // Khôi phục
            motionBlur.shutterAngle.value = originalAngle;
            motionBlur.active = originalActive;
        }
    }

    /// <summary>
    /// Hàm kích hoạt motion blur “bình thường” (nếu muốn dùng riêng)
    /// </summary>
    public IEnumerator EnableMotionBlur(float duration, float intensity)
    {
        if (motionBlur != null)
        {
            // Điều chỉnh cường độ Motion Blur (0 → 360)
            motionBlur.shutterAngle.value = Mathf.Lerp(0, 360, intensity);
            motionBlur.active = true;
            yield return new WaitForSeconds(duration);
            motionBlur.active = false;
        }
    }
}
