﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Threading;
using UnityEngine.UI;
using System.IO;

public class RaiseEvent : MonoBehaviourPun
{
    //GetPotionAudio
    private bool _bPotionAudio = false;
    private float _fDurationTime = 0f;
    // Start is called before the first frame update
    private const byte GET_POTION_EVENT=0;
    private const byte TAKE_TOAST = 1;
    private const byte SEE_SAW_RIGHT = 2;
    private const byte SEE_SAW_LEFT = 3;
    private const byte TREASURE_NORMAL = 4;
    private const byte TREASURE_DEATH = 5;
    private const byte GET_ARMOR = 6;
    private const byte EXCAPE = 7;
    private const byte POTION_OUT = 8;

    /*共同特效部分*/
    private const byte DASH_ON = 9; //衝撞特效 index 1
    private const byte DASH_OFF = 10;
    private const byte SPEEDDOWN_GROUND_ON = 11; //緩速地板特效 index 6
    private const byte SPEEDDOWN_GROUND_OFF = 12; //緩速地板特效 index 6 
    private const byte SPEEDUP_GROUND_ON = 13; //加速地板特效 index 7
    private const byte SPEEDUP_GROUND_OFF = 14; //加速地板特效 index 7
    private const byte WOUNDED_ON = 15; //被撞倒暈眩特效 index 9
    private const byte WOUNDED_OFF = 16; //被撞倒暈眩特效 index 9 
    /*Candy Area*/
    private const byte CANDYSHOOT_PON = 17;
    private const byte CANDYSHOOT_POFF = 18;
    private const byte CANDYSHOOT_DELETE = 19;
    private const byte CANDY_SKILL_ON = 20; //Candy 施放技能特效 index 5
    private const byte CANDY_SKILL_OFF = 21; //Candy 施放技能特效 index 5

    int TeamBlueExcaper = 0;
    int TeamRedExcaper = 0;

