using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MechanicSceneUIManager : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image Slider;
    public Text GoalsText,NotesText,WebText;
    public int NextScene;
    AsyncOperation sync;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        StartCoroutine(LoadingCoroutine());
    }

    IEnumerator LoadingCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        LoadingScreen.SetActive(true);
        sync = SceneManager.LoadSceneAsync(NextScene);
        sync.allowSceneActivation = false;
        while (sync.isDone == false)
        {
            yield return new WaitForSeconds(2.9f);

            Slider.fillAmount = sync.progress - 0.8f;
            yield return new WaitForSeconds(0.9f);
            if (sync.progress == 0.9f)
            {
                Slider.fillAmount = 1f;
                sync.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
