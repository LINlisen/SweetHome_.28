using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PropertiesManager : MonoBehaviour
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
    public void PlayerInitialPropertiesSet()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void RoomInitialPropertiesSet()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
    }
    //***********對自己*************
    public void ChangeProperties( string prop, bool state)
    {
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        Debug.Log("PlayerProperties Changed");
        hash[prop] = state;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        Debug.Log(prop + " changed to " + state);
    }
    public void ChangeProperties( string prop, int value)
    {
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        Debug.Log("PlayerProperties Changed");
        hash[prop] = value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        Debug.Log(prop + " changed to " + value);
    }
    public void ChangeProperties( string prop, string value)
    {
        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        Debug.Log("PlayerProperties Changed");
        hash[prop] = value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        Debug.Log(prop + " changed to " + value);
    }
    //***********對其他*************
    public void ChangePlayerProperties( string prop, bool state, Player player)
    {
        hash = player.CustomProperties;
        Debug.Log("PlayerProperties Changed");
        hash[prop] = state;
        player.SetCustomProperties(hash);
        Debug.Log(prop + " changed to " + state);
    }
    public void ChangePlayerProperties( string prop, int value, Player player)
    {
        hash = player.CustomProperties;
        Debug.Log("PlayerProperties Changed");
        hash[prop] = value;
        player.SetCustomProperties(hash);
        Debug.Log(prop + " changed to " + value);
    }
    public void ChangePlayerProperties( string prop, string value, Player player)
    {
        hash = player.CustomProperties;
        Debug.Log("PlayerProperties Changed");
        hash[prop] = value;
        player.SetCustomProperties(hash);
        Debug.Log(prop + " changed to " + value);
    }
    //***********對房間*************
    public void ChangeRoomProperties( string prop, bool state)
    {
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log("RoomProperties Changed");
        roomhash[prop] = state;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        Debug.Log(prop + " changed to " + state);
    }
    public void ChangeRoomProperties( string prop, int value)
    {
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log("RoomProperties Changed");
        roomhash[prop] = value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        Debug.Log(prop + " changed to " + value);
    }
    public void ChangeRoomProperties( string prop, string value)
    {
        roomhash = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log("RoomProperties Changed");
        roomhash[prop] = value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomhash);
        Debug.Log(prop + " changed to " + value);
    }

}
