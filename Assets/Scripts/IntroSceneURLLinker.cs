using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroSceneURLLinker : MonoBehaviour
{
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
                URLText.text = sceneURLData.URL;
                URLButton.interactable = true;
                break;
            }
        }
    }

    public void OnURLClicked()
    {
        Application.OpenURL(URLText.text);
    }

    [System.Serializable]
    public struct SceneURLData
    {
        public string PrevSceneName;
        public string URL;
    }
}
