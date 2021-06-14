using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSrc = null;
    [SerializeField] private GameObject Overlay = null;

    [SerializeField] private GameObject PauseBtn = null;
    [SerializeField] private GameObject UnpauseBtn = null;

    public void Pause()
    {
        SetPause(true);
    }

    public void Unpause()
    {
        SetPause(false);
    }

    private void SetPause(in bool paused)
    {
        Overlay.SetActive(paused);
        AudioSrc.mute = paused;

        PauseBtn.SetActive(!paused);
        UnpauseBtn.SetActive(paused);
    }
}
