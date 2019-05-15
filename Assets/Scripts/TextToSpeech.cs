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
        StartCoroutine(PlayTranslateAudio("test"));
        //StartCoroutine(PlayCloudAudio("test"));
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

    void PlayCloudAudio(string text)
    {
        //string url = "https://texttospeech.googleapis.com/v1/text:synthesize?fields=audioContent&key=AIzaSyDNiOUrvRHj0anRBsC1NrrU7v8wwA90v8E";
        //string json = "{\n" +
        //        "\"input\": {\n" +
        //            "\"text\": \"Sample Text\"\n" +
        //        "},\n" +
        //        "\"voice\": {\n" +
        //            "\"languageCode\": \"en-US\"\n" +
        //        "},\n" +
        //        "\"audioConfig\": {\n" +
        //            "\"audioEncoding\": \"mp3\"\n" +
        //        "}\n" +
        //    "}";
        //Dictionary<string, string> headers = new Dictionary<string, string>();
        //Debug.Log(json);
        //headers.Add("Content-Type", "application/json");
        //WWW www = new WWW(url, Encoding.ASCII.GetBytes(json.ToCharArray()), headers);
        //yield return www;

        //Debug.Log("hi");

        //var client = TextToSpeechClient.Create();

        //// The input to be synthesized, can be provided as text or SSML.
        //var input = new SynthesisInput
        //{
        //    Text = "This is a demonstration of the Google Cloud Text-to-Speech API"
        //};

        //// Build the voice request.
        //var voiceSelection = new VoiceSelectionParams
        //{
        //    LanguageCode = "en-US",
        //    SsmlGender = SsmlVoiceGender.Female
        //};

        //// Specify the type of audio file.
        //var audioConfig = new AudioConfig
        //{
        //    AudioEncoding = AudioEncoding.Mp3
        //};

        //// Perform the text-to-speech request.
        //var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

        //using (var output = File.Create("output.mp3"))
        //{
        //    response.AudioContent.WriteTo(output);
        //}
        //Console.WriteLine("Audio content written to file \"output.mp3\"");

        //audio.clip = response.AudioContent;
    }
}
