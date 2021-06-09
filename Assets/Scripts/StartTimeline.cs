using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class StartTimeline : MonoBehaviour
{
    [SerializeField] PlayableDirector Director = null;

    public void PlayTimeline()
    {
        Director.Play();
    }
}
