using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiosound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource; // Gán AudioSource trong Inspector
    public AudioClip audioClip; // Gán file âm thanh trong Inspector
    [Range(1f, 50f)] public float minDistance = 1f; // Khoảng cách gần nhất
    [Range(5f, 100f)] public float maxDistance = 15f; // Khoảng cách xa nhất
    public bool loopAudio = true; // Lặp âm thanh liên tục

    private void Start()
    {
        // Đảm bảo AudioSource được thiết lập đúng
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.spatialBlend = 1.0f; // Bật chế độ 3D Sound
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Giảm âm lượng theo khoảng cách tuyến tính
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.dopplerLevel = 0f; // Không bị ảnh hưởng bởi Doppler Effect
        audioSource.loop = loopAudio; // Lặp lại nếu cần

        PlaySound(); // Tự động phát khi game bắt đầu
    }

    public void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }


}
