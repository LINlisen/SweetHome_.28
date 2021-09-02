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
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    Hashtable hash = new Hashtable();
    Hashtable roomhash = new Hashtable();
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
    [SerializeField] GameObject ChooseButton;
    [SerializeField] GameObject startGameButton;

    public GameObject CharacterModels;
    public GameObject loadingScreen;
    public GameObject RoomMenu;
    public Slider slider;
    public Text ProgressText;
    Player[] players = PhotonNetwork.PlayerList;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        roomhash.Add("Choose", false);
        roomhash.Add("StartGame", false);
        roomhash.Add("StartTime", 0);
        hash.Add("Nickname", null);
        hash.Add("WhichTeam", 0); // 0為藍隊，1為紅隊
        hash.Add("Ready", false);
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
        
        if ( hash["Nickname"] == null)
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
        hash["Nickname"] = playerNicknameInputField.text;
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
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
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
        ChooseButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.NickName = "<sprite=0>" + playerNicknameInputField.text;
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        ChooseButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode,string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        roomhash["StartTime"] = (int)PhotonNetwork.Time;
        roomhash["StartGame"] = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        LoadLevel(1);
    }
    public void ChooseRoom()
    {
        roomhash["Choose"] = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
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

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if(PhotonNetwork.CurrentRoom.CustomProperties["Choose"] != null)
        {
            if((bool)PhotonNetwork.CurrentRoom.CustomProperties["Choose"] == true)
            {
                MenuManager.Instance.OpenMenu("Choose");
            }
        }
        //if (PhotonNetwork.CurrentRoom.CustomProperties["StartGame"] != null)
        //{
        //    if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["StartGame"] == true)
        //    {
        //        LoadLevel(1);
        //    }
        //}
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
    public void SwitchToBlue()
    {

        hash["WhichTeam"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        

    }

    public void SwitchToRed()
    {
        hash["WhichTeam"] = 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

    }

    public void LoadLevel(int sceneIndex)
    {
        roomhash["StartGame"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        RoomMenu.SetActive(false);
        CharacterModels.SetActive(false);
        loadingScreen.SetActive(true);

        

        int pready = 0;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            ProgressText.text = progress * 100f - 1 + "%";
            
            yield return null;
        }
        //hash["Ready"] = true;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        //while(pready != PhotonNetwork.PlayerList.Count())
        //{
        //    pready = 0;
        //    for(int i =0;i< players.Count(); i++)
        //    {
        //        if((bool)players[i].CustomProperties["Ready"] == true)
        //        {
        //            pready++;
        //        }
        //    }
        //}
        //Debug.Log(pready);

    }
}
