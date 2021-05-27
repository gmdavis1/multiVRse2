using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FrostweepGames.Plugins.Native;

public class MicPermissionRequest : MonoBehaviour
{
    [SerializeField] private bool RequestOnStart = false;

    private void Start()
    {
        if (RequestOnStart == true)
        {
            RequestMicPermission();
        }
    }

    private void OnDestroy()
    {

    }

    public void RequestMicPermission()
    {
        CustomMicrophone.RequestMicrophonePermission();
    }
}
