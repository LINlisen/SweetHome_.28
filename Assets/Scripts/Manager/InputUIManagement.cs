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
    Image coldBtn;
    [SerializeField]
    public  Sprite CandySprite;
    [SerializeField]
    public Sprite ChocolateSprite;
    [SerializeField]
    public Sprite CanSprite;
    [SerializeField]
    public Sprite CreamSprite;
    [SerializeField]
    public Sprite DashSprite;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        coldBtn = gameObject.transform.GetChild(1).GetComponent<Image>();

    }
    // Start is called before the first frame update
    void Start()
    {
        switch ((int)hash["Charactor"])
        {
            case 1:
                skillImg.sprite = CandySprite;
                coldBtn.sprite = CandySprite;
                break;
            case 2:
                skillImg.sprite = ChocolateSprite;
                coldBtn.sprite = ChocolateSprite;
                break;
            case 3:
                skillImg.sprite = CanSprite;
                coldBtn.sprite = CanSprite;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
