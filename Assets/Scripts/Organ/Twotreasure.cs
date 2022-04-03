using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
public class TwoTreasure : MonoBehaviour
{
    public bool playerOn;
    public int playerCount;

    public GameObject potion1;
    public GameObject potion2;


    [SerializeField]public bool CanOpenTreasure;//treasure trigger flag


   

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
        if (CanOpenTreasure)
        {
            potion1.gameObject.SetActive(true);
            potion2.gameObject.SetActive(true);
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
