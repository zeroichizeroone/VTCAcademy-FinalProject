using System.Collections;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    private Light lightComponent;

    [Header("Light Settings")]
    public int minBlinkCount = 1;  // Số lần chớp tối thiểu
    public int maxBlinkCount = 5;  // Số lần chớp tối đa
    public float minBlinkDuration = 0.05f; // Thời gian tắt/bật tối thiểu
    public float maxBlinkDuration = 0.3f;  // Thời gian tắt/bật tối đa
    public float minRestTime = 1f;  // Thời gian nghỉ giữa các lần chớp
    public float maxRestTime = 4f;  // Thời gian nghỉ tối đa

    void Start()
    {
        lightComponent = GetComponent<Light>();
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
                yield return new WaitForSeconds(Random.Range(minBlinkDuration, maxBlinkDuration));

                lightComponent.enabled = true;
                yield return new WaitForSeconds(Random.Range(minBlinkDuration, maxBlinkDuration));
            }

            yield return new WaitForSeconds(Random.Range(minRestTime, maxRestTime));
        }
    }
}