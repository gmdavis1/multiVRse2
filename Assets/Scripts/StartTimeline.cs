using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class StartTimeline : MonoBehaviour
{
    [SerializeField] PlayableDirector Director = null;
    [SerializeField] private Collider ColliderToDisable = null;

    public void PlayTimeline()
    {
        Director.Play();

        if (ColliderToDisable != null)
        {
            ColliderToDisable.enabled = false;
        }
    }
}
