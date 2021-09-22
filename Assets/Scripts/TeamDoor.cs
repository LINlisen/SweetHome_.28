using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class TeamDoor : MonoBehaviour
{


    private bool IsRedTeam;
    public GameObject theDoor;
    Hashtable hash;
    void Start()
    {

        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        IsRedTeam = false;
    }

    
    void Update()
    {
        //Debug.Log(IsRedTeam);
        if (IsRedTeam==true)
        {
            
            theDoor.GetComponent<BoxCollider>().enabled = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")//the name of the player(different team)
        {
            if((int)hash["WhichTeam"] == 1)
            {
                IsRedTeam = true;
            }
           
        }
    }
}
