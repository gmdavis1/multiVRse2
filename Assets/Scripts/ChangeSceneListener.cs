using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ChangeSceneListener : MonoBehaviour
{
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

        if (socketResponse.Action != SocketActions.changescene)
        {
            return;
        }

        //Change scene
        SceneLoader.Instance.LoadScene(socketResponse.Value, LoadSceneMode.Single);
    }
}
