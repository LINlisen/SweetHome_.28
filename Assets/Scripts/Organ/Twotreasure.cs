using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twotreasure : MonoBehaviour
{
    public bool playerOn;
    public int playerCount;

    void Start()
    {
        playerCount = 0;
        playerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCount == 2)
        {
            Debug.Log("two persoon");
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        playerOn = true;
        playerCount++;
    }
    private void OnTriggerExit(Collider other)
    {
        playerOn = false;
        playerCount--;

    }
}
