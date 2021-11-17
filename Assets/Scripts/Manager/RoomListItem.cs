using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Text PlayerNumber;

    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
        PlayerNumber.text = _info.PlayerCount.ToString();
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
