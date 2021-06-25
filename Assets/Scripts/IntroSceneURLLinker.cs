using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroSceneURLLinker : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenURLInTab(string url);
    
    [SerializeField] private TextMeshProUGUI URLText = null;
    [SerializeField] private Button URLButton = null;
    [SerializeField] SceneURLData[] SceneURLs = System.Array.Empty<SceneURLData>();

    private void Start()
    {
        string prevScene = SceneLoader.Instance.PrevScene;

        URLButton.interactable = false;

        for (int i = 0; i < SceneURLs.Length; i++)
        {
            ref SceneURLData sceneURLData = ref SceneURLs[i];

            if (sceneURLData.PrevSceneName == prevScene)
            {
                URLText.text = "Click here to open ";

                if (prevScene == "")
                {
                    URLText.text += "the consent form";
                }
                else
                {
                    URLText.text += "the survey after " + prevScene + " scene";
                }

                //URLText.text = sceneURLData.URL;
                URLButton.interactable = true;
                break;
            }
        }
    }

    public void OnURLClicked()
    {
        string prevScene = SceneLoader.Instance.PrevScene;

        for (int i = 0; i < SceneURLs.Length; i++)
        {
            ref SceneURLData sceneURLData = ref SceneURLs[i];

            if (sceneURLData.PrevSceneName == prevScene)
            {
                string link = sceneURLData.URL;
                #if !UNITY_EDITOR && UNITY_WEBGL
                    OpenURLInTab(link);
                #else
                    Application.OpenURL(link);
                #endif
            }
        }
    }

    [System.Serializable]
    public struct SceneURLData
    {
        public string PrevSceneName;
        public string URL;
    }
}
