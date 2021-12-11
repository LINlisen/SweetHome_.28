using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;

public class ResultListInistiate : MonoBehaviour
{
    [SerializeField] GameObject ResultListItemPrefab;
    [SerializeField] Transform BlueListContent;
    [SerializeField] Transform RedListContent;
    Hashtable hash = new Hashtable();
    Player[] players = PhotonNetwork.PlayerList;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
        {
            hash = players[i].CustomProperties;
            if ((int)players[i].CustomProperties["WhichTeam"] == 1)
            {
                Instantiate(ResultListItemPrefab, RedListContent).GetComponent<ResultListItem>().SetUp(players[i]);
            }
            else
            {
                Instantiate(ResultListItemPrefab, BlueListContent).GetComponent<ResultListItem>().SetUp(players[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
