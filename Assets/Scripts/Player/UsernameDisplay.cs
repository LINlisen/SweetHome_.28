using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] Text text;

    void Start()
    {
        Color blue = new Color(0, 0, 1, 1);
        Color red = new Color(1, 0, 0, 1);
        text.text = playerPV.Owner.NickName;
        if(playerPV.IsMine)
        {
            gameObject.SetActive(false);
        }
        if ((string)playerPV.Owner.CustomProperties["WhichTeam"] == "藍隊")
        {
            text.color = blue;
        }
        else
        {
            text.color = red;
        }
    }
}
