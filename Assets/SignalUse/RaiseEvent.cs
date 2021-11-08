using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Threading;

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
        if(obj.Code == GET_POTION_EVENT)
        {
            
            object[] datas = (object[])obj.CustomData;
            bool b = (bool)datas[0];
            string PotionName = (string)datas[1];
            if (GameObject.Find(PotionName) != null)
              {
                
                GameObject.Find(PotionName).SetActive(b);
              }
            
           
        }
        if(obj.Code==SEE_SAW_RIGHT)
        {
            object[] datas = (object[])obj.CustomData;
            string ObjTag = (string)datas[0];
            bool ObjState = (bool)datas[1];
            Animator anim =GameObject.FindWithTag(ObjTag).GetComponentInParent<Animator>();
            //Debug.Log(GameObject.FindWithTag(ObjTag).name);
            anim.SetTrigger("moveOC2");
        }
        if (obj.Code == SEE_SAW_LEFT)
        {
            object[] datas = (object[])obj.CustomData;
            string ObjTag = (string)datas[0];
            bool ObjState = (bool)datas[1];
            Animator anim = GameObject.FindWithTag(ObjTag).GetComponentInParent<Animator>();
            //Debug.Log(GameObject.FindWithTag(ObjTag).name);
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
        if(obj.Code == GET_ARMOR)
        {
            object[] datas = (object[])obj.CustomData;
            int ObjIndex = (int)datas[0];
            bool ObjState = (bool)datas[1];
            string ObjName = (string)datas[2];
            GameObject.Find(ObjName).transform.GetChild(ObjIndex).gameObject.SetActive(ObjState);
        }
    }

    public void getPotion(string name)
    {
        Debug.Log("GetPostion");
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
    IEnumerator Coroutine(float sec)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(sec);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
