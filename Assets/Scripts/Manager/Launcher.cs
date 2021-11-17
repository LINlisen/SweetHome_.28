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
    //Hashtable nickname = new Hashtable();
    Hashtable hash = new Hashtable();
    Hashtable roomhash = new Hashtable();
    public static Launcher Instance;
    [SerializeField] InputField playerNicknameInputField;
    [SerializeField] InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Text RoomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform BlueListContent;
    [SerializeField] Transform RedListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject ChooseButton;
    [SerializeField] GameObject startGameButton;
    [SerializeField] Text startGameButtonText;
    [SerializeField] Text startGameHint;
    [SerializeField] Text ClickText;
    [SerializeField] Transform LoadingSpinner;

    public GameObject CharacterModels;
    public GameObject loadingScreen;
    public GameObject RoomMenu;
    public Text ProgressText;
    int team = 0;
    int flag = 0;
    private float pr = 0f;
    GameObject playername;
    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        roomhash.Add("LoadingProgress", 0);
        roomhash.Add("Choose", 0);
        roomhash.Add("StartGame", false);
        roomhash.Add("StartTime", 0);
        roomhash.Add("Player1", 0);
        roomhash.Add("Player2", 0);
        roomhash.Add("Player3", 0);
        roomhash.Add("Player4", 0);
        roomhash.Add("BlueScore", 0);
        roomhash.Add("RedScore", 0);
        hash.Add("TimerReady", false);
        hash.Add("Nickname", null);
        hash.Add("WhichTeam", null); // 0為藍隊，1為紅隊
        hash.Add("Loading", false);
        hash.Add("Ready", false);
        hash.Add("GetOut", false);
        hash.Add("Point", 0);
        //MenuManager.Instance.OpenMenu("nickname");
        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();
        InvokeRepeating("showHide", 1, 0.5f);
        startGameHint.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    void showHide()
    {
        if (ClickText.text == "")
        {
            ClickText.text = "點  擊  開  始  ！";
        }
        else
        {
            ClickText.text = "";
        }

    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        if (hash["Nickname"] == null)
        {
            MenuManager.Instance.OpenMenu("clicktostart");
        }
        else
        {
            MenuManager.Instance.OpenMenu("title");
        }
    }
    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {

        if (hash["Nickname"] == null)
        {
            MenuManager.Instance.OpenMenu("nickname");
        }
        else
        {
            MenuManager.Instance.OpenMenu("title");
            PhotonNetwork.NickName = playerNicknameInputField.text;
        }
        Debug.Log("Joined Lobby");

    }

    public void setNickname()
    {
        hash["Nickname"] = playerNicknameInputField.text;
        //nickname["Nickname"] = playerNicknameInputField.text;
        PhotonNetwork.NickName = playerNicknameInputField.text;
        MenuManager.Instance.OpenMenu("title");
    }

    public void BackToTitle()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.NickName = playerNicknameInputField.text;
        PhotonNetwork.CreateRoom(roomNameInputField.text);


        //MenuManager.Instance.OpenMenu("Loading");
        //PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    public override void OnJoinedRoom()
    {

        MenuManager.Instance.OpenMenu("room");
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;

        ColorBlock cb = startGameButton.GetComponent<Button>().colors;
        Color color = new Color(0, 1, 0.004989f, 1);
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

            if (players[i].CustomProperties["WhichTeam"] == null)
            {
                if (i % 2 != 0)
                {
                    Instantiate(PlayerListItemPrefab, RedListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
                    hash["WhichTeam"] = 1; //紅隊為1
                    players[i].SetCustomProperties(hash);
                }
                else
                {
                    Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
                    hash["WhichTeam"] = 0; //藍隊為0
                    players[i].SetCustomProperties(hash);
                }
            }
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButtonText.text = "開始";
        }
        else
        {
            startGameButtonText.text = "準備";
            cb.selectedColor = color;
            startGameButton.GetComponent<Button>().colors = cb;
        }
        ChooseButton.SetActive(PhotonNetwork.IsMasterClient);
        startGameButton.SetActive(true);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.NickName =playerNicknameInputField.text;
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        ChooseButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        int pready = 0;
        Player[] players = PhotonNetwork.PlayerList;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameHint.CrossFadeAlpha(1.0f, 0.00f, false);
            for (int i = 0; i < players.Count(); i++)
            {
                if ((bool)players[i].CustomProperties["Ready"] == true)
                {
                    pready++;
                }
            }
            if (pready == players.Count() - 1)
            {
                hash["Loading"] = true;
                for (int i = 0; i < players.Count(); i++)
                {
                    players[i].SetCustomProperties(hash);
                }
            }
            else
            {
                startGameHint.CrossFadeAlpha(0.0f, 0.8f, false);
            }
        }
        else
        {
            hash["Ready"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }
    public void ChooseRoom()
    {
        Player[] players = PhotonNetwork.PlayerList;
        roomhash["Choose"] = 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        roomhash["Choose"] = 0;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        for (int i = 0; i < players.Count(); i++)
        {
            team = (int)players[i].CustomProperties["WhichTeam"];
            roomhash["Player" + i] = team;
            PhotonNetwork.CurrentRoom.CustomProperties["Player" + i] = team;
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //MenuManager.Instance.OpenMenu("title");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        //MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void BackToRoom()
    {
        roomhash["Choose"] = 2;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        roomhash["Choose"] = 0;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (PhotonNetwork.CurrentRoom.CustomProperties["Choose"] != null)
        {
            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Choose"] == 1)
            {
                MenuManager.Instance.OpenMenu("Choose");
            }
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Choose"] == 2)
            {
                MenuManager.Instance.OpenMenu("room");
            }
        }
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
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Loading"] == true)
        {
            flag++;
            hash["Loading"] = false;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            if (flag == 1)
            {
                LoadLevel(1);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
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
        Player[] players = PhotonNetwork.PlayerList;

        hash["WhichTeam"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);


    }

    public void SwitchToRed()
    {
        Player[] players = PhotonNetwork.PlayerList;

        hash["WhichTeam"] = 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

    }

    public void LoadLevel(int sceneIndex)
    {
        int pready = 0;
        CharacterModels.SetActive(false);
        roomhash["StartGame"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        hash["Loading"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            RoomMenu.SetActive(false);
            loadingScreen.SetActive(true);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
                ProgressText.text = progress * 100f + "%";

                yield return null;
            }
        }
        else
        {
            RoomMenu.SetActive(false);
            loadingScreen.SetActive(true);
        }
    }

    void Update()
    {
        LoadingSpinner.localEulerAngles = new Vector3(0, 0, ++pr);
    }
    public void CheckTeam()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Count(); i++)
        {
            Debug.Log(players[i].NickName + "隊伍" + (int)players[i].CustomProperties["WhichTeam"]);

        }
    }
}
