using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text MasterIcon;
    [SerializeField] Text text;
    Player player;

    public void SetUp(Player _player)
    {
        if (_player.IsMasterClient)
        {
            MasterIcon.text = "<sprite=0>";
        }
        player = _player;
        text.text = _player.NickName;
        text.color = Color.black;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
