using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    private Light pointLight;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        pointLight = GetComponent<Light>();
    }

    void Update()
    {
        pointLight.intensity = Mathf.Lerp(pointLight.intensity, Random.Range(minIntensity, maxIntensity), flickerSpeed);
    }
}
