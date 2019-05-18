using System;
using System.IO;
using UnityEngine;
//using Google.Cloud.TextToSpeech.V1;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using OVR;

public class TextToSpeech : MonoBehaviour
{
    public TextMesh debugText;
    public SoundFX sound;
    public Animator anim;

    void Start()
    {
        StartCoroutine(PlayCloudAudio("1"));
    }


    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://gmdavis.pythonanywhere.com/multivrse");
        yield return www.SendWebRequest();

        if (! (www.isNetworkError || www.isHttpError))
        {
            if (www.downloadHandler.text != "")
            {
                StartCoroutine(PlayCloudAudio(www.downloadHandler.text));
            }
        }
    }

    IEnumerator PlayCloudAudio(string num)
    {
        debugText.text = num;
        string url = "http://gmdavis.pythonanywhere.com/multivrse/" + num;
        WWW www = new WWW(url);
        yield return www;
        sound.soundClips[0] = www.GetAudioClip(false, true, AudioType.MPEG);
        debugText.text = "Finished playing";
        anim.Play("Talk");
        sound.PlaySound();
    }
}
