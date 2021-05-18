using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayCloudAudio : MonoBehaviour
{
    public AudioSource audioSource;
    //public AudioClip clip;
    public Animator anim;
    public float volume = 0.5f;

    [SerializeField] private string TalkAnim = "Talk";

    [SerializeField] private MechanicSceneUIManager mechanicSceneUIManager = null;
    [SerializeField] private WebSocketHandler WebSocketHndler = null;

    private void OnEnable()
    {
        WebSocketHndler.SocketMessageEvent.RemoveListener(OnSocketResponse);
        WebSocketHndler.SocketMessageEvent.AddListener(OnSocketResponse);
    }

    private void OnDisable()
    {
        if (WebSocketHndler != null)
        {
            WebSocketHndler.SocketMessageEvent.RemoveListener(OnSocketResponse);
        }
    }

    private void OnSocketResponse(WebSocketHandler.SocketResponse socketResponse)
    {
        //Not the active scene, so ignore
        if (socketResponse.Scene != SceneManager.GetActiveScene().name)
        {
            return;
        }

        if (socketResponse.Action == SocketActions.animation)
        {
            anim.Play(socketResponse.Value);
        }
        else if (socketResponse.Action == SocketActions.playaudio)
        {
            StartCoroutine(FetchAndPlayCloudAudio(socketResponse.Value));
        }
    }

    private void Start()
    {
        //Find instance if it's not set
        if (mechanicSceneUIManager == null)
        {
            mechanicSceneUIManager = FindObjectOfType<MechanicSceneUIManager>();
        }
    }

    private IEnumerator FetchAndPlayCloudAudio(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.isHttpError)
            {
                Debug.LogError($"Error fetching clip: {www.responseCode} - {www.error}");
                yield break;
            }

            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);

            if (myClip == null)
            {
                Debug.LogError($"Returned audio clip at {url} is null!");
                yield break;
            }

            if (anim != null)
            {
                anim.Play(TalkAnim);
            }

            audioSource.PlayOneShot(myClip, volume);

            while (audioSource.isPlaying == true)
            {
                yield return null;
            }

            Debug.Log($"Finished playing audio clip from {url}");
        }
    }
}
