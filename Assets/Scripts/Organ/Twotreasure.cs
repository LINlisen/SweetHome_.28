using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class TwoTreasure : MonoBehaviour
{

    Hashtable hash;
    public bool playerOn;
    public int playerCount;

    //public GameObject potion1;
    //public GameObject potion2;
    public GameObject potionSet;

    [SerializeField]public bool CanOpenTreasure;//treasure trigger flag


   

    void Start()
    {
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        playerCount = 0;
        playerOn = false;
        
        CanOpenTreasure = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerCount);
        if (playerCount == 2)
        {
            Debug.Log("two persoon & treasure avalible");
            CanOpenTreasure = !CanOpenTreasure;
        }
        if (CanOpenTreasure)
        {
            potionSet.gameObject.SetActive(true);
            
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
