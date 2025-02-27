using System.Collections;
using UnityEngine;
public class BackgroundMusic : MonoBehaviour
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