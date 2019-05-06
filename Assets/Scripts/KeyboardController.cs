using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            Vector3 target = transform.TransformDirection(new Vector3(-0.1f, 0, 0));
            target.y = 0;
            transform.position += target;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            Vector3 target = transform.TransformDirection(new Vector3(0.1f, 0, 0));
            target.y = 0;
            transform.position += target;
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            Vector3 target = transform.TransformDirection(new Vector3(0, 0, 0.1f));
            target.y = 0;
            transform.position += target;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            Vector3 target = transform.TransformDirection(new Vector3(0, 0, -0.1f));
            target.y = 0;
            transform.position += target;
        }
    }
}