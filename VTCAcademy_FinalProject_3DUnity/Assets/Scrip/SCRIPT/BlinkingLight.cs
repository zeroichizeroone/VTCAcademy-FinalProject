using System.Collections;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    private Light lightComponent;

    [Header("Light Settings")]
    public int blinkCount = 2; // Số lần nhấp nháy
    public float blinkDuration = 0.2f; // Thời gian mỗi lần nhấp nháy (tắt & bật)
    public float restTime = 2f; // Thời gian nghỉ giữa các lần nhấp nháy

    void Start()
    {
        lightComponent = GetComponent<Light>();
        StartCoroutine(BlinkLight());
    }

    private IEnumerator BlinkLight()
    {
        while (true) // Lặp vô hạn để đèn luôn nhấp nháy
        {
            for (int i = 0; i < blinkCount; i++)
            {
                lightComponent.enabled = false;
                yield return new WaitForSeconds(blinkDuration);

                lightComponent.enabled = true;
                yield return new WaitForSeconds(blinkDuration);
            }

            yield return new WaitForSeconds(restTime);
        }
    }
}