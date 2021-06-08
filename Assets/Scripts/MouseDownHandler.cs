using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseDownHandler : MonoBehaviour
{
    public UnityEvent MouseDownEvent = new UnityEvent();

    private void OnDestroy()
    {
        MouseDownEvent.RemoveAllListeners();
    }

    private void OnMouseDown()
    {
        MouseDownEvent.Invoke();
    }
}
