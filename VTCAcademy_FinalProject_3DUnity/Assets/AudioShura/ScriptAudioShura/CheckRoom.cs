using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRoom : MonoBehaviour
{
    [SerializeField] private string[] dialogueMessages; // Danh sách câu thoại
    [SerializeField] private AudioClip[] dialogueSounds; // Danh sách âm thanh thoại
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            DialogueManager.Instance.EnqueueDialogue(dialogueMessages, dialogueSounds, other.gameObject);
        }
    }
}
