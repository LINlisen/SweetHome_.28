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
        if (hasPressed)
        {
            //buttonTimeline.SetGenericBinding(animator, null);
            buttonTimeline.playableAsset = btnunPressed;
            if (!_bplay)
            {
                _bplay = true;
                buttonTimeline.Play();
                Debug.Log("playdown");
            }
            animator.SetBool("hasPressed", true);
        }
        else
        {
            //buttonTimeline.SetGenericBinding(animator, null);
            buttonTimeline.playableAsset = btnPressed;
            if (_bplay)
            {
                _bplay = false;
                buttonTimeline.Play();
                Debug.Log("playup");
            }
            animator.SetBool("hasPressed", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hasPressed = true;
        }

        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hasPressed = false;
        }
    }
}
