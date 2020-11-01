using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    [HideInInspector]
    public bool flag = false;

    // Use this for initialization
    void Start()
    {


    }

    void Update()
    {
        if (flag)
        {
            if (transform.position.z == currentView.position.z)
            {
                flag = false;
                Debug.Log("okay");
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            flag = true;
            currentView = views[0];
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            flag = true;
            currentView = views[1];
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            flag = true; 
            currentView = views[2];
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    currentView = views[3];
        //}

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    currentView = views[4];
        //}

    }


    void LateUpdate()
    {
        if (flag)
        {
            if (transform.position.z == currentView.position.z)
            {
                flag = false;
            }
            //Lerp position
            transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

            Vector3 currentAngle = new Vector3(
             Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
             Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
             Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

            transform.eulerAngles = currentAngle;
            

        }
    }

    public void Transition(string name)
    {
        flag = true;
        if (name == "A")
        {
            currentView = views[0];
        }
        else if (name == "B")
        {
            currentView = views[1];
        }
        else if (name == "C")
        {
            currentView = views[2];
        }
    }
}
