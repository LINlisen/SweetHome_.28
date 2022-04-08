using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class DuoTreasure : MonoBehaviour
{

    public bool playerOn;
    public int playerCount;

    //public GameObject potion1;
    //public GameObject potion2;
    public GameObject potionSet;

    [SerializeField] public bool CanOpenTreasure;//treasure trigger flag

    Hashtable roomhash = new Hashtable();

    void Start()
    {
        playerCount = 0;
        playerOn = false;
        CanOpenTreasure = false;
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        roomhash.Add("DuoTreasureState", false);
        roomhash.Add("PlayerOnDuoTreasure", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(roomhash["PlayerOnDuoTreasure"]);
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
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["PlayerOnDuoTreasure"]==2)
        {
            PhotonNetwork.CurrentRoom.CustomProperties["DuoTreasureState"] = true;
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties["DuoTreasureState"] == null)
        {
            return;
        }
        else
        {
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["DuoTreasureState"] == true)
            {
                potionSet.gameObject.SetActive(true);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        playerOn = true;
        playerCount++;
        roomhash["PlayerOnDuoTreasure"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["PlayerOnDuoTreasure"] + 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
    }
    private void OnTriggerExit(Collider other)
    {
        playerOn = false;
        playerCount--;
        roomhash["PlayerOnDuoTreasure"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["PlayerOnDuoTreasure"] - 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
    }
}


