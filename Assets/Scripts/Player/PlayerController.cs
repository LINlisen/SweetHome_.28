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
using System.IO;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float rotateVelocity;

    [SerializeField] GameObject camerHolder;
    [SerializeField] float mouseSensitivity, walkSpeed, smoothTime;
    [SerializeField] TextMesh NickName;

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
    public AudioSource DashSound;
    //Punched
    bool _bPunched = false;
    float _fPunchedCount = 0.0f;
    Vector3 _VecPunched;
    //Skill
    bool _bIsSkill = false;
    bool _bIntoCold = false;
    bool _bAbilityOn = false;
    private float skillTime = 0.0f;
    private float skillCold = 0;
    private GameObject skillColdBtn;
    private float skillDuration;
    private SkillManager skillManager;
    public AudioSource SkillSound;
    public AudioSource SkillGenerateSound;
    public AudioSource SkillShootSound;
    /*Candy Ability*/
    private int CandyBombNum = 0;
    /*Chocolate Ability*/
    private int WallNum = 0;

    /*Ice Ability*/
    public GameObject[] IceBall = new GameObject[3];
    private Vector3[] IceShootDir = new Vector3[3];
    public bool[] IceBallShoot = new bool[3];
    private int IceBallShootNum = 0;
    public bool[] IceDelete = new bool[3];
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
    //jump pad
    [SerializeField] public float jumpPadHeight;
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

    bool flag = false;

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
        //for show nickname on scene not done
        agent = gameObject.GetComponent<NavMeshAgent>();


        if (PV.IsMine)
        {
            playerManager = GetComponentInParent<PlayerManager>();
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //Destroy(GetComponentInChildren<PlayerController>());
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
        //jumpPad
        jumpPadHeight = 60;
    }
    public void Dash()
    {
        DashSound.Play();
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
            if ((int)hash["Charactor"] != 2  && (int)hash["Charactor"] != 4) //Chocolate技能施放動畫設定在按第二次技能鍵
            {
                playerManager.animator.SetTrigger("Skill");
            }
            switch ((int)hash["Charactor"])
            {
                case 1:
                    SkillSound.Play();
                    GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().CandySkillOn(gameObject.name);
                    _bAbilityOn = true;
                    break;
                case 2:
                    gameObject.transform.GetChild(4).gameObject.SetActive(true);
                    _bAbilityOn = true;
                    break;
                case 3:
                    /*Can Skill*/
                    //CanSkill.SetActive(true);
                    GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().CanSkillOn(gameObject.name);
                    Debug.Log("CanOn");
                    //CanSkillEffect.SetActive(true);
                    GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().CanSkillEffectOn(gameObject.name);
                    Debug.Log("CanEffectOn");
                    SkillSound.Play();
                    OnGetPlayer();
                    _bAbilityOn = true;
                    break;
                case 4:
                    SkillSound.Play();
                    GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().IceSkillOn(gameObject.name);
                    for (int i = 0; i<3; i++)
                    {
                        IceBall[i] = GameObject.Find("IceShoot").transform.GetChild(i).gameObject;
                        IceBallShoot[i] = false;
                        IceDelete[i] = false;
                    }
                    _bAbilityOn = true;
                    break;
            }
        }
        else
        {
            Debug.Log("Click");
            switch ((int)hash["Charactor"])
            {
                case 1:
                    SkillShootSound.Play();
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CandyBomb"), gameObject.transform.position,gameObject.transform.rotation);
                    CandyBombNum++;
                    if(CandyBombNum == 4)
                    {
                        _bIntoCold = true;
                        CandyBombNum = 0;
                        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().CandySkillOff(gameObject.name);
                    }
                    break;
                case 2:
                    playerManager.animator.SetTrigger("Skill");

                    //gameObject.transform.GetChild(8).gameObject.SetActive(true);
                    GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().ChocolateSkillOn(gameObject.name);
                    SkillSound.Play();
                    gameObject.transform.GetChild(4).gameObject.SetActive(false);
                    gameObject.transform.GetChild(5).gameObject.SetActive(true);
                    _bIntoCold = true;
                    break;
                case 3:
                    break;
                case 4:
                    SkillShootSound.Play();
                    directionXOZ.y = 0f;// 只做平面的上下移动和水平移动，不做高度上的上下移动
                    directionXOZ = -playerController.transform.right;// forward 指向物体当前的前方
                    IceBall[IceBallShootNum].transform.position = IceBall[IceBallShootNum].transform.position - new Vector3(0, 6.5f, 0);
                    IceBallShoot[IceBallShootNum] = true;
                    IceShootDir[IceBallShootNum] = -directionXOZ;
                    playerManager.animator.SetTrigger("Skill");
                    IceBallShootNum++;
                    break;
            }
        }

    }

    private void Update()
    {
        if (PV.IsMine)
        {
            _bWounded = (bool)PhotonNetwork.LocalPlayer.CustomProperties["Wounded"];
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
                        //GameObject.Find(playerName).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().DashParticleOn(playerName);

                        dahsColdBtn = GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject;//dashColdTimeBtn_Get
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


                    //GameObject.Find(playerName).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().DashParticleOff(playerName);

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

            /*Ice Ability Setting*/
            if ((int)hash["Charactor"] == 4)
            {
                if (IceBallShootNum == 3)
                {
                    _bIntoCold = true;
                    IceBallShootNum = 0;
                    GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().IceSkillOff(gameObject.name);
                }
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
            }
            //Move();
            
            //if (_bWounded)
            //{
            //  playerManager.animator.SetTrigger("Wounded");
            //}

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
            if (_bPunched)
            {
                _fPunchedCount += Time.deltaTime;
                if (_fPunchedCount <= 2.5f)
                {
                    playerController.Move(_VecPunched * Time.deltaTime * 35);

                }
                else
                {
                    _bPunched = false;
                    _fPunchedCount = 0;
                }
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
        {
            if (IceBallShoot[0])
            {
                IceBall[0].transform.position += IceShootDir[0];
                StartCoroutine(IceBallShootFun(0));
            }
            else
            {
                if (IceBall[0] != null)
                {
                    IceBall[0].transform.position = gameObject.transform.GetChild(4).position;
                    IceBall[0].transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 90, gameObject.transform.rotation.z));

                }
            }
            if (IceBallShoot[1])
            {
                IceBall[1].transform.position += IceShootDir[1];
                StartCoroutine(IceBallShootFun(1));
            }
            else
            {
                if (IceBall[1] != null)
                {
                    IceBall[1].transform.position = gameObject.transform.GetChild(4).position;
                    IceBall[1].transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 90, gameObject.transform.rotation.z));
                }
            }

            if (IceBallShoot[2])
            {
                IceBall[2].transform.position += IceShootDir[2];
                StartCoroutine(IceBallShootFun(2));
            }
            else
            {
                if (IceBall[2] != null)
                {
                    IceBall[2].transform.position = gameObject.transform.GetChild(4).position;
                    IceBall[2].transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 90, gameObject.transform.rotation.z));
                }
            }
        }
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
            if (playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Wounded") || playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") || playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Skill"))
            {
                if (playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Wounded"))
                {
                    //gameObject.transform.GetChild(9).gameObject.SetActive(true);
                    GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().WoundedOn(gameObject.name);
                }
                Debug.Log("Can't Move");
            }
            else
            {
                //gameObject.transform.GetChild(9).gameObject.SetActive(false);
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().WoundedOff(gameObject.name);
                PlayerMovement(move.x, move.y);
            }
            /*Dash*/
            /*Candy's shoot open?*/

            /*Candy's shoot*/

            /*Ice Ability Setting*/
            if (IceBallShoot[0])
            {
                //IceBall[0].transform.position += IceShootDir[0];
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().IceShoot(0, IceShootDir[0]);
                StartCoroutine(IceBallShootFun(0));
            }
            else
            {
                if(IceBall[0] != null)
                {
                    IceBall[0].transform.position = gameObject.transform.GetChild(4).position;
                    IceBall[0].transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.x,gameObject.transform.rotation.y+90, gameObject.transform.rotation.z));

                }
            }
            if (IceBallShoot[1])
            {
                //IceBall[1].transform.position += IceShootDir[1];
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().IceShoot(1, IceShootDir[1]);
                StartCoroutine(IceBallShootFun(1));
            }
            else
            {
                if (IceBall[1] != null)
                {
                      IceBall[1].transform.position = gameObject.transform.GetChild(4).position;
                      IceBall[1].transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y+90, gameObject.transform.rotation.z));
                }
            }

            if (IceBallShoot[2])
            {
                //IceBall[2].transform.position += IceShootDir[2];
                GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().IceShoot(2, IceShootDir[2]);
                StartCoroutine(IceBallShootFun(2));
            }
            else
            {
                if (IceBall[2] != null)
                {
                      IceBall[2].transform.position = gameObject.transform.GetChild(4).position;
                      IceBall[2].transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.x,gameObject.transform.rotation.y+90, gameObject.transform.rotation.z));
                }
            }
        }
        


    }
    //Ball hit player
    public void Punched(Vector3 dir)
    {
        _VecPunched = dir;
        Debug.Log("Punch");
        _bPunched = true;
    }
    private void OnCollisionEnter(Collision col)
    {

    }


    void OnTriggerStay(Collider other)
    {
        /*ChocolateWallTrigger*/
        //if (other.gameObject.name == "ChocolateWallIsTrigger")
        //{
        //    other.gameObject.GetComponentInParent<PlayableDirector>().Play();
        //}
        if(other.gameObject.name == "ChocolateWallIsTrigger" && flag == true)
        {
            Debug.Log("Push");
            other.gameObject.transform.parent.position += playerController.transform.right;
        }
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


            other.gameObject.GetComponentInChildren<AudioSource>().Play();

            //gameObject.transform.GetChild(7).gameObject.SetActive(true);
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().SpeedUpOn(gameObject.name);

            StartCoroutine("BoostDuration");
        }
        if (other.CompareTag("SlowDowner"))
        {
            walkSpeed = walkSpeed / 2;

            //gameObject.transform.GetChild(6).gameObject.SetActive(true);
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().SpeedDwonOn(gameObject.name);

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
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().TreasureDeath("Death_Chest", true);
        }
        //treasureDeath2
        if (other.tag == "TreasureDeathTwo")
        {
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().TreasureDeathTwo("Death_Chest1", true);
        }
        //duoTreasure
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["PlayerOnDuoTreasure"] == 2)
        {
            GameObject.Find("WoodChest1").GetComponent<Animator>().SetBool("openbox", true);

        }
        //armor
        if (other.gameObject.tag == "armorTG")
        {
            if (playerHasArmor == false)
            {
                switch ((int)PhotonNetwork.LocalPlayer.CustomProperties["Charactor"])
                {
                    case 1:
                        GameObject.Find("Audios/Shield1").gameObject.transform.position = other.gameObject.transform.position;
                        GameObject.Find("Audios/Shield1").GetComponent<AudioSource>().Play();
                        break;
                    case 2:
                        GameObject.Find("Audios/Shield2").gameObject.transform.position = other.gameObject.transform.position;
                        GameObject.Find("Audios/Shield2").GetComponent<AudioSource>().Play();
                        break;
                    case 3:
                        GameObject.Find("Audios/Shield3").gameObject.transform.position = other.gameObject.transform.position;
                        GameObject.Find("Audios/Shield3").GetComponent<AudioSource>().Play();
                        break;
                    case 4:
                        GameObject.Find("Audios/Shield4").gameObject.transform.position = other.gameObject.transform.position;
                        GameObject.Find("Audios/Shield4").GetComponent<AudioSource>().Play();
                        break;
                }
            }
            playerHasArmor = true;
        }
        //jump Pad
        if (other.gameObject.tag == "jumpPad")
        {
            gameObject.transform.position = Vector3.zero;
            Debug.Log("jumpPad");
            playerController.Move(new Vector3(0, jumpPadHeight, 0));
        }

        /*禁止icon*/
        if(other.gameObject.name == "StopIconTrigger")
        {
            GameObject.Find("UpInformationCanvas").gameObject.transform.GetChild(11).gameObject.SetActive(true);
        }
        //Excape
        if (other.gameObject.tag == "ExitPortal")
        {
            hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["GetOut"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            for (int i = 0; i < players.Count(); i++)
            {
                if ((string)players[i].CustomProperties["WhichTeam"] == (string)PhotonNetwork.LocalPlayer.CustomProperties["WhichTeam"] && players[i] != PhotonNetwork.LocalPlayer)
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
            if ((string)PhotonNetwork.LocalPlayer.CustomProperties["WhichTeam"] == "藍隊")
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
        if (other.gameObject.transform.tag == "Potion")
        {
            PhotonView photonView = PhotonView.Get(UpInformation);
            hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["Point"] = (int)hash["Point"] + 1;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            photonView.RPC("getPoint", RpcTarget.All, (string)team["WhichTeam"]);
            other.GetComponent<AudioSource>().Play();
            other.GetComponent<RaiseEvent>().getPotion(other.gameObject.name);
        }
        if (other.gameObject.transform.name == "RockExplo")
        {
            explosionRock.explode();
        }
        /*Candy Shoot*/
        if(other.gameObject.transform.tag == "CandyBomb" && gameObject.name!= "CandyCharactor(Clone)")
        {
            walkSpeed -= 10;
            StartCoroutine("WalkSpeedReset");
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "ChocolateWall")
        {
            GameObject.Find("Audios/Dizzy").GetComponent<AudioSource>().Play();
            PhotonView photonView = PhotonView.Get(UpInformation);
            if(gameObject.name == "ChocolateCharactor(Clone)")
            {
                flag = true;
            }
            else
            {
                walkSpeed -= 10;
                StartCoroutine("WalkSpeedReset");
                Destroy(other.gameObject.transform.parent.gameObject);
            }
            
        }
        if (other.gameObject.tag == "IceBall" && gameObject.name != "IceCharactor(Clone)")
        {
            walkSpeed -= 10;
            StartCoroutine("WalkSpeedReset");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "toast")
        {
            GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject.SetActive(false);
        }
        /*Stop Icon set false*/
        if (other.gameObject.name == "StopIconTrigger")
        {
            GameObject.Find("UpInformationCanvas").gameObject.transform.GetChild(11).gameObject.SetActive(false); 
        }
        if (other.gameObject.tag == "ChocolateWall")
        {
            flag = false;

        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
            PhotonView photonView = PhotonView.Get(UpInformation);
            if (hit.gameObject.tag == "Player" /*&& playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") &&_bWounded == false*/)
            {
                if (playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
                {
                    if (GameObject.Find(hit.gameObject.name).GetComponent<PlayerController>()._bWounded == false)
                    {
                        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
                        Debug.Log("_bWounded是false");
                        GameObject.Find("Audios/Dizzy").GetComponent<AudioSource>().Play();
                        if ((int)hash["Point"] != 0)
                        {
                            Hashtable Wounded = new Hashtable();
                            Player[] players = PhotonNetwork.PlayerList;
                            hash["Point"] = (int)hash["Point"] - 1;
                            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                            photonView.RPC("losePoint", RpcTarget.All, (string)team["WhichTeam"]);
                            for (int j = 0; j < players.Count(); j++)
                            {
                                Wounded = players[j].CustomProperties;
                                if (player[j].name == hit.gameObject.name)
                                {
                                    Wounded["Wounded"] = true;
                                    players[j].SetCustomProperties(Wounded);
                                }
                            }
                            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().PotionOut(hit.gameObject.name, true);
                        }
                        else
                        {
                            Hashtable Wounded = new Hashtable();
                            Player[] players = PhotonNetwork.PlayerList;
                            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                            
                        for (int j = 0; j < players.Count(); j++)
                            {
                                Wounded = players[j].CustomProperties;
                                if (player[j].name == hit.gameObject.name)
                                {
                                    Wounded["Wounded"] = true;
                                    players[j].SetCustomProperties(Wounded);
                                }
                            }
                            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().PotionOut(hit.gameObject.name, false);
                        }
                    }
                } 
            }
        //hit by the dangerStick or object, add [dangerStick] tag to access
        //if (hit.gameObject.tag == "dangerStick" && _bWounded == false)
        //{
        //    GameObject.Find("Audios/Dizzy").GetComponent<AudioSource>().Play();
        //    if ((int)hash["Point"] != 0)
        //    {
        //        hash["Point"] = (int)hash["Point"] - 1;
        //        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        //        photonView.RPC("losePoint", RpcTarget.All, (int)team["WhichTeam"]);
        //        _bWounded = true;
        //        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().PotionOut(gameObject.name,true);
        //    }
        //    else
        //    {
        //        _bWounded = true;
        //        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().PotionOut(gameObject.name, false);
        //    }
        //}
        //if (hit.gameObject.tag == "ChocolateWall" && _bWounded == false)
        //{
        //    GameObject.Find("Audios/Dizzy").GetComponent<AudioSource>().Play();
        //    if ((int)hash["Point"] != 0)
        //    {
        //        hash["Point"] = (int)hash["Point"] - 1;
        //        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        //        photonView.RPC("losePoint", RpcTarget.All, (int)team["WhichTeam"]);
        //        _bWounded = true;
        //        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().PotionOut(gameObject.name, true);
        //    }
        //    else
        //    {
        //        _bWounded = true;
        //        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().PotionOut(gameObject.name, false);
        //    }
        //    Destroy(hit.gameObject);
        //}


    }
    public void ResetPost()
    {
        gameObject.transform.position = Vector3.zero;
        Debug.Log("resetPos");
        playerController.Move(new Vector3(0, 100, 0));
        
    }
    /*Chocolate's Put   Ability animation events*/
    public void ChocolateWall_Put()
    {
        
    }
    public void Close_wall()
    {
        GameObject Wall;
        Wall = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "chocolatewall"), gameObject.transform.GetChild(4).position, gameObject.transform.GetChild(4).rotation);
        Wall.name = "wall" + WallNum.ToString();
        WallNum++;
        //gameObject.transform.GetChild(8).gameObject.SetActive(false);
        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().ChocolateSkillOff(gameObject.name);
        gameObject.transform.GetChild(5).gameObject.SetActive(false);
    }

    public void CA_WoundedSetFalse()
    {
        Debug.Log("callreset");
        StartCoroutine(WoundedSetFalseCount("CandyCharactor(Clone)"));
    }
    public void CH_WoundedSetFalse()
    {
        Debug.Log("callreset");
        StartCoroutine(WoundedSetFalseCount("ChocolateCharactor(Clone)"));
    }
    public void CAN_WoundedSetFalse()
    {
        Debug.Log("callreset");
        StartCoroutine(WoundedSetFalseCount("CanCharactor(Clone)"));
    }
    public void ICE_WoundedSetFalse()
    {
        Debug.Log("callreset");
        StartCoroutine(WoundedSetFalseCount("IceCharactor(Clone)"));
    }
    /*Can Skill*/
    public void OnGetPlayer()
    {
        _bIntoCold = true;
        //獲取所有敵人
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        float distance_range = 200;    //主角與怪物的最近距離
        float distance = 0;            //當前怪物與主角的距離
        Hashtable blind = new Hashtable();
        Player[] players = PhotonNetwork.PlayerList;
        //遍歷所有敵人,計算距離並比較
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].activeSelf == true)
            {
                distance = Vector3.Distance(transform.position, player[i].transform.position);
                if (distance < distance_range)
                {
                    for(int j = 0;j < players.Count(); j++)
                    {
                        blind = players[j].CustomProperties;
                        if (player[i].name == "CandyCharactor(Clone)"&&(int)players[j].CustomProperties["Charactor"] == 1)
                        {
                            blind["Blind"] = true;
                            players[j].SetCustomProperties(blind);
                        }
                        else if (player[i].name == "ChocolateCharactor(Clone)" && (int)players[j].CustomProperties["Charactor"] == 2)
                        {
                            blind["Blind"] = true;
                            players[j].SetCustomProperties(blind);
                        }
                        else if (player[i].name == "IceCharactor(Clone)" && (int)players[j].CustomProperties["Charactor"] == 4)
                        {
                            blind["Blind"] = true;
                            players[j].SetCustomProperties(blind);
                        }
                    }
                }
            }
        }
    }
    public void CanEffectClose()
    {
        //CanSkill.SetActive(false);
        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().CanSkillOff(gameObject.name);

        //CanSkillEffect.SetActive(false);
        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().CanSkillEffectOff(gameObject.name);
    }
    


    IEnumerator BoostDuration()
    {
        //boost cooldown
        yield return new WaitForSeconds(speedCooldown);
        walkSpeed = normalSpeed;

        //gameObject.transform.GetChild(6).gameObject.SetActive(false);//slowPad
        //gameObject.transform.GetChild(7).gameObject.SetActive(false);//boostPad
        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().SpeedDwonOff(gameObject.name);
        GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().SpeedUpOff(gameObject.name);

    }
    
    IEnumerator IceBallShootFun(int index)
    {
        //boost cooldown
        yield return new WaitForSeconds(2);
        if (IceDelete[index] ==false)
        {
            print(index);
            GameObject.Find("RaiseEvent").GetComponent<RaiseEvent>().IceShootDelete(IceBall[index].gameObject.name,index);
        }
    }
    IEnumerator WalkSpeedReset()
    {
        //boost cooldown
        yield return new WaitForSeconds(3);
        walkSpeed = normalSpeed;
    }
    IEnumerator WoundedSetFalseCount(string name)
    {
        yield return new WaitForSeconds(3);
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        Hashtable Wounded = new Hashtable();
        Player[] players = PhotonNetwork.PlayerList;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        for (int j = 0; j < players.Count(); j++)
        {
            Wounded = players[j].CustomProperties;
            if (player[j].name == name)
            {
                Wounded["Wounded"] = false;
                players[j].SetCustomProperties(Wounded);
            }
        }

    }
}




