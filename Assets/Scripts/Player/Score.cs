using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Score : MonoBehaviour
{
    //C#檔放在兩隊分數的Canvas裡

    [SerializeField] Text teambluepoint;
    [SerializeField] Text teamredpoint;
    int redpoint;
    int bluepoint;
    Hashtable roomhash;
    // Start is called before the first frame update
    void Start()
    {
        redpoint = 0;
        bluepoint = 0;
        teamredpoint.text = redpoint.ToString();
        teambluepoint.text = bluepoint.ToString();
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    void getPoint(string team)
    {
        Debug.Log("getPotion");
        if (team == "紅隊")
        {
            redpoint++;
            teamredpoint.text = redpoint.ToString();
            roomhash["RedScore"] = redpoint;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        }
        else
        {
            bluepoint++;
            teambluepoint.text = bluepoint.ToString();
            roomhash["BlueScore"] = bluepoint;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        }
    }
    [PunRPC]
    void losePoint(string team)
    {
        Debug.Log("getPotion");
        if (team == "紅隊")
        {
            redpoint--;
            teamredpoint.text = redpoint.ToString();
            roomhash["RedScore"] = redpoint;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        }
        else
        {
            bluepoint--;
            teambluepoint.text = bluepoint.ToString();
            roomhash["BlueScore"] = bluepoint;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);

        }
    }
    //下面三行擺到在判斷拿到藥水的函式裡
    //Hashtable team = PhotonNetwork.LocalPlayer.CustomProperties;
    //PhotonView photonView = PhotonView.Get(this);
    //photonView.RPC("getPoint", RpcTarget.All,(int)team["WhichTeam"]);
}