using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text MasterIcon;
    [SerializeField] Text NickName;
    [SerializeField] Text Score;
    Player player;
    public GameObject ExcapeO;
    public GameObject ExcapeX;
    public void SetUp(Player _player)
    {
        Color blue = new Color32(81, 107, 147, 255);
        Color red = new Color32(202, 88, 88, 255);
        if (_player.IsMasterClient)
        {
            MasterIcon.text = "<sprite=0>";
        }
        player = _player;
        NickName.text = _player.NickName;
        if((string)player.CustomProperties["WhichTeam"] == "紅隊")
        {
            NickName.color = red;
        }
        else
        {
            NickName.color = blue;
        }
        Score.text = _player.CustomProperties["Point"].ToString();
        if((bool)player.CustomProperties["GetOut"] == true)
        {
            ExcapeO.SetActive(true);
        }
        else
        {
            ExcapeX.SetActive(false);
        }
    }

}
