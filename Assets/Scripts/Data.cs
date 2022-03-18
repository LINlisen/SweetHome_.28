using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Data : MonoBehaviour
{
    public bool _bSeeSaw;
    
    // Start is called before the first frame update
    void Start()
    {
        Hashtable hash = new Hashtable();
        _bSeeSaw = true;
        //hash.Add("seesawbool",_bSeeSaw);
        //PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        GameObject.Find("PropertiesManager").GetComponent<InGamePropertiesManager>().ChangeRoomProperties("seesawbool", _bSeeSaw);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
