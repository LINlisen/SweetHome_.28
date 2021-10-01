﻿using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using System.Linq;

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
    Hashtable CustomeValue = new Hashtable();
    int pready = 0;
    void Start()
    {
        time.Add("StartTime", (int)PhotonNetwork.Time);
        CustomeValue.Add("TimerReady", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(time);
        PhotonNetwork.LocalPlayer.SetCustomProperties(CustomeValue);
        RedTeam = 0;
        BlueTeam = 0;
        allSeconds = (minutes * 60) + seconds;
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
        }
        if (!startTimer) return;
        if (startTime == 0)
        {
            startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
        }
        else
        {
            timerIncrementValue = allSeconds - ((int)PhotonNetwork.Time - (int)startTime);

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
}