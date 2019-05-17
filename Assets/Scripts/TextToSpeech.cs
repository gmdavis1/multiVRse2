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
    public SoundFX audio;


    // Start is called before the first frame update
    void Start()
    {
        //audio = gameObject.GetComponent<AudioSource>();
        //StartCoroutine(PlayTranslateAudio("test"));
        StartCoroutine(PlayCloudAudio("test"));
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
            // Show results as text
            if (www.downloadHandler.text != "")
            {
                //Debug.Log(www.downloadHandler.text);
                StartCoroutine(PlayTranslateAudio(www.downloadHandler.text));
            }
        }
    }

    IEnumerator PlayTranslateAudio(string text)
    {
        debugText.text = text;
        text = text.Replace(" ", "%20");
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" + text + "&tl=En-US";
        WWW www = new WWW(url);
        yield return www;
        //Debug.Log(Application.dataPath + "/test.mp3");
        //System.IO.File.WriteAllBytes(Application.dataPath + "/test.mp3", www.bytes);
        //AudioClip clip = www.GetAudioClip(false, true, AudioType.MPEG);
        //clip.loadType = DecompressOnLoad;
        //EncodeMP3.convert(clip, Application.dataPath + "/test.mp3", 128);

        //audio.soundClips[0] = clip;
        //audio.PlaySound();
        debugText.text = "Finished playing";
    }

    IEnumerator PlayCloudAudio(string text)
    {
        debugText.text = text;
        string url = "http://gmdavis.pythonanywhere.com/multivrse/1";
        WWW www = new WWW(url);
        yield return www;
        audio.soundClips[0] = www.GetAudioClip(false, true, AudioType.MPEG);
        debugText.text = "Finished playing";
        audio.PlaySound();
    }
}
