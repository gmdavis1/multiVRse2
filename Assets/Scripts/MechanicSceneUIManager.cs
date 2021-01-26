using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MechanicSceneUIManager : MonoBehaviour
{
    [SerializeField] private GameObject IntroCanvas = null;

    public GameObject LoadingScreen;
    public Image Slider;
    public Text GoalsText,NotesText,WebText;
    public string NextSceneName = string.Empty;
    AsyncOperation sync;

    private void Awake()
    {
        IntroCanvas.SetActive(true);
    }

    void Start()
    {
     //   GoalsText.text = "YOUR GOALS";
        NotesText.text = "YOUR NOTES";
        for (int i=0;i<4;i++)
        {
           // GoalsText.text += "\n" + "- " + PlayerPrefs.GetString("Goal" + i.ToString());
            NotesText.text += "\n" + "- " + PlayerPrefs.GetString("Note" + i.ToString());
        }
        
    }

    public void OnClick()
    {
        StartCoroutine(LoadingCoroutine());
    }

    public void OpenIntroCanvas()
    {
        IntroCanvas.SetActive(true);
    }

    IEnumerator LoadingCoroutine()
    {
        LoadingScreen.SetActive(true);
        sync = SceneManager.LoadSceneAsync(NextSceneName, LoadSceneMode.Single);
        sync.allowSceneActivation = true;
        while (sync.isDone == false)
        {
            yield return null;
            Slider.fillAmount = sync.progress;
        }
    }
}
