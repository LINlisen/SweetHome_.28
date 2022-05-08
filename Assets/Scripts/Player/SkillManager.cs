using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;
public class SkillManager : MonoBehaviour
{
    private struct SkillInfo
    {
        public float range;
        public string way;
        public string name;
    }
    private SkillInfo Candy;
    private SkillInfo Chocolate;
    private SkillInfo Can;
    private SkillInfo Cream;


    public GameObject Arrow;
    public Canvas SkillRange;
    private bool _bOpen = false;
    private Vector2 dir = Vector2.zero;
    private Vector3 PlayerPos;
    private Vector3 position;

    private bool _SkillbtnDown;
    private Vector2 DragDir;

    PlayerController PlayerController;
    GameObject Player;

    //Candy's ability
    public Text _tLittleCandy;
    public int _iLittleCandyNum = 4;
    public bool _bLittleCandyOne;
    public bool _bLittleCandyTwo;
    public bool _bLittleCandyThree;
    public bool _bLittleCandyZero;
    public float _fCountOne = 0;
    public float _fCountTwo = 0;
    public float _fCountThree = 0;
    public float _fCountZero = 0;
    // Start is called before the first frame update
    void Start()
    {
        Candy.name = "Candy";
        Candy.range = 10.0f;
        Candy.way = "sector";

        Chocolate.name = "Candy";
        Chocolate.range = 10.0f;
        Chocolate.way = "sector";

        Can.name = "Candy";
        Can.range = 10.0f;
        Can.way = "sector";

        Cream.name = "Candy";
        Cream.range = 10.0f;
        Cream.way = "sector";

        _SkillbtnDown = false;

        //PlayerController = GameObject.Find("PlayerManager(Clone)").transform.GetChild(0).GetComponent<PlayerController>();
        if (GameObject.Find("PlayerManager(Clone)").GetComponentInChildren<PhotonView>().IsMine)
        {
            Player = GameObject.Find("PlayerManager(Clone)");
        }
        _tLittleCandy.text = "4";
        _tLittleCandy.gameObject.SetActive(false);
        _bLittleCandyOne = _bLittleCandyTwo = _bLittleCandyThree = _bLittleCandyZero = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bOpen == true)
        {

            Quaternion transRot = Quaternion.LookRotation(position - PlayerPos);
            transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
            SkillRange.transform.rotation = Quaternion.Lerp(transRot, SkillRange.transform.rotation, 0f);
        }
        if (_SkillbtnDown)
        {
           
            Arrow.transform.position = Player.transform.GetChild(0).transform.GetChild(0).position;
        }
        else
        {
            Arrow.SetActive(false);
        }
    }
    public void UseSkill(Vector3 pos,string name,float x,float y,Vector3 playerPos)
    {
        switch (name)
        {
            case "Candy":
                Arrow.SetActive(true);
                _bOpen = true;
                dir.x = x;
                dir.y = y;
                PlayerPos = playerPos;
                position = pos;
                break;
        }
    }
    public void ShowArrow()
    {
        //Arrow.SetActive(true);
        //_SkillbtnDown = true;
        
        
    }
    public void Drag()
    {

    }
    public void DisaArrow()
    {
        //_SkillbtnDown = false;
    }
    public void UseSkill()
    {
        //PlayerController.Skill();
        Player.GetComponentInChildren<PlayerController>().Skill();

    }
    
}
