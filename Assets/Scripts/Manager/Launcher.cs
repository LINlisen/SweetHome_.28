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
    [SerializeField] GameObject GoToChooseButton;
    [SerializeField] GameObject Ready_Btn_Click;
    [SerializeField] GameObject Ready_Btn;
    [SerializeField] GameObject Start_Btn;
    [SerializeField] Text startGameButtonText;
    [SerializeField] Text startGameHint;
    [SerializeField] Image ImgClick;
    [SerializeField] Sprite S_ImgClick;
    [SerializeField] Transform LoadingSpinner;
    Hashtable hash = new Hashtable();
    Hashtable roomhash = new Hashtable();
    public GameObject PropertiesManager;

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
        
        //MenuManager.Instance.OpenMenu("nickname");
        Debug.Log("Connecting To Master");
        if (!PhotonNetwork.IsConnected)
        {
            //roomhash.Add("LoadingProgress", 0);
            //roomhash.Add("Choose", 0);
            //roomhash.Add("StartGame", false);
            //roomhash.Add("GameOver", false);
            //roomhash.Add("StartTime", 0);
            //roomhash.Add("Player1", 0);
            //roomhash.Add("Player2", 0);
            //roomhash.Add("Player3", 0);
            //roomhash.Add("Player4", 0);
            //roomhash.Add("BlueScore", 0);
            //roomhash.Add("RedScore", 0);
            //hash.Add("TimerReady", false);
            //hash.Add("Nickname", null);
            //hash.Add("WhichTeam", null); // 0為藍隊，1為紅隊
            //hash.Add("Loading", false);
            //hash.Add("Ready", false);
            //hash.Add("GetOut", false);
            //hash.Add("Blind", false);
            //hash.Add("Point", 0);
            //hash.Add("Wounded", false);
            PhotonNetwork.ConnectUsingSettings();
            InvokeRepeating("showHide", 1, 0.5f);
            startGameHint.CrossFadeAlpha(0.0f, 0.0f, false);
        }
        else
        {
            //RoomInfoRefresh();
            
        }
        
    }

    void RoomInfoRefresh()
    {
        Debug.Log("Refresh RoomInfo");
        MenuManager.Instance.OpenMenu("room");
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
        {
            if ((int)players[i].CustomProperties["WhichTeam"] == 1)
            {
                Instantiate(PlayerListItemPrefab, RedListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            else
            {
                Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
        }
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }
    void showHide()
    {
        if (ImgClick.color.a == 0)
        {
            Color Imagecolor = new Color(1, 1, 1, 255);
            ImgClick.color = Imagecolor;
        }
        else
        {
            Color Imagecolor = new Color(1, 1, 1, 0);
            ImgClick.color = Imagecolor;
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
        PropertiesManager.GetComponent<PropertiesManager>().PlayerInitialPropertiesSet();
        PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties( "Nickname", playerNicknameInputField.text);
        //hash["Nickname"] = playerNicknameInputField.text;
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
    }

    public override void OnJoinedRoom()
    {

        MenuManager.Instance.OpenMenu("room");
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
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
                    //hash["WhichTeam"] = 1; //紅隊為1
                    //players[i].SetCustomProperties(hash);
                    PropertiesManager.GetComponent<PropertiesManager>().ChangePlayerProperties( "WhichTeam", "紅隊", players[i]);
                }
                else
                {
                    Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
                    //hash["WhichTeam"] = 0; //藍隊為0
                    //players[i].SetCustomProperties(hash);
                    PropertiesManager.GetComponent<PropertiesManager>().ChangePlayerProperties("WhichTeam", "藍隊", players[i]);
                }
            }
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Start_Btn.SetActive(true);
            PropertiesManager.GetComponent<PropertiesManager>().RoomInitialPropertiesSet();
            PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties("RoomMaster", (string)PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            Ready_Btn.SetActive(true);
            Ready_Btn_Click.SetActive(false);
        }
        GoToChooseButton.SetActive(PhotonNetwork.IsMasterClient);
        //startGameButton.SetActive(true);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.NickName =playerNicknameInputField.text;
        Start_Btn.SetActive(PhotonNetwork.IsMasterClient);
        GoToChooseButton.SetActive(PhotonNetwork.IsMasterClient);
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
        Color color = new Color(0, 1, 0.004989f, 1);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for (int i = 0; i < players.Count(); i++)
            {
                if ((bool)players[i].CustomProperties["Ready"] == true)
                {
                    pready++;
                }
            }
            if (pready == players.Count() - 1)
            {
                //hash["Loading"] = true;
                for (int i = 0; i < players.Count(); i++)
                {
                    //players[i].SetCustomProperties(hash);
                    PropertiesManager.GetComponent<PropertiesManager>().ChangePlayerProperties("Loading", true, players[i]);
                }
            }
            else
            {
                startGameHint.CrossFadeAlpha(1.0f, 0.00f, false);
                startGameHint.CrossFadeAlpha(0.0f, 0.8f, false);
            }
        }
        else
        {
            Ready_Btn.SetActive(false);
            Ready_Btn_Click.SetActive(true);
            //hash["Ready"] = true;
            //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties( "Ready", true);
        }
    }
    public void ChooseRoom()
    {
        Player[] players = PhotonNetwork.PlayerList;
        //roomhash["Choose"] = 1;
        //PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        //roomhash["Choose"] = 0;
        //PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties( "Choose", 1);
        PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties( "Choose", 0);
        for (int i = 1; i < players.Count()+1; i++)
        {
            //roomhash["Player" + i] = team;
            //PhotonNetwork.CurrentRoom.CustomProperties["Player" + i] = team;
            PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties( "Player" + i, (string)players[i - 1].CustomProperties["WhichTeam"]);
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
        //roomhash["Choose"] = 2;
        //PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        //roomhash["Choose"] = 0;
        //PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties( "Choose", 2);
        PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties( "Choose", 0);

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
            if ((string)team["WhichTeam"] == "紅隊")
            {
                Instantiate(PlayerListItemPrefab, RedListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            else if ((string)team["WhichTeam"] == "藍隊")
            {
                Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
        }
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Loading"] == true)
        {
            flag++;
            //hash["Loading"] = false;
            //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties( "Loading", false);
            if (flag == 1)
            {
                LoadLevel(1);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                foreach (Transform trans in roomListContent)
                {
                    if (trans.gameObject.GetComponentInChildren<Text>().text == roomList[i].Name)
                    {
                        Destroy(trans.gameObject);
                    }
                }
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties("PlayerNumChanged", true);
        if (PhotonNetwork.PlayerList.Count() % 2 == 0)
        {

            Instantiate(PlayerListItemPrefab, RedListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
            //hash["WhichTeam"] = 1;
            //newPlayer.SetCustomProperties(hash);
            PropertiesManager.GetComponent<PropertiesManager>().ChangePlayerProperties("WhichTeam", "紅隊", newPlayer);
        }
        else
        {
            Instantiate(PlayerListItemPrefab, BlueListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
            //hash["WhichTeam"] = 0;
            //newPlayer.SetCustomProperties(hash);
            PropertiesManager.GetComponent<PropertiesManager>().ChangePlayerProperties("WhichTeam", "藍隊", newPlayer);
        }
        PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties("PlayerNumChanged", false);
    }
    public void SwitchToBlue()
    {
        Player[] players = PhotonNetwork.PlayerList;

        //hash["WhichTeam"] = 0;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties( "WhichTeam", "藍隊");

    }

    public void SwitchToRed()
    {
        Player[] players = PhotonNetwork.PlayerList;

        //hash["WhichTeam"] = 1;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties("WhichTeam", "紅隊");

    }

    public void LoadLevel(int sceneIndex)
    {
        int pready = 0;
        CharacterModels.SetActive(false);
        //roomhash["StartGame"] = false;
        //PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        //hash["Loading"] = false;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        PropertiesManager.GetComponent<PropertiesManager>().ChangeRoomProperties( "StartGame", false);
        PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties( "Loading", false);
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
                //PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
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
            Debug.Log(players[i].NickName + "隊伍" + (string)players[i].CustomProperties["WhichTeam"]);
        }
    }
}
