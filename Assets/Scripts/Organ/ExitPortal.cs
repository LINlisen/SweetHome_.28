using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ExitPortal : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField] Text teambluegetout;
    [SerializeField] Text teamredgetout;

    Hashtable hash = new Hashtable();
    Player[] players = PhotonNetwork.PlayerList;
    // Start is called before the first frame update
    void Start()
    {
        teamredgetout.text = "紅隊逃出人數: 0 ";
        teambluegetout.text = "藍隊逃出人數: 0 ";
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
        {
            hash = players[i].CustomProperties;
            if ((bool)hash["GetOut"] == true)
            {
                if ((int)hash["WhichTeam"] == 1)
                {
                    teamredgetout.text = "紅隊逃出人數: 1 ";
                }
                else
                {
                    teambluegetout.text = "藍隊逃出人數: 1 ";
                }
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        //int character = 0;
        //if (col.gameObject.GetPhotonView().Controller == PhotonNetwork.LocalPlayer)
        //{
        //    playerController = col.gameObject.GetPhotonView().GetComponent<PlayerController>();
        //    for (int i = 0; i < players.Count(); i++)
        //    {
        //        //Debug.Log((int)players[i].CustomProperties["WhichTeam"]);
        //        if ((int)players[i].CustomProperties["WhichTeam"] == (int)PhotonNetwork.LocalPlayer.CustomProperties["WhichTeam"] && players[i] != PhotonNetwork.LocalPlayer)
        //        {
        //            character = (int)players[i].CustomProperties["Charactor"];
        //        }
        //    }
        //    playerController.Excape(character);
        //    hash["GetOut"] = true;
        //    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        //}
    }
    private void OnTriggerExit(Collider col)
    {
        //if (col.gameObject.GetPhotonView().Controller == PhotonNetwork.LocalPlayer)
        //{
        //    Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        //    hash["GetOut"] = false;
        //    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        //}
    }
}
