using System.Collections;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public GameObject jumpscareObject; // Đối tượng jumpscare (hình ảnh con ma)
    public AudioSource jumpscareSound; // Âm thanh đáng sợ
    public Camera mainCamera; // Camera chính
    public float shakeIntensity = 0.3f; // Độ rung của camera
    public float shakeDuration = 0.5f; // Thời gian rung camera
    public float jumpscareDuration = 2f; // Thời gian xuất hiện jumpscare

    private bool hasTriggered = false;

    public GameObject bossPatrol;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(TriggerJumpscare());
        }
    }

    IEnumerator TriggerJumpscare()
    {
        // Bật hình ảnh jumpscare
        jumpscareObject.SetActive(true);

        // Phát âm thanh jumpscare
        if (jumpscareSound)
        {
            jumpscareSound.Play();
        }

        // Hiệu ứng rung camera
        StartCoroutine(CameraShake());

        // Giữ trạng thái jumpscare trong một khoảng thời gian
        yield return new WaitForSeconds(jumpscareDuration);

        // Tắt hình ảnh jumpscare sau khi hết thời gian
        jumpscareObject.SetActive(false);

        if (gameObject.name == "JumScare2")
        {
            bossPatrol.SetActive(true);
        }

        if (gameObject.name == "JumScare3")
        {
            var shura = GameObject.FindGameObjectWithTag("Player");
            shura.GetComponent<ShuraMovement>().StartFaintToMoveScene2();
        }
    }

    IEnumerator CameraShake()
    {
        Vector3 originalPosition = mainCamera.transform.localPosition;
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);

            // Sử dụng vị trí gốc làm điểm tham chiếu để rung quanh vị trí đó
            mainCamera.transform.localPosition = new Vector3(
                originalPosition.x + x,
                originalPosition.y + y,
                originalPosition.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Đặt lại vị trí ban đầu khi kết thúc
        mainCamera.transform.localPosition = originalPosition;
    }
}
