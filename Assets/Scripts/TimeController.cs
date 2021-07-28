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

    void Start()
    {
        RedTeam = 0;
        BlueTeam = 0;
        //text_RT.text = RedTeam.ToString();
        //text_BT.text = BlueTeam.ToString();
        time.Add("Time", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(time);
        allSeconds = (minutes * 60) + seconds;
        StartCoroutine(Timmer(allSeconds)); // 呼叫協程

    }


    //void AddPotions()
    //{
    //    RedTeam += 1;
    //    BlueTeam += 1;
    //    text_RT.text = RedTeam.ToString();
    //    text_BT.text = BlueTeam.ToString();
    //}
    IEnumerator Timmer(int currenttime)
    {

        /*allSeconds = (minutes * 60) + seconds;*/ //時間換算為秒數
        /*text_Timmer.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));*/ //顯示一次最初的時間

        //使用迴圈和 WaitForSeconds 來計秒
        while (currenttime > 0)
        {

            yield return new WaitForSeconds(1); //每經過一秒

             //更改總合與顯示的秒數
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                currenttime--;
                time["Time"] = currenttime;
                PhotonNetwork.CurrentRoom.SetCustomProperties(time);
            }
            else
            {
                ctime = PhotonNetwork.CurrentRoom.CustomProperties;
                currenttime = (int)ctime["Time"];
                Debug.Log((int)ctime["Time"]);
                minutes = currenttime / 60;
                seconds = currenttime % 60;
            }
            seconds--; //換算

            if (seconds < 0)
            {

                if (minutes > 0)
                {

                    minutes -= 1;

                    seconds = 59;

                }

                else

                {

                    seconds = 0;

                }

            }

            //更改顯示的時間

            text_Timmer.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));

        }

        yield return new WaitForSeconds(1); //為了顯示 00:00 停留一秒再顯示 GAME OVER

        text_Timmer.gameObject.SetActive(false);

        gameOver.SetActive(true);

        //Time.timeScale = 0; //控制遊戲時間暫停


    }
}
