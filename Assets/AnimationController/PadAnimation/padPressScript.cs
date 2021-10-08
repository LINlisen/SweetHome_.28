using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class padPressScript : MonoBehaviour
{
    Animator animator;
    public bool hasPressed;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        hasPressed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (hasPressed)
        {
            animator.SetBool("hasPressed", true);
        }
        else
        {
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
