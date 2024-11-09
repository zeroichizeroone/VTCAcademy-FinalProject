using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoTATECKI : MonoBehaviour
{
    public Transform hourHand; 
    public Transform minuteHand; 
    public Transform secondHand; 

    public float hourSpeed = 30f; 
    public float minuteSpeed = 6f; 
    public float secondSpeed = 6f;

    public float delayTime = 1f; 
    private float lastRotationTime = 0f;// Thời gian quay lần cuối

    void Update()
    {
        // Kiểm tra nếu thời gian trôi qua đủ lâu để quay lại
        if (Time.time - lastRotationTime >= delayTime)
        {
            // Tính toán góc quay của kim giờ, phút, giây
            float hourAngle = (Time.time / 3600f) * 360f * (hourSpeed / 30f); // 1 giờ = 360°
            float minuteAngle = (Time.time / 60f) * 360f * (minuteSpeed / 60f); // 1 phút = 360°
            float secondAngle = (Time.time % 60f) * 360f * (secondSpeed / 60f); // 1 giây = 360°

            // Cập nhật vị trí kim
            hourHand.rotation = Quaternion.Euler(0, 0, -hourAngle);
            minuteHand.rotation = Quaternion.Euler(0, 0, -minuteAngle);
            secondHand.rotation = Quaternion.Euler(0, 0, -secondAngle);

            // Cập nhật thời gian quay lần cuối
            lastRotationTime = Time.time;
        }
    }
}
