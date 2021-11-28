using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TouchControlsKit;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Linq;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float rotateVelocity;

    [SerializeField] GameObject camerHolder;
    [SerializeField] float mouseSensitivity, walkSpeed, smoothTime;

    private PlayerManager playerManager;
    float rotation;

    //Dash
    bool _bIsDash = false;
    private float dashTime = 0.0f;
    private float dashCold = 0;
    private GameObject dahsColdBtn;
    public float dashDuration;// 控制冲刺时间
    private Vector3 directionXOZ;
    public float dashSpeed;// 冲刺速度
    //Skill
    bool _bIsSkill = false;
    bool _bIntoCold = false;
    bool _bAbilityOn = false;
    private float skillTime = 0.0f;
    private float skillCold = 0;
    private GameObject skillColdBtn;
    private float skillDuration;
    private SkillManager skillManager;


    // Start is called before the first frame update

    public CharacterController playerController;
    Rigidbody rb;
    PhotonView PV;
    GameObject UpInformation;
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

    //armor
    [SerializeField] private GameObject armor;
    //[SerializeField] private GameObject armorTag;
    private bool playerHasArmor;

    //pad
    private GameObject plat;
    //rock
    private ExplosionRock explosionRock;
    public GameObject rock;

    public GameObject TeamMate;
    public GameObject Excaper;
    public GameObject ExcaperCamera;
    public GameObject ExcaperTouchPad;
    public GameObject ExcaperAbility;

    //Wounded
    public bool _bWounded = false;

    Player[] players = PhotonNetwork.PlayerList;
    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        playerController = GetComponent<CharacterController>();
        PV = GetComponent<PhotonView>();
        UpInformation = GameObject.Find("UpInformationCanvas");
    }

    void Start()
    {
        explosionRock = rock.GetComponent<ExplosionRock>();
        team = PhotonNetwork.LocalPlayer.CustomProperties;
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        skillManager = GameObject.Find("SkillManager").GetComponent<SkillManager>();

        agent = gameObject.GetComponent<NavMeshAgent>();

        if (PV.IsMine)
        {
            playerManager = GetComponentInParent<PlayerManager>();
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<PlayerController>());
            //Destroy(playerController);
            //Destroy(rb);
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

        _bIsDash = true;
        playerManager.animator.SetBool("Dash", true);
        directionXOZ.y = 0f;// 只做平面的上下移动和水平移动，不做高度上的上下移动
        directionXOZ = -playerController.transform.right;// forward 指向物体当前的前方




        TCKInput.SetControllerActive("dashBtn", false);
    }
    public void Skill()
    {
        if (!_bAbilityOn)
        {
            _bIsSkill = true;
            
            playerManager.animator.SetTrigger("Skill");
            switch ((int)hash["Charactor"])
            {
                case 1:
                    for(int i =0;i <4; i++)
                    {
                        gameObject.transform.GetChild(4).transform.GetChild(i).gameObject.SetActive(true);
                    }
                    gameObject.transform.GetChild(4).gameObject.SetActive(true);
                    skillManager._tLittleCandy.gameObject.SetActive(true);
                    _bAbilityOn = true;
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }
        else
        {
            Debug.Log("Click");
            switch ((int)hash["Charactor"])
            {
                case 1:
                    skillManager._iLittleCandyNum -= 1;
                    skillManager._tLittleCandy.text = skillManager._iLittleCandyNum.ToString();
                    CandyShoot(skillManager._iLittleCandyNum);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }

    }
    private void Update()
    {
        if (PV.IsMine)
        {
            Vector2 look = TCKInput.GetAxis("Touchpad");

            if (TCKInput.GetAction("dashBtn", EActionEvent.Down))
            {
                Dash();
            }

            if (_bIsDash == true)
            {
                string playerName = PV.gameObject.name;
                if (dashTime <= dashDuration)
                {
                    if (playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
                    {
                        GameObject.Find(playerName).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        dahsColdBtn = GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(6).gameObject;//dashColdTimeBtn_Get
                        dahsColdBtn.SetActive(true);
                        dashTime += Time.deltaTime;
                        playerController.Move(-directionXOZ * dashTime * dashSpeed);
                    }
                }
                else
                {
                    dashCold += Time.deltaTime;
                    dahsColdBtn.GetComponent<Image>().fillAmount += Time.deltaTime / 5.0f;

                    playerManager.animator.SetBool("Dash", false);


                    GameObject.Find(playerName).gameObject.transform.GetChild(1).gameObject.SetActive(false);
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
            if (skillManager._iLittleCandyNum == 0)
            {
                _bIntoCold = true;
                skillManager._iLittleCandyNum = 4;
                skillManager._tLittleCandy.text = skillManager._iLittleCandyNum.ToString();
                skillManager._tLittleCandy.gameObject.SetActive(false);//Candy's Ability num
            }
            if (_bIsSkill == true)
            {
                if (_bIntoCold) //Into cold time
                {
                    skillColdBtn = GameObject.Find("AbilityCanvas").gameObject.transform.GetChild(1).gameObject;//dashColdTimeBtn_Get
                    skillColdBtn.SetActive(true);
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
                        _bAbilityOn = false;
                        _bIntoCold = false;
                    }
                }
                else //open the ability 
                {
                    switch ((int)hash["Charactor"]) //everycharacter ability
                    {
                        case 1://Candy

                            break;
                        case 2://Chocolate
                            break;
                        case 3://Can
                            break;
                        case 4://Cream
                            break;
                    }
                }
               

            }


            //Move();

            PlayerRotation(look.x, look.y);
            //armor
            if (playerHasArmor == true)
            {
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().GetArmor(2, true, gameObject.name);
            }
            if (TeamMate)
            {
                ExcaperCamera.transform.position = TeamMate.transform.position;
                ExcaperCamera.transform.rotation = TeamMate.transform.rotation;
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
        moveDirection += playerController.transform.right * vertical;
        moveDirection.y = -1.0f;
        playerController.Move(moveDirection * Time.fixedDeltaTime * walkSpeed);
    }

    // PlayerRotation
    public void PlayerRotation(float horizontal, float vertical)
    {
        playerController.transform.Rotate(0f, horizontal * 12f, 0f);
        rotation += vertical * 12f;
        rotation = Mathf.Clamp(rotation, -60f, 60f);
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
                playerManager.animator.SetFloat("Speed", 5);
            }
            else
            {
                playerManager.animator.SetFloat("Speed", 0);
            }

            PlayerMovement(move.x, move.y);
            /*Dash*/
            
        }
        /*Candy's shoot*/
        if (skillManager._bLittleCandyZero && skillManager._fCountZero < 5)
        {
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).position += new Vector3(10, 0, 0);
            skillManager._fCountZero += Time.deltaTime;
        }
        else if(skillManager._fCountZero > 5)
        {
            skillManager._fCountZero = 0;
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            skillManager._bLittleCandyZero = false;
        }
        if (skillManager._bLittleCandyOne && skillManager._fCountOne< 5)
        {
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(1).position += new Vector3(10, 0, 0);
            skillManager._fCountOne += Time.deltaTime;
        }
        else if(skillManager._fCountOne > 5)
        {
            skillManager._fCountOne = 0;
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.SetActive(false);
            skillManager._bLittleCandyOne = false;
        }
        if (skillManager._bLittleCandyTwo && skillManager._fCountTwo < 5)
        {
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(2).position += new Vector3(10, 0, 0);
            skillManager._fCountTwo += Time.deltaTime;
        }
        else if (skillManager._fCountTwo > 5)
        {
            skillManager._fCountTwo = 0;
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.SetActive(false);
            skillManager._bLittleCandyTwo = false;
        }
        if (skillManager._bLittleCandyThree && skillManager._fCountThree < 5)
        {
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(3).position += new Vector3(10, 0, 0);
            skillManager._fCountThree += Time.deltaTime;
        }
        else if (skillManager._fCountThree > 5)
        {
            skillManager._fCountThree = 0;
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(3).gameObject.SetActive(false);
            skillManager._bLittleCandyThree = false;
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
                data["seesawbool"] = false;
                PhotonNetwork.CurrentRoom.SetCustomProperties(data);
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().SeeSawTriggerR("AnimRSeesaw", false);
            }
        }
        if (other.tag == "AnimLSeesaw")
        {
            if ((bool)data["seesawbool"] == false)
            {
                data["seesawbool"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(data);
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().SeeSawTriggerL("AnimLSeesaw", false);
            }
        }

        //treasure
        if (other.tag == "TreasureNormal")
        {
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().TreasureNormal("Wooden_Chest", true);
        }
        //easter
        if (other.tag == "TreasureDeath")
        {
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().TreasureDeath("TreasureDeath", true);
        }
        //armor
        if (other.gameObject.tag == "armorTG")
        {
            playerHasArmor = true;
        }
        //Excape
        if (other.gameObject.tag == "ExitPortal")
        {
            hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["GetOut"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            for (int i = 0; i < players.Count(); i++)
            {
                if ((int)players[i].CustomProperties["WhichTeam"] == (int)PhotonNetwork.LocalPlayer.CustomProperties["WhichTeam"] && players[i] != PhotonNetwork.LocalPlayer)
                {
                    switch ((int)players[i].CustomProperties["Charactor"])
                    {
                        case 1:
                            TeamMate = GameObject.Find("CandyCharactor(Clone)/CameraTarget");
                            break;
                        case 2:
                            TeamMate = GameObject.Find("ChocolateCharactor(Clone)/CameraTarget");
                            break;
                        case 3:
                            TeamMate = GameObject.Find("CanCharactor(Clone)/CameraTarget");
                            break;
                        case 4:
                            break;
                    }
                }
            }
            Excaper.SetActive(false);
            ExcaperTouchPad = GameObject.Find("_TCKCanvas");
            ExcaperTouchPad.SetActive(false);
            ExcaperAbility = GameObject.Find("AbilityBtn");
            ExcaperAbility.SetActive(false);
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["WhichTeam"] == 0)
            {
                GameObject.Find("TeamBlueExcape").GetComponent<Text>().text = "藍隊逃出人數: 1";
            }
            else
            {
                GameObject.Find("TeamRedExcape").GetComponent<Text>().text = "紅隊逃出人數: 1";
            }
            PhotonView photonView = PhotonView.Get(UpInformation);
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().Excape((int)PhotonNetwork.LocalPlayer.CustomProperties["Charactor"], "", (int)PhotonNetwork.LocalPlayer.CustomProperties["WhichTeam"]);
        }
        /*PotionGet*/
        if (other.gameObject.transform.parent.name == "PotionList" || other.gameObject.transform.tag == "Potion")
        {
            PhotonView photonView = PhotonView.Get(UpInformation);
            hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["Point"] = (int)hash["Point"] + 1;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            photonView.RPC("getPoint", RpcTarget.All, (int)team["WhichTeam"]);
            other.GetComponent<AudioSource>().Play();
            other.GetComponent<RaiseEvent>().getPotion(other.gameObject.name);
        }
        if (other.gameObject.transform.name == "RockExplo")
        {
            explosionRock.explode();
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "toast")
        {
            GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject.SetActive(false);
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (PV.IsMine)
        {
            if (hit.gameObject.tag == "Player" && playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") &&_bWounded == false)
            {
                _bWounded = true;
                Debug.Log("Collider other players");
                Debug.Log(hit.gameObject.name);
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().PotionOut(hit.gameObject.name);
            }
        }
    }
    public void ResetPost()
    {
        gameObject.transform.position = Vector3.zero;
        Debug.Log("resetPos");
        playerController.Move(new Vector3(0, 100, 0));
        
    }
    /*Organ*/
    IEnumerator BoostDuration()
    {
        //boost cooldown
        yield return new WaitForSeconds(speedCooldown);
        walkSpeed = normalSpeed;

    }
    /*Candy's shoot*/
    private void CandyShoot(int i)
    {
        switch (i)
        {
            case 0:
                skillManager._bLittleCandyZero = true;
                break;
            case 1:
                skillManager._bLittleCandyOne = true;
                break;
            case 2:
                skillManager._bLittleCandyTwo= true;
                break;
            case 3:
                skillManager._bLittleCandyThree = true;
                break;

        }
    }
}




