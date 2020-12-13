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

    void Start()
    {
        //StartCoroutine(TestAudio());
     //   StartCoroutine(PlayCloudAudio("0"));
    }

    // Update is called once per frame
    void Update()
    {
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
        UnityWebRequest www = UnityWebRequest.Get("http://gmdavis.pythonanywhere.com/multivrse");
        yield return www.SendWebRequest();

        if (! (www.isNetworkError || www.isHttpError))
        {
            if (www.downloadHandler.text != "")
            {
                FindObjectOfType<MechanicSceneUIManager>().WebText.text = www.downloadHandler.text;
                Debug.Log(www.downloadHandler.text);
                StartCoroutine(PlayCloudAudio(www.downloadHandler.text));
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    IEnumerator PlayCloudAudio(string num)
    {
        string url = "http://gmdavis.pythonanywhere.com/multivrse/" + num;
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log("Accessing file at " + url);
        if (!(www.isNetworkError || www.isHttpError))
        {
            if (www.downloadHandler.text != "")
            {
               // FindObjectOfType<MechanicSceneUIManager>().WebText.text = www.downloadHandler.text;
                Debug.Log("data" + www.downloadHandler.text);               // StartCoroutine(PlayCloudAudio(www.downloadHandler.text));
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
        yield return new WaitForSeconds(1f);
        using (www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
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
