using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class TimeController : MonoBehaviour
{
    public int allSeconds;

    public int minutes = 5; //設定倒數計時的「分」

    public int seconds = 00; //設定倒數計時的「秒」

    [Space(10)]

    [Header("設定 UI 元素")]

    public Text text_Timmer; // 指定倒數計時的文字
    public Text text_Countdown;
    public GameObject text_Waiting;
    //public Text text_RT;
    //public Text text_BT;

    public GameObject gameOver; // 指定顯示 GameOver 物件
    private int RedTeam;
    private int BlueTeam;

    Hashtable time = new Hashtable();
    Hashtable roomhash = new Hashtable();
    Hashtable hash = new Hashtable();

    // Start is called before the first frame update
    bool startTimer = false;
    int countdown = 4;
    int timerIncrementValue;
    int startTime;
    int BlindStartTime;
    public GameObject BlindVision;
    Hashtable CustomeValue = new Hashtable();

    Player[] players = PhotonNetwork.PlayerList;

    int pready = 0;
    void Start()
    {
        time.Add("StartTime", (int)PhotonNetwork.Time);
        CustomeValue.Add("TimerReady", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(time);
        PhotonNetwork.LocalPlayer.SetCustomProperties(CustomeValue);
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        RedTeam = 0;
        BlueTeam = 0;
        allSeconds = (minutes * 60) + seconds;
        for (int i = 0; i < players.Count(); i++)
        {
            players[i].CustomProperties["WhichTeam"] = (string)PhotonNetwork.CurrentRoom.CustomProperties["Player" + (i+1).ToString()];
        }
    }

    void Update()
    {
        if (pready != PhotonNetwork.PlayerList.Count())
        {
            pready = 0;
            for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
            {
                if ((bool)PhotonNetwork.PlayerList[i].CustomProperties["TimerReady"] == true)
                {
                    pready++;
                }
            }
        }
        else
        {
            startTimer = true;
            text_Waiting.SetActive(false);
        }
        if (!startTimer) return;
        if (startTime == 0)
        {
            startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
            string playerName ="";
            switch (PhotonNetwork.LocalPlayer.CustomProperties["Charactor"])
            {
                case 1:
                    playerName = "CandyCharactor(Clone)";
                    break;
                case 2:
                    playerName = "ChocolateCharactor(Clone)";
                    break;
                case 3:
                    playerName = "CanCharactor(Clone)";
                    break;
                case 4:
                    playerName = "IceCharactor(Clone)";
                    break;
            }
            for (int i = 0; i < players.Count(); i++)
            {
                switch (players[i].CustomProperties["Charactor"])
                {
                    case 1:
                        GameObject.Find("CandyCharactor(Clone)").GetComponentInChildren<TextMesh>().text = players[i].NickName;
                        GameObject.Find("CandyCharactor(Clone)").GetComponentInChildren<TextMesh>().transform.LookAt(GameObject.Find(playerName).transform);
                        break;
                    case 2:
                        GameObject.Find("ChocolateCharactor(Clone)").GetComponentInChildren<TextMesh>().text = players[i].NickName;
                        GameObject.Find("ChocolateCharactor(Clone)").GetComponentInChildren<TextMesh>().transform.LookAt(GameObject.Find(playerName).transform);
                        break;
                    case 3:
                        GameObject.Find("CanCharactor(Clone)").GetComponentInChildren<TextMesh>().text = players[i].NickName;
                        GameObject.Find("CanCharactor(Clone)").GetComponentInChildren<TextMesh>().transform.LookAt(GameObject.Find(playerName).transform);
                        break;
                    case 4:
                        GameObject.Find("IceCharactor(Clone)").GetComponentInChildren<TextMesh>().text = players[i].NickName;
                        GameObject.Find("IceCharactor(Clone)").GetComponentInChildren<TextMesh>().transform.LookAt(GameObject.Find(playerName).transform);
                        break;
                }
            }
            GameObject.Find("PlayerManager(Clone)").GetComponentInChildren<TextMesh>().text = "";
        }
        else
        {
            timerIncrementValue = countdown - ((int)PhotonNetwork.Time - (int)startTime);
            text_Countdown.text = (timerIncrementValue % 60).ToString();
            if(timerIncrementValue > 0)
            {
                Debug.Log("Can't move");
            }
            else if (timerIncrementValue == 0)
            {
                text_Countdown.text = "Start !!";
                GameObject.Find("Audios/AudioSourceBGM").GetComponent<AudioSource>().Play();
            }
            else if (timerIncrementValue < 0)
            {
                text_Countdown.text = ' '.ToString();
                timerIncrementValue = allSeconds - ((int)PhotonNetwork.Time - (int)startTime - countdown);
                minutes = timerIncrementValue / 60;
                seconds = timerIncrementValue % 60;

                text_Timmer.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
                /*Can Skill*/
                if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Blind"] == true)
                {
                    BlindStartTime = (int)PhotonNetwork.Time;
                    hash = PhotonNetwork.LocalPlayer.CustomProperties;
                    hash["Blind"] = false;
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                }
                if ((int)PhotonNetwork.Time - (int)BlindStartTime == 5)
                {
                    BlindVision.SetActive(false);
                }
                if (timerIncrementValue <= 0)
                {
                    //Timer Completed
                    //Do What Ever You What to Do Here
                    text_Timmer.gameObject.SetActive(false);

                    gameOver.SetActive(true);
                    if(timerIncrementValue <= -1.6)
                    {
                        roomhash["GameOver"] = true;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
                    }
                }
            }
        }
    }
}
