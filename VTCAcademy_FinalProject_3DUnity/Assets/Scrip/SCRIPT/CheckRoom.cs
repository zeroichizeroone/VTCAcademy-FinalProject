using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRoom : MonoBehaviour
{
    [SerializeField] private string dialogueMessage = "Mặc định"; 
    private bool hasTriggered = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player")) 
        {
            MessageManager.Instance.ShowTextMessage(dialogueMessage); 
            hasTriggered = true; 
        }
    }
}

