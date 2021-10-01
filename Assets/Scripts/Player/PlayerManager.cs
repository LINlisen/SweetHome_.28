﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    Hashtable hash;
    [SerializeField]

    public Animator animator;
    public GameObject character;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        if (PV.IsMine)
        {
            CreateController();
        }
        
    }

    void CreateController()
    {
        int i = UnityEngine.Random.Range(0, 50);
        //Debug.Log(hash["Charactor"]);
        switch ((int)hash["Charactor"])
        {
            case 1:
                character = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CandyCharactor"), new Vector3(i, 20, 0), Quaternion.identity);
                character.transform.parent = transform;
                animator = character.GetComponent<Animator>();
                break;
            case 2:
                character = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ChocolateCharactor"), new Vector3(i, 20, 0), Quaternion.identity);
                character.transform.parent = transform;
                animator = character.GetComponent<Animator>();
                break;
        }
    }
}