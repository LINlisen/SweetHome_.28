using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class InputUIManagement : MonoBehaviour
{
    PhotonView PV;
    Hashtable hash;
    [SerializeField]
    Image skillImg;
    [SerializeField]
    Sprite CandySprite;
    [SerializeField]
    Sprite ChocolateSprite;
    [SerializeField]
    Sprite CanSprite;
    [SerializeField]
    Sprite CreamSprite;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        switch ((int)hash["Charactor"])
        {
            case 1:
                skillImg.sprite = CandySprite;
                break;
            case 2:
                skillImg.sprite = ChocolateSprite;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