    /* potion out bool*/
    private bool _bPotionOut = false;
    private int _PotionNum = 0;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //GetPotoinAudio
        if (_bPotionAudio == true)
        {
            
            
            _fDurationTime += Time.deltaTime;

            if (_fDurationTime > 1.0f)
            {
                _bPotionAudio = false;
                _fDurationTime = 0f;

            }
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        //Debug.Log("OnEable");
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
        //Debug.Log("OnDisEable");
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == GET_POTION_EVENT)
        {

            object[] datas = (object[])obj.CustomData;
            bool b = (bool)datas[0];
            string PotionName = (string)datas[1];
            if (GameObject.Find(PotionName) != null)
            {

                GameObject.Find(PotionName).SetActive(b);
            }


        }
        if (obj.Code == SEE_SAW_RIGHT)
        {
            object[] datas = (object[])obj.CustomData;
            string ObjTag = (string)datas[0];
            bool ObjState = (bool)datas[1];
            Animator anim = GameObject.FindWithTag(ObjTag).GetComponentInParent<Animator>();
            anim.SetTrigger("moveOC2");
        }
        if (obj.Code == SEE_SAW_LEFT)
        {
            object[] datas = (object[])obj.CustomData;
            string ObjTag = (string)datas[0];
            bool ObjState = (bool)datas[1];
            Animator anim = GameObject.FindWithTag(ObjTag).GetComponentInParent<Animator>();
            anim.SetTrigger("moveOC");
        }
        if (obj.Code == TREASURE_NORMAL)
        {
            object[] datas = (object[])obj.CustomData;
            string ObjTag = (string)datas[0];
            bool ObjState = (bool)datas[1];
            Animator anim = GameObject.Find(ObjTag).GetComponent<Animator>();
            //Debug.Log(GameObject.FindWithTag(ObjTag).name);
            anim.SetBool("openbox", true);
        }
        if (obj.Code == TREASURE_DEATH)
        {
            object[] datas = (object[])obj.CustomData;
            string ObjTag = (string)datas[0];
            bool ObjState = (bool)datas[1];
            Animator anim = GameObject.FindWithTag(ObjTag).GetComponentInParent<Animator>();
            //Debug.Log(GameObject.FindWithTag(ObjTag).name);
            anim.SetBool("openbox", true);
        }
        if (obj.Code == GET_ARMOR)
        {
            object[] datas = (object[])obj.CustomData;
            int ObjIndex = (int)datas[0];
            bool ObjState = (bool)datas[1];
            string ObjName = (string)datas[2];
            GameObject.Find(ObjName).transform.GetChild(ObjIndex).gameObject.SetActive(ObjState);
        }
        if (obj.Code == EXCAPE)
        {
            object[] datas = (object[])obj.CustomData;
            int ObjIndex = (int)datas[0];
            string ObjName = (string)datas[1];
            int TeamBlueExcaper = (int)datas[2];
            int TeamRedExcaper = (int)datas[3];
            GameObject.Find(ObjName).SetActive(false);
            GameObject.Find("TeamBlueExcape").GetComponent<Text>().text = "藍隊逃出人數: " + TeamBlueExcaper;
            GameObject.Find("TeamRedExcape").GetComponent<Text>().text = "紅隊逃出人數: " + TeamRedExcaper;
        }
        if (obj.Code == POTION_OUT)
        {
            if (!_bPotionOut)
            {
                _bPotionOut = true;
                object[] datas = (object[])obj.CustomData;
                string Playername = (string)datas[0];

                GameObject.Find(Playername).GetComponent<Animator>().SetTrigger("Wounded");
                StartCoroutine(SetWoundedFalse(3.0f, Playername));
            }
        }
        if (obj.Code == CANDYSHOOT_PON)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            int index = (int)datas[1];
            if (GameObject.Find("CandySkill(Clone)") != null)
            {
                GameObject.Find("CandySkill(Clone)").transform.GetChild(index).transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        if (obj.Code == CANDYSHOOT_POFF)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            int index = (int)datas[1];
            if (GameObject.Find("CandySkill(Clone)") != null)
            {
                GameObject.Find("CandySkill(Clone)").transform.GetChild(index).transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        if (obj.Code == CANDYSHOOT_DELETE)
        {
            Destroy(GameObject.Find("CandySkill(Clone)"));
        }
        /*共同特效部分*/
        if(obj.Code == DASH_ON)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (obj.Code == DASH_OFF)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        if(obj.Code == SPEEDDOWN_GROUND_ON)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(6).gameObject.SetActive(true);
        }
        if (obj.Code == SPEEDDOWN_GROUND_OFF)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(6).gameObject.SetActive(false);
        }
        if (obj.Code == SPEEDUP_GROUND_ON)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(7).gameObject.SetActive(true);
        }
        if (obj.Code == SPEEDUP_GROUND_OFF)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(7).gameObject.SetActive(false);
        }
        if (obj.Code == WOUNDED_ON)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(9).gameObject.SetActive(true);
        }
        if (obj.Code == WOUNDED_OFF)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(9).gameObject.SetActive(false);
        }

        /*糖果特效*/
        if(obj.Code == CANDY_SKILL_ON)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(5).gameObject.SetActive(true);
        }
        if (obj.Code == CANDY_SKILL_OFF)
        {
            object[] datas = (object[])obj.CustomData;
            string Name = (string)datas[0];
            GameObject.Find(Name).gameObject.transform.GetChild(5).gameObject.SetActive(false);
        }

    }



    public void getPotion(string name)
    {
        //Debug.Log("GetPostion");
        StartCoroutine(Coroutine(2.0f));
        
        bool b = false;
        string PotionName = name;
        object[] datas = new object[] {b, PotionName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GET_POTION_EVENT,datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void takeToast()
    {
        //Vector3 takePos =
    }
    public void SeeSawTriggerR(string tag,bool state)
    {
        string ObjTag = tag;
        bool ObjState = state;
        object[] datas = new object[] { ObjTag, ObjState };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SEE_SAW_RIGHT, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void SeeSawTriggerL(string tag, bool state)
    {
        string ObjTag = tag;
        bool ObjState = state;
        object[] datas = new object[] { ObjTag, ObjState };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SEE_SAW_LEFT, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void TreasureNormal(string tag, bool state)
    {
        string ObjTag = tag;
        bool ObjState = state;
        object[] datas = new object[] { ObjTag, ObjState };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(TREASURE_NORMAL, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void TreasureDeath(string tag, bool state)
    {
        string ObjTag = tag;
        bool ObjState = state;
        object[] datas = new object[] { ObjTag, ObjState };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(TREASURE_DEATH, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void GetArmor(int index,bool state,string name)
    {
        int ObjIndex = index;
        bool ObjState = state;
        string ObjName = name;
        object[] datas = new object[] { ObjIndex, ObjState, ObjName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GET_ARMOR, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void Excape(int charactor, string name, int team)
    {
        int PlayerCharactor = charactor;
        switch (PlayerCharactor)
        {
            case 1:
                name = "CandyCharactor(Clone)";
                break;
            case 2:
                name = "ChocolateCharactor(Clone)";
                break;
            case 3:
                name = "CanCharactor(Clone)";
                break;
            case 4:

                break;
        }
        string ObjName = name;
        if (team == 0)
        {
            TeamBlueExcaper++;
        }
        else
        {
            TeamRedExcaper++;
        }
        object[] datas = new object[] { PlayerCharactor, ObjName, TeamBlueExcaper, TeamRedExcaper };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(EXCAPE, datas, raiseEventOptions, SendOptions.SendReliable);

    }
    public void PotionOut(string PlayerName,bool havePoion)
    {
        string Name = PlayerName;
        object[] datas = new object[] { Name };
        GameObject Potion;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(POTION_OUT, datas, raiseEventOptions, SendOptions.SendReliable);
        Vector3 iniPos = GameObject.Find(Name).gameObject.transform.position + new Vector3(10, 0, 10);
        if (havePoion)
        {
            Potion = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Potion"), iniPos, GameObject.Find(Name).gameObject.transform.rotation);
            _PotionNum++;
            Potion.name = "00" + _PotionNum;
            
        }
        _bPotionOut = false;

    }
    public void CandyShootParticleOn(string Name,int index)
    {
        object[] datas = new object[] {Name , index};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(CANDYSHOOT_PON, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void CandyShootParticleOff(string Name, int index)
    {
        object[] datas = new object[] { Name, index };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(CANDYSHOOT_POFF, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void CandShootDelete()
    {
        object[] datas = new object[] {};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(CANDYSHOOT_DELETE, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    /*角色共同特效區*/
    public void DashParticleOn(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(DASH_ON, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void DashParticleOff(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(DASH_OFF, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void SpeedDwonOn(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SPEEDDOWN_GROUND_ON, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void SpeedDwonOff(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SPEEDDOWN_GROUND_OFF, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void SpeedUpOn(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SPEEDUP_GROUND_ON, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void SpeedUpOff(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SPEEDUP_GROUND_OFF, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void WoundedOn(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(WOUNDED_ON, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void WoundedOff(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(WOUNDED_OFF, datas, raiseEventOptions, SendOptions.SendReliable);
    }

    /*糖果技能施放特效*/
    public void CandySkillOn(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(CANDY_SKILL_ON, datas, raiseEventOptions, SendOptions.SendReliable);
    }
    public void CandySkillOff(string PlayerName)
    {
        object[] datas = new object[] { PlayerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(CANDY_SKILL_OFF, datas, raiseEventOptions, SendOptions.SendReliable);
    }

    IEnumerator Coroutine(float sec)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(sec);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    IEnumerator SetWoundedFalse(float sec,string Playername)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(sec);

        GameObject.Find(Playername).GetComponent<PlayerController>()._bWounded = false;
        
        Debug.Log("_bPotionOutfalse");
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
