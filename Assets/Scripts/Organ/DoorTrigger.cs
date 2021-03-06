using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]

    public GameObject Door;
    public GameObject doorSmoke;

    public float maxOpen = 5f;//max door height
    public float maxClose = 0f;
    public float doorSpeed = 5f;
   

    bool playerHere;
    bool isOpened;
    Hashtable dooropen = new Hashtable();
    Hashtable isopen = new Hashtable();
    //float countTime=10;
    //float count=0;

    private void Start()
    {
        playerHere = false;
        isOpened = false;
        dooropen.Add("DoorState", false);
        PhotonNetwork.CurrentRoom.SetCustomProperties(dooropen);
        isopen = PhotonNetwork.CurrentRoom.CustomProperties;
    }
    private void Update()
    {
        if (isopen["DoorState"] == null)
        {
            return ;
        }
        else
        {
            //Debug.Log((bool)isopen["DoorState"]);
            if ((bool)isopen["DoorState"] && playerHere == true)
            {
                if (Door.transform.position.y < maxOpen)//move LeftRight
                {
                    //Debug.Log(playerHere);
                    Door.transform.Translate(0f, doorSpeed * Time.deltaTime, 0f);
                }
            }
            else if (!(bool)isopen["DoorState"])
            {
                if (Door.transform.position.y > maxClose)
                {
                    //Debug.Log(playerHere);
                    Door.transform.Translate(0f, -doorSpeed * Time.deltaTime, 0f);
                }
            }
        }
       
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            switch ((int)PhotonNetwork.LocalPlayer.CustomProperties["Charactor"])
            {
                case 1:
                    GameObject.Find("Audios/DoorOpen1").gameObject.transform.position = gameObject.transform.position;
                    GameObject.Find("Audios/DoorOpen1").GetComponent<AudioSource>().Play();
                    break;
                case 2:
                    GameObject.Find("Audios/DoorOpen2").gameObject.transform.position = gameObject.transform.position;
                    GameObject.Find("Audios/DoorOpen2").GetComponent<AudioSource>().Play();
                    break;
                case 3:
                    GameObject.Find("Audios/DoorOpen3").gameObject.transform.position = gameObject.transform.position;
                    GameObject.Find("Audios/DoorOpen3").GetComponent<AudioSource>().Play();
                    break;
                case 4:
                    GameObject.Find("Audios/DoorOpen4").gameObject.transform.position = gameObject.transform.position;
                    GameObject.Find("Audios/DoorOpen4").GetComponent<AudioSource>().Play();
                    break;
            }
            doorSmoke.SetActive(true);
            playerHere = true;
            dooropen["DoorState"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(dooropen);
        }
        
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerHere = false;
            dooropen["DoorState"] = false;
            doorSmoke.SetActive(false);
            PhotonNetwork.CurrentRoom.SetCustomProperties(dooropen);
        }
    }
    //private void OnTriggerStay(Collider col)
    //{
    //    if (isOpened == false)
    //    {
    //        
    //        if(count!=500)
    //        {
    //            count ++;
    //            door.transform.position += new Vector3(0, 1, 0)*Time.deltaTime;
    //            
    //            Debug.Log(door.transform.position);
    //            Debug.Log(Time.deltaTime);
    //            Debug.Log(count);
    //        }
    //        count = 0;
    //    }
    //}


}
