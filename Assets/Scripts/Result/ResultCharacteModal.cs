using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ResultCharacteModal : MonoBehaviour
{
    public GameObject candy_model;
    public GameObject chocolat_model;
    public GameObject can_model;
    public GameObject ice_model;
    Hashtable roomhash = new Hashtable();
    Player[] players = PhotonNetwork.PlayerList;
    // Start is called before the first frame update
    void Start()
    {
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log(roomhash["Win"]);
        for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
        {
            if((string)roomhash["Win"] == "Blue")
            {
                if ((string)players[i].CustomProperties["WhichTeam"] == "藍隊")
                {
                    switch ((int)players[i].CustomProperties["Charactor"])
                    {
                        case 1:
                            candy_model.SetActive(true);
                            break;
                        case 2:
                            chocolat_model.SetActive(true);
                            break;
                        case 3:
                            can_model.SetActive(true);
                            break;
                        case 4:
                            ice_model.SetActive(true);
                            break;
                    }
                }
            }
            else
            {
                if ((string)players[i].CustomProperties["WhichTeam"] == "紅隊")
                {
                    switch ((int)players[i].CustomProperties["Charactor"])
                    {
                        case 1:
                            candy_model.SetActive(true);
                            break;
                        case 2:
                            chocolat_model.SetActive(true);
                            break;
                        case 3:
                            can_model.SetActive(true);
                            break;
                        case 4:
                            ice_model.SetActive(true);
                            break;
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
