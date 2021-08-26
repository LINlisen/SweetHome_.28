using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    Hashtable team;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        team = PhotonNetwork.LocalPlayer.CustomProperties;
        if (PV.IsMine)
        {
            CreateController();
        }
        
    }

    void CreateController()
    {
        int i = UnityEngine.Random.Range(0, 50);
        if ((int)team["WhichTeam"] == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ChocolateCharactor"), new Vector3(i, 20, 0), Quaternion.identity);
        }
        else 
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CandyCharactor"), new Vector3(i, 20, 0), Quaternion.identity);
        }
    }
}
