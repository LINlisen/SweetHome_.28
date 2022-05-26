using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TrigDestroyandSound : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buffsound;
    public GameObject Buff;
    public GameObject getBuffParticle;


    public bool istouch;
    [SerializeField]public int charNum;
    public int netNum;
    void Start()
    {
        istouch = false;
        netNum = (int)PhotonNetwork.LocalPlayer.CustomProperties["Charactor"];
    }

    // Update is called once per frame
    void Update()
    {
        if (istouch == true)
        {
            //switch (switch_on)
            //{
            //    case 1:
            //        Debug.Log("playerbuffed");
            //        buffsound.GetComponent<AudioSource>().Play();
            //        A.gameObject.SetActive(false);
            //        break;
            //    case 2:
            //        Debug.Log("playerbuffed");
            //        buffsound.GetComponent<AudioSource>().Play();
            //        B.gameObject.SetActive(false);
            //        break;
            //    case 3:
            //        Debug.Log("playerbuffed");
            //        buffsound.GetComponent<AudioSource>().Play();
            //        C.gameObject.SetActive(false);
            //        break;
            //    case 4:
            //        Debug.Log("playerbuffed");
            //        buffsound.GetComponent<AudioSource>().Play();
            //        D.gameObject.SetActive(false);
            //        break;

            //}
            Debug.Log("playerbuffed");
            buffsound.GetComponent<AudioSource>().Play();
            Buff.gameObject.SetActive(false);
            getBuffParticle.SetActive(true);
            istouch = false;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"&& netNum == charNum)
        {
            istouch = true;
        }
        
    }
   
}
