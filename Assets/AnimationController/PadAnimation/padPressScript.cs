using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class padPressScript : MonoBehaviour
{
    Animator animator;
    public bool hasPressed;
    [SerializeField]
    PlayableDirector buttonTimeline;
    [SerializeField]
    TimelineAsset btnPressed;
    [SerializeField]
    TimelineAsset btnunPressed;
    private bool _bplay;
    void Start()
    {
        animator = GetComponent<Animator>();
        hasPressed = false;
        buttonTimeline = gameObject.GetComponent<PlayableDirector>();
        _bplay = false;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
