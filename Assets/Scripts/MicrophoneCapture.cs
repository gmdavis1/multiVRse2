using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FrostweepGames.Plugins.Native;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneCapture : MonoBehaviour
{
    public AudioSource AudioSrc { get; private set; } = null;

    public bool IsMicConnected { get; private set; } = false;

    private int MinMicFrequency = 0;
    private int MaxMicFrequency = 1;

    [SerializeField] private int RecordClipDuration = 60;
    [SerializeField] private double VoiceThreshold = .02d;
    [SerializeField] private float AverageVoiceLevel = .0001f;

    //null or an empty string indicates the default microphone
    private string ChosenMic = null;

    private float[] ClipSampleData = new float[1024];
    public bool IsUserSpeaking = false;

    public float AverageSpeakingVolume = 0f;

    public UnityEvent OnUserStartSpeaking = new UnityEvent();
    public UnityEvent OnUserStopSpeaking = new UnityEvent();

    private Coroutine MicFetchRoutine = null;

    private void Awake()
    {
        AudioSrc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        MicFetchRoutine = StartCoroutine(FetchMicAndStartRecording());
    }

    private void OnDestroy()
    {
        //End the recording if it started
        EndRecording(ChosenMic);

        OnUserStartSpeaking.RemoveAllListeners();
        OnUserStopSpeaking.RemoveAllListeners();

        if (MicFetchRoutine != null)
        {
            StopCoroutine(MicFetchRoutine);
            MicFetchRoutine = null;
        }
    }

    private void Update()
    {
        if (IsMicConnected == false || CustomMicrophone.HasMicrophonePermission() == false)
        {
            return;
        }

        bool isSpeaking = CustomMicrophone.IsVoiceDetected(ChosenMic, AudioSrc.clip, ref AverageVoiceLevel, VoiceThreshold);

        if (isSpeaking == true)
        {
            //Started speaking
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

        //Start up the clip again if it stops to continue getting spectrum data
        /*if (AudioSrc.isPlaying == false)
        {
            AudioSrc.Play();
        }

        AudioSrc.GetSpectrumData(ClipSampleData, 0, FFTWindow.Rectangular);
        float curAverageVolume = Average(ClipSampleData);

#if UNITY_EDITOR
        //Debug.Log($"AVERAGE VOLUME: {curAverageVolume}");
#endif
        AverageSpeakingVolume = curAverageVolume;

        if (curAverageVolume > VoiceThreshold)
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
        }*/
    }

    private IEnumerator FetchMicAndStartRecording()
    {
        while (CustomMicrophone.HasMicrophonePermission() == false)
        {
            yield return null;
        }

        Debug.Log("Permission granted for microphone!");

        string[] availableMics = CustomMicrophone.devices;

        if (availableMics.Length <= 0)
        {
            Debug.LogWarning("No microphone connected!");

            enabled = false;
        }
        else
        {
            IsMicConnected = true;

            CustomMicrophone.GetDeviceCaps(ChosenMic, out MinMicFrequency, out MaxMicFrequency);

            //If min and max frequency are 0, the microphone supports any frequency (according to Unity docs)
            if (MinMicFrequency == 0 && MaxMicFrequency == 0)
            {
                //Use 44100 Hz as the recording sampling rate  
                MaxMicFrequency = 44100;
            }

            Debug.Log($"Microphone has Min Frequency = {MinMicFrequency} and Max Frequency = {MaxMicFrequency}");

            StartRecording();
        }

        MicFetchRoutine = null;
    }

    public void StartRecording()
    {
        if (CustomMicrophone.HasMicrophonePermission() == false)
        {
            Debug.Log("No permission to use the microphone - unable to start recording.");
            return;
        }

        if (IsMicConnected == false)
        {
            Debug.Log("No mic connected to start recording.");
            return;
        }

        AudioClip clip = CustomMicrophone.Start(ChosenMic, true, RecordClipDuration, MaxMicFrequency);

        if (clip == null)
        {
            Debug.LogWarning($"Failed to start recording - {nameof(AudioClip)} from {nameof(CustomMicrophone.Start)} is null!");
            return;
        }

        AudioSrc.clip = clip;

        //Play audio source to get spectrum data
        //To avoid the user hearing it, add an AudioMixerGroup with the Attenuation at -80 db
        //Then, set that mixer group as the Output on the AudioSource
        //We do it this way because setting the AudioSource's volume to 0 causes no spectrum data to be output
        //AudioSrc.Play();
    }

    public void EndRecording(string microphone)
    {
        if (CustomMicrophone.HasMicrophonePermission() == false)
        {
            Debug.Log("No permission to use the microphone - unable to end recording.");
            return;
        }

        if (IsMicConnected == false)
        {
            Debug.Log("No microphone connected to end recording.");
            return;
        }

        if (CustomMicrophone.IsRecording(microphone) == true)
        {
            CustomMicrophone.End(microphone);

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
