﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TouchControlsKit;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject camerHolder;
    [SerializeField] float mouseSensitivity, walkSpeed, smoothTime;
    [SerializeField] Item[] items;

    Animator playerAni;
    private PlayerManager playerManager;

    int itemIndex;
    int previousItemIndex = -1;
    public GameObject rotation_Wall;
    public GameObject RedDoor;
    public GameObject BlueDoor;
    float verticalLookRotation;
    float rotation;
    
    //Dash
    bool _bIsDash = false;
    private float dashTime = 0.0f;
    private float dashCold = 0;
    private GameObject dahsColdBtn;
    public float dashDuration;// 控制冲刺时间
    //Skill
    bool _bIsSkill = false;
    private float skillTime = 0.0f;
    private float skillCold = 0;
    private GameObject skillColdBtn;
    private float skillDuration;

    private Vector3 directionXOZ;
    
    public float dashSpeed;// 冲刺速度
    // Start is called before the first frame update

    public CharacterController playerController;
    Rigidbody rb;
    PhotonView PV;
    GameObject UpInformation;
    Material playerColor;
    /*Organ*/
    //[SerializeField] private GameObject SeesawSet;

    //[SerializeField] private float Speed = 5;
    //boost speed var
    private float normalSpeed;
    public float boostedSpeed;
    public float speedCooldown;

    private float angle = 20.0f;

    private bool playerOnLeftSeesaw;
    private bool playerOnRightSeesaw;
    public float maxAngle;
    public float minAngle;
    Hashtable team;
    Hashtable hash;
    //treasure
    [SerializeField] private GameObject treasure;

    //armor
    [SerializeField] private GameObject armor;
    //[SerializeField] private GameObject armorTag;
    private bool playerHasArmor;

    //pad
    private GameObject plat;
    //rock
    private ExplosionRock explosionRock;
    public GameObject rock;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<CharacterController>();
        PV = GetComponent<PhotonView>();
        UpInformation = GameObject.Find("UpInformationCanvas");
        treasure = GameObject.Find("Wooden_Chest");
        playerAni = GetComponent<Animator>();
        
    }

    void Start()
    {
        explosionRock = rock.GetComponent<ExplosionRock>();
        team = PhotonNetwork.LocalPlayer.CustomProperties;
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        
        if (PV.IsMine)
        {
            //EquipItem(0);
            playerManager = GetComponentInParent<PlayerManager>();
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(playerController);
            Destroy(rb);
        }
        GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject.SetActive(false);
        /*Orgna*/
        normalSpeed = walkSpeed;

        //seesaw init
        playerOnLeftSeesaw = false;
        playerOnRightSeesaw = false;

        //armor
        armor.SetActive(false);
        playerHasArmor = false;


    }
    public void Dash()
    {
        string playerName = PV.gameObject.name;
        _bIsDash = true;
        playerManager.animator.SetBool("Dash", true);
        directionXOZ.y = 0f;// 只做平面的上下移动和水平移动，不做高度上的上下移动
        directionXOZ = -playerController.transform.right;// forward 指向物体当前的前方
        
        GameObject.Find(playerName).gameObject.transform.GetChild(2).gameObject.SetActive(true);
        dahsColdBtn = GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(6).gameObject;//dashColdTimeBtn_Get
        dahsColdBtn.SetActive(true);

        TCKInput.SetControllerActive("dashBtn",false);
    }
    public void Skill()
    {
        _bIsSkill = true;
        skillColdBtn = GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(7).gameObject;//dashColdTimeBtn_Get
        skillColdBtn.SetActive(true);
        playerManager.animator.SetTrigger("Skill");
        switch ((int)hash["Charactor"])
        {
            case 1:
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }

    }
    private void Update()
    {
        if (PV.IsMine)
        {


            if (TCKInput.GetAction("dashBtn", EActionEvent.Down))
            {
                Dash();
            }
            if (TCKInput.GetAction("skillBtn", EActionEvent.Down))
            {
                Skill();
            }
            if (_bIsDash == true)
            {

                if (dashTime <= dashDuration)
                {
                    dashTime += Time.deltaTime;
                    playerController.Move(-directionXOZ * dashTime * dashSpeed);
                }
                else
                {
                    dashCold += Time.deltaTime;
                    dahsColdBtn.GetComponent<Image>().fillAmount += Time.deltaTime / 5.0f;

                    //playerAni.SetBool("Dash", false);
                    playerManager.animator.SetBool("Dash", false);

                    string playerName = playerController.gameObject.name;
                    GameObject.Find(playerName).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    if (dashCold >= 5.0f)
                    {

                        dashTime = 0.0f;
                        _bIsDash = false;
                        TCKInput.SetControllerActive("dashBtn", true);
                        dahsColdBtn.GetComponent<Image>().fillAmount = 0;
                        dahsColdBtn.SetActive(false);
                        dashCold = 0.0f;
                    }

                }
            }

            if (_bIsSkill == true)
            {
              skillCold += Time.deltaTime;
              skillColdBtn.GetComponent<Image>().fillAmount += Time.deltaTime / 5.0f;
                if (skillCold >= 5.0f)
                  {

                        skillTime = 0.0f;
                        _bIsSkill = false;
                        TCKInput.SetControllerActive("skillBtn", true);
                        skillColdBtn.GetComponent<Image>().fillAmount = 0;
                        skillColdBtn.SetActive(false);
                        skillCold = 0.0f;
                    }

             }
            

            //Move();
            Vector2 look = TCKInput.GetAxis("Touchpad");
            PlayerRotation(look.x, look.y);
            //armor
            if (playerHasArmor == true)
            {
                //armor.SetActive(true);
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().GetArmor(3, true, gameObject.name);
            }
        }
        else
        {
            return;
        }
    }
    
    private void PlayerMovement(float horizontal, float vertical)
    {
        Vector3 moveDirection = -playerController.transform.forward * horizontal;
        moveDirection += playerController.transform.right * vertical ;
        moveDirection.y = -1.0f;
        playerController.Move(moveDirection * Time.fixedDeltaTime * walkSpeed);
    }

    // PlayerRotation
    public void PlayerRotation(float horizontal, float vertical)
    {
        playerController.transform.Rotate(0f, horizontal * 12f, 0f);
        //rb.transform.Rotate(0f, horizontal * 12f, 0f);
        rotation += vertical * 12f;
        rotation = Mathf.Clamp(rotation, -60f, 60f);
        //camerHolder.transform.localEulerAngles = new Vector3(-rotation, camerHolder.transform.localEulerAngles.y, 0f);
    }

    void EquipItem(int _index)
    {

        if (_index == previousItemIndex)
            return;

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;
    }

    bool grounded;
    public void SetGroundedState(bool _grounded)
    {
		grounded = _grounded;
    }

    private void FixedUpdate()
    {
		if (!PV.IsMine)
			return;
        else
        {
            Vector2 move = TCKInput.GetAxis("Joystick"); // NEW func since ver 1.5.5
            if (move.x != 0 || move.y != 0)
            {
                //playerAni.SetFloat("Speed",5);
                playerManager.animator.SetFloat("Speed", 5);
            }
            else
            {
                //playerAni.SetFloat("Speed", 0);
                playerManager.animator.SetFloat("Speed", 0);
            }

            PlayerMovement(move.x, move.y);
            /*Dash*/
        }



    }

    private void OnCollisionEnter(Collision col)
    {
       
    }


    void OnTriggerStay(Collider other)
    {
         
    }

    private void OnTriggerEnter(Collider other)
    {
        Hashtable data = PhotonNetwork.CurrentRoom.CustomProperties;
        
        //toast
        if (other.gameObject.name == "toast")
        {
            GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject.SetActive(true);
        }

        
        //Set player on Pad (code in late update)
        //unfinished
        if (other.gameObject.tag == "Pad")
        {
            Debug.Log("pad setting");
            gameObject.AddComponent<FixedJoint>();
            gameObject.GetComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
        }

        /*Organ*/
        //booster
        if (other.CompareTag("SpeedBooster"))
        {
            walkSpeed = boostedSpeed;
            StartCoroutine("BoostDuration");
        }
        if (other.CompareTag("SlowDowner"))
        {
            walkSpeed = walkSpeed / 2;
            StartCoroutine("BoostDuration");
        }


        
        //animated seesaw
        if (other.tag == "AnimRSeesaw")
        {

            if ((bool)data["seesawbool"] == true)
            {
                //Debug.Log((bool)data["seesawbool"]);
                data["seesawbool"] = false;
                PhotonNetwork.CurrentRoom.SetCustomProperties(data);
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().SeeSawTriggerR("AnimRSeesaw", false);
            }
        }
        if (other.tag == "AnimLSeesaw")
        {
            if ((bool)data["seesawbool"] == false)
            {
               // Debug.Log((bool)data["seesawbool"]);
                data["seesawbool"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(data);
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().SeeSawTriggerL("AnimLSeesaw", false);
            }
        }

        //treasure
        if (other.tag == "TreasureNormal")
        {
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().TreasureNormal("Wooden_Chest", true);
            //Animator boxAnim = treasure.GetComponent<Animator>();
            //boxAnim.SetBool("openbox", true);
        }
        //easter
        if (other.tag == "TreasureDeath")
        {
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().TreasureDeath("TreasureDeath", true);
            //Animator diebox = other.GetComponentInParent<Animator>();
            //diebox.SetBool("openbox", true);
        }
        //armor
        if (other.gameObject.tag == "armorTG")
        {
            playerHasArmor = true;
        }

        /*PotionGet*/
        if (other.gameObject.transform.parent.name == "PotionList")
        {
            //Debug.Log("take"+other.gameObject.name);
            //Debug.Log("take" + other.gameObject.transform.parent.GetSiblingIndex());
            Hashtable team = PhotonNetwork.LocalPlayer.CustomProperties;
            PhotonView photonView = PhotonView.Get(UpInformation);
            photonView.RPC("getPoint", RpcTarget.All, (int)team["WhichTeam"]);
            other.GetComponent<AudioSource>().Play();
            other.GetComponent<RaiseEvent>().getPotion(other.gameObject.name);
        }
        if(other.gameObject.transform.name == "RockExplo")
        {
            //Debug.Log("call explode func");
            explosionRock.explode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "toast")
        {
            GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject.SetActive(false);
        }

        /*Organ*/
        //seesaw set
        //if (other.CompareTag("RSeesaw"))
        //{
        //    playerOnLeftSeesaw = false;
        //}
        //if (other.CompareTag("LSeesaw"))
        //{
        //    playerOnLeftSeesaw = false;
        //}
    }

    /*Organ*/
    IEnumerator BoostDuration()
    {
        //boost cooldown
        yield return new WaitForSeconds(speedCooldown);
        walkSpeed = normalSpeed;

    }
}




