using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanDomTiLing : MonoBehaviour
{
    public Material material; // Gán material cần thay đổi tiling
    public float changeInterval = 1f; // Thời gian giữa các lần thay đổi (tính bằng giây)
    private float timer = 0f;

    void Start()
    {
        if (material == null)
        {
            // Thử lấy Material từ Renderer nếu chưa được gán
            material = GetComponent<Renderer>().material;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            // Đặt timer về 0 sau khi thay đổi
            timer = 0f;

            // Tạo giá trị ngẫu nhiên cho tiling (x, y)
            float randomX = Random.Range(1f, 9f);
            float randomY = Random.Range(1f, 9f);

            // Cập nhật Tiling của material
            material.mainTextureScale = new Vector2(randomX, randomY);

            // In ra console để kiểm tra
            Debug.Log($"New Tiling: {randomX}, {randomY}");
        }
    }
}
