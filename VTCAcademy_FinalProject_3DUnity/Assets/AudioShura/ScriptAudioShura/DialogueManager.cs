using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private Queue<string> dialogueQueue = new Queue<string>(); // Hàng đợi câu thoại
    private Queue<AudioClip> audioQueue = new Queue<AudioClip>(); // Hàng đợi âm thanh
    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EnqueueDialogue(string[] messages, AudioClip[] sounds, GameObject player)
    {
        AudioSource playerAudio = player.GetComponent<AudioSource>();
        if (playerAudio == null)
        {
            Debug.LogWarning("Player không có AudioSource!");
            return;
        }

        // Thêm câu thoại & âm thanh vào hàng đợi
        foreach (var message in messages) dialogueQueue.Enqueue(message);
        foreach (var sound in sounds) audioQueue.Enqueue(sound);

        if (!isPlaying) StartCoroutine(PlayDialogue(playerAudio)); // Nếu chưa có thoại nào đang chạy thì bắt đầu
    }

    private IEnumerator PlayDialogue(AudioSource playerAudio)
    {
        isPlaying = true;

        while (dialogueQueue.Count > 0)
        {
            string message = dialogueQueue.Dequeue();
            MessageManager.Instance.ShowTextMessage(message); // Hiển thị thoại

            if (audioQueue.Count > 0)
            {
                AudioClip clip = audioQueue.Dequeue();
                if (clip != null)
                {
                    playerAudio.clip = clip;
                    playerAudio.Play();
                    yield return new WaitForSeconds(playerAudio.clip.length); // Chờ thoại phát xong
                }
            }
            else
            {
                yield return new WaitForSeconds(2f); // Nếu không có âm thanh, chờ 2s trước câu tiếp theo
            }
        }

        isPlaying = false;
    }
}
