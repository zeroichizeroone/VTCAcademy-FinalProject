using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOfLight : MonoBehaviour
{
    public Light lightSource; // Gán đèn cần bật/tắt
    public float blinkInterval = 0.5f; // Thời gian giữa mỗi lần bật/tắt (tính bằng giây)
    private float timer = 0f;

    void Start()
    {
        if (lightSource == null)
        {
            // Tự động lấy Light từ GameObject nếu chưa gán
            lightSource = GetComponent<Light>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            // Đặt lại timer
            timer = 0f;

            // Bật hoặc tắt đèn
            lightSource.enabled = !lightSource.enabled;

            // In ra console để kiểm tra trạng thái
            // Debug.Log("Light toggled: " + (lightSource.enabled ? "On" : "Off"));
        }
    }
}
