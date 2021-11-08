using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameManagement : MonoBehaviour
{

    [SerializeField] public GameObject setPage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.AutomaticallySyncScene)
        {
            //Debug.Log("all loading ok");
        }

    }
    public void QuitGameSingle()
    {
        Application.Quit();
    }
    public void OpenSettingBox()
    {
        setPage.SetActive(true);
    }
    public void CloseSettingBox()
    {
        setPage.SetActive(false);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //MenuManager.Instance.OpenMenu("title");
    }
}
