using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoTreasure : MonoBehaviour
{
    public bool playerOn;
    public int playerCount;

    [SerializeField]
    public bool CanOpenTreasure;//treasure trigger flag

    void Start()
    {
        playerCount = 0;
        playerOn = false;

        CanOpenTreasure = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCount == 2)
        {
            Debug.Log("two persoon & treasure avalible");
            CanOpenTreasure = !CanOpenTreasure;
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
