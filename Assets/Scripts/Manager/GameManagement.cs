using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class GameManagement : MonoBehaviour
{

    [SerializeField] public GameObject setPage;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] Transform BlueListContent;
    [SerializeField] Transform RedListContent;
    Hashtable hash = new Hashtable();
    void Start()
    {
        
        PlayerLeaderBoardInfoPlayerList();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void PlayerLeaderBoardInfoPlayerList()
    {
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in BlueListContent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in RedListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
        {
            hash = players[i].CustomProperties;
            if ((int)hash["WhichTeam"] == 1)
            {
                Instantiate(PlayerListItemPrefab, RedListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            else
            {
                Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
        }
    }
    public void QuitGameSingle()
    {
        Application.Quit();
    }
    public void OpenSettingBox()
    {
        setPage.SetActive(true);
    }
    public void CloseSettingBox()
    {
        setPage.SetActive(false);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //MenuManager.Instance.OpenMenu("title");
    }

}
