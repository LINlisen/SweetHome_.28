using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;

public class TimeController : MonoBehaviour
{
    public int allSeconds;

    public int minutes = 5; //設定倒數計時的「分」

    public int seconds = 00; //設定倒數計時的「秒」

    [Space(10)]

    [Header("設定 UI 元素")]

    public Text text_Timmer; // 指定倒數計時的文字
    //public Text text_RT;
    //public Text text_BT;

    public GameObject gameOver; // 指定顯示 GameOver 物件
    private int RedTeam;
    private int BlueTeam;

    Hashtable time = new Hashtable();
    Hashtable ctime = new Hashtable();

    // Start is called before the first frame update
    bool startTimer = false;
    int timerIncrementValue;
    int startTime;
    ExitGames.Client.Photon.Hashtable CustomeValue;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = (int)PhotonNetwork.Time;
            startTimer = true;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        else
        {
            startTime = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }
        RedTeam = 0;
        BlueTeam = 0;
        //text_RT.text = RedTeam.ToString();
        //text_BT.text = BlueTeam.ToString();
        allSeconds = (minutes * 60) + seconds;
    }

    void Update()
    {
        if (!startTimer) return;
        timerIncrementValue = allSeconds - ((int)PhotonNetwork.Time - startTime);
        Debug.Log(timerIncrementValue);
        minutes = timerIncrementValue / 60;
        seconds = timerIncrementValue % 60;
        text_Timmer.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
        if (timerIncrementValue <= 0)
        {
            //Timer Completed
            //Do What Ever You What to Do Here
            text_Timmer.gameObject.SetActive(false);

            gameOver.SetActive(true);
        }
    }
}
