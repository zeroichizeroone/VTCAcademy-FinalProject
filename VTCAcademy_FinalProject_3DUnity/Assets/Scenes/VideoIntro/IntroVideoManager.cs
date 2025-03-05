using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Kéo VideoPlayer vào Inspector
    public string nextSceneName = "MainMenu";  // Tên scene tiếp theo

    private void Start()
    {
        // Đăng ký sự kiện khi video kết thúc
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Chuyển scene sau khi video kết thúc
        SceneManager.LoadScene("MainMenu");
    }
}
