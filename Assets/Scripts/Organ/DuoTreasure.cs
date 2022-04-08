using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class DuoTreasure : MonoBehaviour
{

    Hashtable hash;
    public bool playerOn;
    public int playerCount;

    //public GameObject potion1;
    //public GameObject potion2;
    public GameObject potionSet;

    [SerializeField] public bool CanOpenTreasure;//treasure trigger flag

    Hashtable CanOpen = new Hashtable();
    Hashtable PlayerOnTopCount = new Hashtable();

    void Start()
    {
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        playerCount = 0;
        playerOn = false;
        CanOpenTreasure = false;

        CanOpen.Add("DuoTreasureState", false);
        PlayerOnTopCount.Add("PlayerOnDuoTreasure", 0);
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
            potionSet.gameObject.SetActive(true);

        }

        //----muti code------
        if ((int)PlayerOnTopCount["PlayerOnDuoTreasure"]==2)
        {
            CanOpen["DuoTreasureState"] = true;
        }

        if (CanOpen["DuoTreasureState"] == null)
        {
            return;
        }
        else
        {
            if ((bool)CanOpen["DuoTreasureState"] == true)
            {
                potionSet.gameObject.SetActive(true);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        playerOn = true;
        playerCount++;
        PlayerOnTopCount["PlayerOnDuoTreasure"] = (int)PlayerOnTopCount["PlayerOnDuoTreasure"] + 1;

    }
    private void OnTriggerExit(Collider other)
    {
        playerOn = false;
        playerCount--;
        PlayerOnTopCount["PlayerOnDuoTreasure"] = (int)PlayerOnTopCount["PlayerOnDuoTreasure"] - 1;
    }
}


