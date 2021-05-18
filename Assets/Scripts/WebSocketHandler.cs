using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NativeWebSocket;

public class WebSocketHandler : MonoBehaviour
{
    private const string SOCKET_URL = "wss://e5pvp3ghmc.execute-api.us-east-2.amazonaws.com/Prod";

    public OnSocketMessageReceived SocketMessageEvent = new OnSocketMessageReceived();

    private WebSocket Socket = null;

    private async void Start()
    {
        Socket = new WebSocket(SOCKET_URL);

        Socket.OnOpen -= OnSocketOpened;
        Socket.OnOpen += OnSocketOpened;

        Socket.OnError -= OnSocketError;
        Socket.OnError += OnSocketError;

        Socket.OnClose -= OnSocketClose;
        Socket.OnClose += OnSocketClose;

        Socket.OnMessage -= OnSocketMessage;
        Socket.OnMessage += OnSocketMessage;

        //Waiting for messages
        await Socket.Connect();
    }

    private void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
            Socket.DispatchMessageQueue();
        #endif

        //if (Input.GetKeyDown(KeyCode.Space) == true)
        //{
        //    SendSocketMessage();
        //}
    }

    private void OnDestroy()
    {
        SocketMessageEvent.RemoveAllListeners();
    }

    private async void SendSocketMessage()
    {
        //using (HttpClient client = new HttpClient())
        //{
        //    using (var request = new HttpRequestMessage(HttpMethod.Post, "https://dc368368.herokuapp.com/"))
        //    {
        //        string json = JsonUtility.ToJson(new SrcAction { action = "sendmessage" }, true);
        //        Debug.Log($"JSON:\n{json}");
        //
        //        request.Content = new StringContent(json);
        //
        //        using (HttpResponseMessage response = await client.SendAsync(request))
        //        {
        //            Debug.Log($"Response Success = {response.IsSuccessStatusCode} | Code: {response.StatusCode} ({(int)response.StatusCode})");
        //
        //            //await Socket.Receive();
        //        }
        //    }
        //}

        /*if (Socket.State == WebSocketState.Open)
        {
            Debug.Log("Sending text...");

            string text = JsonUtility.ToJson(new SrcAction { action = "sendmessage" }, true);

            await Socket.SendText(text);

            Debug.Log("Send complete!");
        }*/

        //await Socket.Receive();
    }

    private async void OnApplicationQuit()
    {
        try
        {
            if (Socket != null)
            {
                await Socket.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Exception closing socket: {e.Message}");
        }
    }

    private void OnSocketError(string errorMsg)
    {
        Debug.Log($"Socket encountered error: {errorMsg}");
    }

    private void OnSocketOpened()
    {
        Debug.Log("Connection open!");
    }

    private void OnSocketClose(WebSocketCloseCode closeCode)
    {
        Debug.Log($"Connection closed with code {closeCode} ({(int)closeCode})!");
    }

    private void OnSocketMessage(byte[] data)
    {
        //Get the message as a string
        string message = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log($"Message received | Data length: {data.Length} | Message: {message}");

        SocketData socketData = null;

        try
        {
            socketData = JsonUtility.FromJson<SocketData>(message);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing JSON: {e.Message}");
            return;
        }

        if (System.Enum.TryParse(socketData.action, true, out SocketActions socketAction) == false)
        {
            Debug.LogWarning($"Received unrecognized socket action: {socketData.action}");
            return;
        }

        SocketResponse socketResponse = new SocketResponse(socketData.scene, socketAction, socketData.value);

        SocketMessageEvent.Invoke(socketResponse);
    }

    [System.Serializable]
    public class OnSocketMessageReceived : UnityEvent<SocketResponse>
    {

    }

    [System.Serializable]
    public struct SocketResponse
    {
        public string Scene;
        public SocketActions Action;
        public string Value;

        public SocketResponse(string scene, in SocketActions action, string value)
        {
            Scene = scene;
            Action = action;
            Value = value;
        }
    }

    [System.Serializable]
    private class SocketData
    {
        public string scene = string.Empty;
        public string action = string.Empty;
        public string value = string.Empty;
    }

    //[System.Serializable]
    //public class SrcAction
    //{
    //    public string action = string.Empty;
    //}
}
