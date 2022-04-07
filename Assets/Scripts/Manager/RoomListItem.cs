using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] Text RoomName;
    [SerializeField] Text RoomMaster;
    [SerializeField] Text PlayerNumber;

    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        RoomName.text = _info.Name;
        RoomMaster.text = (string)_info.CustomProperties["RoomMaster"];
        Debug.Log(_info.CustomProperties["RoomMaster"]);
        Debug.Log((string)_info.CustomProperties["RoomMaster"]);
        PlayerNumber.text = (_info.PlayerCount.ToString() + " / 4 ");
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
