using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class GameManagement : MonoBehaviourPunCallbacks
{

    [SerializeField] public GameObject setPage;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] Transform BlueListContent;
    [SerializeField] Transform RedListContent;
    [SerializeField] GameObject BlindVision;
    Hashtable hash = new Hashtable();
    Hashtable roomhash = new Hashtable();
    Player[] players = PhotonNetwork.PlayerList;
    void Start()
    {
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log(hash);
        Debug.Log(roomhash);

    }

    // Update is called once per frame
    void Update()
    {


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
        PhotonNetwork.LoadLevel(0);
        //MenuManager.Instance.OpenMenu("title");
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        int bluegetoutnumber = 0;
        int redgetoutnumber = 0;
        for (int i = 0; i < players.Count(); i++)
        {
            if ((bool)players[i].CustomProperties["GetOut"] == true)
            {
                if ((int)players[i].CustomProperties["WhichTeam"] == 1)
                {
                    redgetoutnumber++;
                }
                else
                {
                    bluegetoutnumber++;
                }
            }
        }
        if (bluegetoutnumber == 2 || redgetoutnumber == 2)
        {
            PhotonNetwork.LoadLevel(2);
        }
        /*Can Skill*/
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Blind"] == true)
        {
            BlindVision.SetActive(true);
        }
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {

        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (PhotonNetwork.CurrentRoom.CustomProperties["BlueScore"]==null || PhotonNetwork.CurrentRoom.CustomProperties["RedScore"]==null)
        {
            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["BlueScore"] == 25 || (int)PhotonNetwork.CurrentRoom.CustomProperties["RedScore"] == 25)
            {
                PhotonNetwork.LoadLevel(2);
            }
            else if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["GameOver"] == true)
            {
                PhotonNetwork.LoadLevel(2);
            }
        }
        
    }
}