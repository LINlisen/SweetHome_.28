using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
public class Instantiate_Obj : MonoBehaviourPunCallbacks
{
    //public GameObject[] Potions;
    private byte[] Potions;
    public Transform[] Points;
    public float Ins_Time = 1;
    GameObject PotionList;
    GameObject[]  potions;
    // Start is called before the first frame update
    GameObject Ins_objs(int size)
    {
        GameObject potionone =PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Potion"), Points[size].transform.position, Points[size].transform.rotation);
        return (potionone);
    }
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PotionList = GameObject.Find("PotionList");
            for (int i = 0; i < Points.Length; i++)
            {
                potions = new GameObject[Points.Length];
                potions[i] = Ins_objs(i);
                potions[i].name = "potion" + i.ToString();
                potions[i].transform.parent = PotionList.transform;
            }
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
