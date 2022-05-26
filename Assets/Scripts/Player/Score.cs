using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Score : MonoBehaviour
{
    //C#檔放在兩隊分數的Canvas裡

    [SerializeField] Text teambluepoint;
    [SerializeField] Text teamredpoint;
    int redpoint;
    int bluepoint;
    int _PotionNum;
    bool _bPotionOut;
    Hashtable roomhash;
    Hashtable hash;
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
    void getPoint(string team, Player player)
    {
        if(player == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("getPotion");
        }
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
    void losePoint(string team, Player player)
    {
        Debug.Log("losePotion");
        hash = player.CustomProperties;
        hash["Point"] = (int)hash["Point"] - 1;
        player.SetCustomProperties(hash);
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
    [PunRPC]
    void PotionOut(string Playername,bool havePoion)
    {
        Debug.Log("RPC Poition out");
        GameObject Potion;
        Vector3 iniPos = GameObject.Find(Playername).gameObject.transform.position + new Vector3(10, 0, 10);
        if (havePoion)
        {
            Potion = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Potion"), iniPos, GameObject.Find(Playername).gameObject.transform.rotation);
            _PotionNum++;
            Potion.name = "00" + _PotionNum;
        }
        GameObject.Find(Playername).GetComponent<Animator>().SetTrigger("Wounded");
        GameObject.Find(Playername).GetComponent<PlayerController>()._bWounded = false;
        //StartCoroutine(SetWoundedFalse(3.0f, Playername));
    }
}