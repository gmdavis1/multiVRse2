using System;
using System.IO;
using UnityEngine;
//using Google.Cloud.TextToSpeech.V1;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

public class TextToSpeech : MonoBehaviour
{
    public AudioSource audioSource;
    //public AudioClip clip;
    public Animator anim;
    public float volume = 0.5f;

    [SerializeField] private MechanicSceneUIManager mechanicSceneUIManager = null;

    void Start()
    {
        //Find instance if it's not set
        if (mechanicSceneUIManager == null)
        {
            mechanicSceneUIManager = FindObjectOfType<MechanicSceneUIManager>();
        }
        //StartCoroutine(TestAudio());
     //   StartCoroutine(PlayCloudAudio("0"));
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(PlayCloudAudio("1.mp3"));
        StartCoroutine(GetText());
    }

    //IEnumerator TestAudio()
    //{
    //    string url = "https://file-examples-com.github.io/uploads/2017/11/file_example_OOG_1MG.ogg";
    //    Debug.Log("Accessing file at " + url);
    //    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.isHttpError)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
    //            anim.Play("Talk");
    //            audioSource.PlayOneShot(myClip, volume);
    //            Debug.Log("Finished playing " + url);
    //        }
    //    }
    //}

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://gmdavis.pythonanywhere.com/multivrse"))
        {
            yield return www.SendWebRequest();

            if (!(www.isNetworkError || www.isHttpError))
            {
                if (string.IsNullOrEmpty(www.downloadHandler.text) == false)
                {
                    if (mechanicSceneUIManager != null)
                    {
                        mechanicSceneUIManager.WebText.text = www.downloadHandler.text;
                    }
                    else
                    {
                        Debug.LogWarning("No scene UI manager to display text corresponding to audio!");
                    }
                    Debug.Log(www.downloadHandler.text);
                    StartCoroutine(PlayCloudAudio(www.downloadHandler.text));
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }
    }

    IEnumerator PlayCloudAudio(string num)
    {
        string url = "https://multivrse-audio.s3.us-east-2.amazonaws.com/" + num;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            Debug.Log("Accessing file at " + url);
            if (!(www.isNetworkError || www.isHttpError))
            {
                if (string.IsNullOrEmpty(www.downloadHandler.text) == false)
                {
                    Debug.Log("data" + www.downloadHandler.text);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("error");
            }
        }

        yield return new WaitForSeconds(1f);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                anim.Play("Talk");
                audioSource.PlayOneShot(myClip, volume);
                Debug.Log("Finished playing " + url);
            }
        }
    }
}
