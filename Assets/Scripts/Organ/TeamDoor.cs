using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class TeamDoor : MonoBehaviour
{

    private bool IsRedTeam;
    private bool IsBlueTeam;

    [SerializeField] public bool BlueDoor;

    public GameObject theDoor;
    Hashtable hash;
    void Start()
    {

        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        switch ((int)hash["WhichTeam"])
        {
            case 0://blue team
                
                
                break;

            case 1://red team
                
                
                break;

        }
        

    }

    
    void Update()
    {
        //Debug.Log(IsRedTeam);
        if (IsRedTeam==true)
        {
            if (!BlueDoor)
            {
                theDoor.GetComponent<BoxCollider>().enabled = false;
            }
        }
        if (IsBlueTeam == true)
        {
            if (BlueDoor)
            {
                theDoor.GetComponent<BoxCollider>().enabled = false;
            }
           
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
            if ((int)hash["WhichTeam"] == 0)
            {
                IsBlueTeam = true;
            }

        }
    }
}
