using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public string Explainations;
    int count;
    public Text TextExplain, NameText, PhoneText;
    public GameObject NextButton,GetName,GetPhone,GetGoals,GetNotes,LoadingScreen;
    public Text[] GoalsText,NotesText;
    public Text progressTExt;
    public Image Slider;
    public int NextScene;
    AsyncOperation sync;
    Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        //  DontDestroyOnLoad(gameObject);
        StartCoroutine(PlayText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickNextIntro()
    {
        Invoke("GetNameEnabled", 1f);
        NextButton.SetActive(false);
    }

    IEnumerator PlayText()
    {
        //foreach (char c in Explainations)
        //{   if(count >0)
        //    {
                
        //    }
        //    TextExplain.text += c;
        //    yield return new WaitForSeconds(0.01f);
        //}
        //count++;
        yield return new WaitForSeconds(1.5f);
        //if(count < Explainations.Length)
        //{
        //    NextButton.SetActive(true);
        //}
        //else
        //{
            Invoke("GetNameEnabled", 1f);
        //}

    }

    public void OnClick(string name)
    {
        if(name == "OkayName")
        {
            TextExplain.text += "\n";
            TextExplain.text += "\n";
            TextExplain.text += "Your Name : " + NameText.text;
            TextExplain.text += "\n";
            TextExplain.text += "Phone Number : 650-387-9745" ;
            GetName.SetActive(false);
            Invoke("GetNotesEnabled", 1f);
        }

        else if (name == "OkayPhone")
        {
            TextExplain.text += "\n";
            TextExplain.text += "Phone Number : "+PhoneText.text;
            GetPhone.SetActive(false);
            Invoke("GetNotesEnabled", 1f);
        }

        else if (name == "OkayGoals")
        {
            PlayerPrefs.SetString("Goal0", GoalsText[0].text);
            PlayerPrefs.SetString("Goal1", GoalsText[1].text);
            PlayerPrefs.SetString("Goal2", GoalsText[2].text);
            PlayerPrefs.SetString("Goal3", GoalsText[3].text);
            GetGoals.SetActive(false);
            //Invoke("GetNotesEnabled", 1f);
        }

        else if (name == "OkayNotes")
        {
            PlayerPrefs.SetString("Note0", NotesText[0].text);
            PlayerPrefs.SetString("Note1", NotesText[1].text);
            PlayerPrefs.SetString("Note2", NotesText[2].text);
            PlayerPrefs.SetString("Note3", NotesText[3].text);
            //SceneManager.LoadScene(1);
            GetNotes.SetActive(false);
          //  Invoke("GetNotesEnabled", 1f);
            StartCoroutine(LoadingCoroutine());
        }
    }

    void GetNameEnabled()
    {
        GetName.SetActive(true);
    }
    void GetPhoneEnabled()
    {
        GetPhone.SetActive(true);
    }
    void GetGoalsEnabled()
    {
        GetGoals.SetActive(true);
    }
    void GetNotesEnabled()
    {
        GetNotes.SetActive(true);
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
