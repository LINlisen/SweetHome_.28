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
    public GameObject pos_1;
    public GameObject pos_2;
    Hashtable roomhash = new Hashtable();
    Player[] players = PhotonNetwork.PlayerList;
    int n = 0;
    int[] winner = new int[2];
    // Start is called before the first frame update
    void Start()
    {
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        candy_model.GetComponent<Animator>().SetTrigger("Celebrate");
        chocolat_model.GetComponent<Animator>().SetTrigger("Celebrate");
        can_model.GetComponent<Animator>().SetTrigger("Celebrate");
        ice_model.GetComponent<Animator>().SetTrigger("Celebrate");
        Debug.Log(roomhash["Win"]);
        for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
        {
            if ((string)roomhash["Win"] == "Blue")
            {
                if ((string)players[i].CustomProperties["WhichTeam"] == "藍隊")
                {
                    winner[n] = i;
                    n++;
                }
            }
            else
            {
                if ((string)players[i].CustomProperties["WhichTeam"] == "紅隊")
                {
                    winner[n] = i;
                    n++;
                }
            }
        }
        switch ((int)players[winner[0]].CustomProperties["Charactor"])
        {
            case 1:
                candy_model.SetActive(true);
                candy_model.transform.position = pos_1.transform.position;
                candy_model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
            case 2:
                chocolat_model.SetActive(true);
                chocolat_model.transform.position = pos_1.transform.position;
                chocolat_model.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
                break;
            case 3:
                can_model.SetActive(true);
                can_model.transform.position = pos_1.transform.position;
                can_model.transform.localScale = new Vector3(14.0f, 14.0f, 14.0f);
                break;
            case 4:
                ice_model.SetActive(true);
                ice_model.transform.position = pos_1.transform.position;
                ice_model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
        }
        switch ((int)players[winner[1]].CustomProperties["Charactor"])
        {
            case 1:
                candy_model.SetActive(true);
                candy_model.transform.position = pos_2.transform.position;
                candy_model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
            case 2:
                chocolat_model.SetActive(true);
                chocolat_model.transform.position = pos_2.transform.position;
                chocolat_model.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
                break;
            case 3:
                can_model.SetActive(true);
                can_model.transform.position =  pos_2.transform.position;
                can_model.transform.localScale = new Vector3(14.0f, 14.0f, 14.0f);
                break;
            case 4:
                ice_model.SetActive(true);
                ice_model.transform.position = pos_2.transform.position;
                ice_model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
