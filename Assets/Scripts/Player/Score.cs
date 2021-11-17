using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Score : MonoBehaviour
{
    //C#檔放在兩隊分數的Canvas裡

    [SerializeField] TMP_Text teambluepoint;
    [SerializeField] TMP_Text teamredpoint;
    int redpoint;
    int bluepoint;
    Hashtable roomhash;
    // Start is called before the first frame update
    void Start()
    {
        redpoint = 0;
        bluepoint = 0;
        teamredpoint.SetText(redpoint.ToString());
        teambluepoint.SetText(bluepoint.ToString());
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    void getPoint(int team)
    {
        Debug.Log("getPotion");
        if (team == 1)
        {
            redpoint++;
            teamredpoint.SetText(redpoint.ToString());
            roomhash["RedScore"] = redpoint;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        }
        else
        {
            bluepoint++;
            teambluepoint.SetText(bluepoint.ToString());
            roomhash["BlueScore"] = bluepoint;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        }
    }

    //下面三行擺到在判斷拿到藥水的函式裡
    //Hashtable team = PhotonNetwork.LocalPlayer.CustomProperties;
    //PhotonView photonView = PhotonView.Get(this);
    //photonView.RPC("getPoint", RpcTarget.All,(int)team["WhichTeam"]);
}