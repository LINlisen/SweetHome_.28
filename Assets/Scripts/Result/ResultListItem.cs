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

    public void SetUp(Player _player)
    {
        if (_player.IsMasterClient)
        {
            MasterIcon.text = "<sprite=0>";
        }
        player = _player;
        NickName.text = _player.NickName;
        NickName.color = Color.black;
        Score.text = _player.CustomProperties["Point"].ToString();
    }

}
