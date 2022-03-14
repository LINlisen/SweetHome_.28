using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class InGamePropertiesManager : MonoBehaviour
{
    Hashtable hash = new Hashtable();
    Hashtable roomhash = new Hashtable();
    private void Awake()
    {
        roomhash.Add("LoadingProgress", 0);
        roomhash.Add("Choose", 0);
        roomhash.Add("StartGame", false);
        roomhash.Add("GameOver", false);
        roomhash.Add("StartTime", 0);
        roomhash.Add("Player1", 0);
        roomhash.Add("Player2", 0);
        roomhash.Add("Player3", 0);
        roomhash.Add("Player4", 0);
        roomhash.Add("BlueScore", 0);
        roomhash.Add("RedScore", 0);
        hash.Add("TimerReady", false);
        hash.Add("Nickname", null);
        hash.Add("WhichTeam", null); // 0為藍隊，1為紅隊
        hash.Add("Loading", false);
        hash.Add("Ready", false);
        hash.Add("GetOut", false);
        hash.Add("Blind", false);
        hash.Add("Point", 0);
        hash.Add("Wounded", false);
        hash.Add("Charactor", 1);
    }
    void Start()
    {

    }
    public void ChangeProperties(string hashtype, string prop, bool state)
    {
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        switch (hashtype)
        {
            case "room":
                Debug.Log("RoomProperties Changed");
                roomhash[prop] = state;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
                Debug.Log(prop + " changed to " + state);
                break;
            case "player":
                Debug.Log("PlayerProperties Changed");
                hash[prop] = state;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                Debug.Log(prop + " changed to " + state);
                break;
        }
    }
    public void ChangeProperties(string hashtype, string prop, int value)
    {
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        switch (hashtype)
        {
            case "room":
                Debug.Log("RoomProperties Changed");
                roomhash[prop] = value;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
                Debug.Log(prop + " changed to " + value);
                break;
            case "player":
                Debug.Log("PlayerProperties Changed");
                hash[prop] = value;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                Debug.Log(prop + " changed to " + value);
                break;
        }
    }
    public void ChangeProperties(string hashtype, string prop, string value)
    {
        //roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        switch (hashtype)
        {
            case "room":
                Debug.Log("RoomProperties Changed");
                roomhash[prop] = value;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
                Debug.Log(prop + " changed to " + value);
                break;
            case "player":
                Debug.Log("PlayerProperties Changed");
                hash[prop] = value;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                Debug.Log(prop + " changed to " + value);
                break;
        }
    }
}
