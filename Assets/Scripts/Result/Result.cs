using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Result : MonoBehaviour
{
    [SerializeField] Text TeamBlueScore;
    [SerializeField] Text TeamBlueExcaper;
    [SerializeField] Text TeamRedScore;
    [SerializeField] Text TeamRedExcaper;
    [SerializeField] Text TeamWon;

    Hashtable roomhash = new Hashtable();
    Hashtable hash = new Hashtable();
    // Start is called before the first frame update
    void Start()
    {
        int bluegetoutnumber = 0;
        int redgetoutnumber = 0;
        int bluescore = 0;
        int redscore = 0;
        Player[] players = PhotonNetwork.PlayerList;
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        for (int i = 0; i < players.Count(); i++)
        {
            if ((int)players[i].CustomProperties["WhichTeam"] == 1)
            {
                redscore += (int)players[i].CustomProperties["Point"];
                if ((bool)players[i].CustomProperties["GetOut"] == true)
                {
                    redgetoutnumber++;
                }
            }
            else
            {
                bluescore += (int)players[i].CustomProperties["Point"];
                if ((bool)players[i].CustomProperties["GetOut"] == true)
                {
                    bluegetoutnumber++;
                }
            }

        }
        Debug.Log(bluescore);
        TeamBlueScore.text = bluescore.ToString();
        TeamRedScore.text = redscore.ToString();
        TeamBlueExcaper.text = bluegetoutnumber.ToString();
        TeamRedExcaper.text = redgetoutnumber.ToString();
        if (bluegetoutnumber == 2)
        {
            TeamWon.text = "藍隊";
        }
        else if (redgetoutnumber == 2)
        {
            TeamWon.text = "紅隊";
        }
        else
        {
            if ((int)roomhash["BlueScore"] > (int)roomhash["RedScore"] || (int)roomhash["BlueScore"] == 25)
            {
                TeamWon.text = "藍隊";
            }
            else
            {
                TeamWon.text = "紅隊";
            }
        }
        roomhash["LoadingProgress"] = 0;
        roomhash["Choose"] = 0;
        roomhash["StartGame"] = false;
        roomhash["GameOver"] = false;
        roomhash["StartTime"] = 0;
        roomhash["Player1"] = 0;
        roomhash["Player2"] = 0;
        roomhash["Player3"] = 0;
        roomhash["Player4"] = 0;
        roomhash["BlueScore"] = 0;
        roomhash["RedScore"] = 0;
        hash["TimerReady"] = false;
        hash["Loading"] = false;
        hash["Ready"] = false;
        hash["GetOut"] = false;
        hash["Point"] = 0;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        Destroy(RoomManager.Instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}