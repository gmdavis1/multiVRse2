using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicPermissionRequest : MonoBehaviour
{
    [SerializeField] private bool RequestOnStart = false;

    public bool CurrentlyRequestingPerms => (RequestingMicRoutine != null);

    private Coroutine RequestingMicRoutine = null;

    private void Start()
    {
        if (RequestOnStart == true)
        {
            RequestMicPermission();
        }
    }

    public void RequestMicPermission()
    {
        if (CurrentlyRequestingPerms == true)
        {
            Debug.LogError("Already requesting mic permissions! Wait for this to finish before sending another request.");
            return;
        }

        RequestingMicRoutine = StartCoroutine(RequestMic());
    }

    private IEnumerator RequestMic()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        AsyncOperation asyncOp = Application.RequestUserAuthorization(UserAuthorization.Microphone);

        while (asyncOp.isDone == false)
        {
            yield return null;
        }

        RequestingMicRoutine = null;
#else 
        RequestingMicRoutine = null;
        yield break;
#endif
    }
}
