using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Debriefing : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenURLInTab(string url);

    public void OpenURL()
    {
        string link = "https://stanforduniversity.qualtrics.com/jfe/form/SV_7QXqGaXrlMFHcVg?Q_Language=ZH-S";
        #if !UNITY_EDITOR && UNITY_WEBGL
            OpenURLInTab(link);
        #else
            Application.OpenURL(link);
        #endif
    }
}
