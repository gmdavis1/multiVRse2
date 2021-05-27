using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneCapture : MonoBehaviour
{
    public AudioSource AudioSrc { get; private set; } = null;

    public bool IsMicConnected { get; private set; } = false;

    private int MinMicFrequency = 0;
    private int MaxMicFrequency = 0;

    [SerializeField] private MicPermissionRequest MicPermRequest = null;
    [SerializeField] private int RecordClipDuration = 1;
    [SerializeField] private float MinSpeakingLevel = 5f;

    //null or an empty string indicates the default microphone
    private string ChosenMic = string.Empty;

    private float[] ClipSampleData = new float[1024];
    public bool IsUserSpeaking = false;

    public float AverageSpeakingVolume = 0f;

    public UnityEvent OnUserStartSpeaking = new UnityEvent();
    public UnityEvent OnUserStopSpeaking = new UnityEvent();

    private void Awake()
    {
        AudioSrc = GetComponent<AudioSource>();

        MicPermRequest.MicPermRequestCompleteEvent.RemoveListener(FetchMicAndStartRecording);
        MicPermRequest.MicPermRequestCompleteEvent.AddListener(FetchMicAndStartRecording);
    }

    private void OnDestroy()
    {
        MicPermRequest.MicPermRequestCompleteEvent.RemoveListener(FetchMicAndStartRecording);

        //End the recording if it started
        EndRecording(ChosenMic);

        OnUserStartSpeaking.RemoveAllListeners();
        OnUserStopSpeaking.RemoveAllListeners();
    }

    private void Update()
    {
        if (IsMicConnected == false)
        {
            return;
        }

        //Start up the clip again if it stops to continue getting spectrum data
        if (AudioSrc.isPlaying == false)
        {
            AudioSrc.Play();
        }

        AudioSrc.GetSpectrumData(ClipSampleData, 0, FFTWindow.Rectangular);
        float curAverageVolume = Average(ClipSampleData);

#if UNITY_EDITOR
        //Debug.Log($"AVERAGE VOLUME: {curAverageVolume}");
#endif
        AverageSpeakingVolume = curAverageVolume;

        if (curAverageVolume > MinSpeakingLevel)
        {
            if (IsUserSpeaking == false)
            {
                IsUserSpeaking = true;
                OnUserStartSpeaking.Invoke();
            }
        }
        else
        {
            if (IsUserSpeaking == true)
            {
                //Stopped speaking
                IsUserSpeaking = false;
                OnUserStopSpeaking.Invoke();
            }
        }
    }

    private void FetchMicAndStartRecording()
    {
        if (Application.HasUserAuthorization(UserAuthorization.Microphone) == false)
        {
            Debug.Log("Cannot initialize microphone - permission denied.");

            enabled = false;
            return;
        }

        Debug.Log("Permission granted for microphone!");

        string[] availableMics = Microphone.devices;

        if (availableMics.Length <= 0)
        {
            Debug.LogWarning("No microphone connected!");

            enabled = false;
        }
        else
        {
            IsMicConnected = true;

            Microphone.GetDeviceCaps(null, out MinMicFrequency, out MaxMicFrequency);

            //If min and max frequency are 0, the microphone supports any frequency (according to Unity docs)
            if (MinMicFrequency == 0 && MaxMicFrequency == 0)
            {
                //Use 44100 Hz as the recording sampling rate  
                MaxMicFrequency = 44100;
            }

            StartRecording();
        }
    }

    public void StartRecording()
    {
        if (IsMicConnected == false)
        {
            Debug.Log("No mic connected to start recording. You may have to grant permission to use the microphone.");
            return;
        }

        AudioClip clip = Microphone.Start(ChosenMic, true, RecordClipDuration, MaxMicFrequency);

        if (clip == null)
        {
            Debug.LogWarning("Failed to start recording!");
            return;
        }

        AudioSrc.clip = clip;

        //Play audio source to get spectrum data
        //To avoid the user hearing it, add an AudioMixerGroup with the Attenuation at -80 db
        //Then, set that mixer group as the Output on the AudioSource
        //We do it this way because setting the AudioSource's volume to 0 causes no spectrum data to be output
        AudioSrc.Play();
    }

    public void EndRecording(string microphone)
    {
        if (IsMicConnected == false)
        {
            Debug.Log("No microphone connected to end recording.");
            return;
        }

        if (Microphone.IsRecording(microphone) == true)
        {
            Microphone.End(microphone);

            if (IsUserSpeaking == true)
            {
                IsUserSpeaking = false;
                OnUserStopSpeaking.Invoke();
            }
        }
    }

    private float Average(float[] data)
    {
        float sum = 0;

        for (int i = 0; i < data.Length; i++)
        {
            sum += data[i];
        }

        sum /= data.Length;

        return sum;
    }
}
