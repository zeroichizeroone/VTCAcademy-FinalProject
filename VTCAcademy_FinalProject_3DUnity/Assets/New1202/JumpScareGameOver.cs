using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class JumpScareGameOver : MonoBehaviour
{
    public PlayableDirector timeline;
    public string nextScene = "IntroGame"; // Scene sẽ load sau jumpscare

    private void Start()
    {
        timeline.stopped += OnJumpScareEnd;
        timeline.Play();
    }

    private void OnJumpScareEnd(PlayableDirector director)
    {
        SceneManager.LoadScene(nextScene);
    }
}
