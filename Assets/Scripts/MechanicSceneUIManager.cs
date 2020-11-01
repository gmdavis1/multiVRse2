using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechanicSceneUIManager : MonoBehaviour
{
    public Text GoalsText,NotesText;
    // Start is called before the first frame update
    void Start()
    {
        GoalsText.text = "YOUR GOALS";
        NotesText.text = "YOUR NOTES";
        for (int i=0;i<4;i++)
        {
            GoalsText.text += "\n" + "- " + PlayerPrefs.GetString("Goal" + i.ToString());
            NotesText.text += "\n" + "- " + PlayerPrefs.GetString("Note" + i.ToString());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
