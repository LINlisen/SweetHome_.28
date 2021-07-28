using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    Hashtable nickname = new Hashtable();
    public static Launcher Instance;
    [SerializeField] TMP_InputField playerNicknameInputField;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] Transform BlueListContent;
    [SerializeField] Transform RedListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuManager.Instance.OpenMenu("loading");
        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        if( nickname["Nickname"] == null)
        {
            MenuManager.Instance.OpenMenu("nickname");
        }
        else
        {
            MenuManager.Instance.OpenMenu("title");
        }
        Debug.Log("Joined Lobby");
        
    }

    public void setNickname()
    {
        nickname.Add("Nickname",playerNicknameInputField.text);
        PhotonNetwork.NickName = playerNicknameInputField.text;
        MenuManager.Instance.OpenMenu("title");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.NickName = "<sprite=0>" + playerNicknameInputField.text;
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Hashtable hash = new Hashtable();
        hash.Add("WhichTeam", 0);
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
            if (i % 2 != 0)
            {
                Instantiate(PlayerListItemPrefab, RedListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
                hash["WhichTeam"] = 1; //紅隊為1
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
            else
            {
                Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
                hash["WhichTeam"] = 0; //藍隊為0
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode,string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name );
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Player[] players = PhotonNetwork.PlayerList;
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
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
            Hashtable team = players[i].CustomProperties;
            if ((int)team["WhichTeam"] == 1)
            {
                Instantiate(PlayerListItemPrefab, RedListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            else if ((int)team["WhichTeam"] == 0)
            {
                Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Hashtable hash = new Hashtable();
        if (PhotonNetwork.PlayerList.Count() % 2 == 0)
        {
            Instantiate(PlayerListItemPrefab, RedListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
            hash["WhichTeam"] = 1;
            newPlayer.SetCustomProperties(hash);
        }
        else
        {
            Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
            hash["WhichTeam"] = 0;
            newPlayer.SetCustomProperties(hash);

        }
    }
    public void SwitchToBlue(int team)
    {

        Player[] players = PhotonNetwork.PlayerList;
        Hashtable hash = new Hashtable();
        hash["WhichTeam"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        

    }

    public void SwitchToRed(int team)
    {
        Player[] players = PhotonNetwork.PlayerList;
        Hashtable hash = new Hashtable();
        hash["WhichTeam"] = 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

    }
}
