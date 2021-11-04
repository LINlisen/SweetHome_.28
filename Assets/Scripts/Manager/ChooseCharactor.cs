using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ChooseCharactor : MonoBehaviour
{
    [System.Serializable]
        public struct Charactor
        {
           public int Id;
           public Text Name;
           public Text Introduction;
           public Transform CharactorModel;
           public GameObject ModelsParent;
           //public Button CharactorButton;
        }
        public struct UserInput
        {
            public int charactor_id;
        }
    [System.Serializable]
        public class CharactorModels
            {   
                public GameObject CandyModel;
                public GameObject ChocolateModel;
                public GameObject CanModel;
                //public GameObject CandyModel;
            }   
    private Charactor Candy;
    private Charactor Chocolate;
    private Charactor Can;
    public Button CandyButton;
    public Button ChocolateButton;
    public Button CanButton;
    public Button IceCreamButton;
    public UserInput Input;
    public Charactor UserChoose;
    public CharactorModels Models;
    public Color cb = new Color(0.72f, 0.72f, 0.72f, 1);

    private bool _bCreated = false;
    Hashtable hash = new Hashtable();
    // Start is called before the first frame update
    private void Start()
    {
        setCandyInfo();
        setChocolateInfo();
        setCanInfo();
        Input.charactor_id = 1;
        UserChoose.Name.GetComponent<Text>();
        UserChoose.Introduction.GetComponent<Text>();
        UserChoose.CharactorModel.GetComponent<Transform>();
        hash.Add("Charactor", 1);
        UserChoose.ModelsParent.SetActive(true);
    }
    private void Update()
    {
        switch (Input.charactor_id)
        {
            case 1:
                hash["Charactor"] = 1;
                CandyButton.GetComponent<Image>().color = Color.white;
                ChocolateButton.GetComponent<Image>().color = Color.gray;
                CanButton.GetComponent<Image>().color = Color.gray;
                IceCreamButton.GetComponent<Image>().color = Color.gray;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                UserChoose.Id = Candy.Id;
                UserChoose.Name.text = "史帝夫‧哈靈頓";
                UserChoose.Introduction.text = "技能效果：小糖果追蹤敵人5秒，當被追到時，小糖果會黏上去，並緩速敵人2秒。技能增強：小糖果會變快。";
                if (!_bCreated)
                {
                    if (UserChoose.CharactorModel.childCount != 0)
                    {
                        GameObject pre = UserChoose.CharactorModel.GetChild(0).gameObject;
                        Destroy(pre);
                    }
                    Vector3 newobjectPos = new Vector3(0f,0f,0f);
                    Debug.Log(1);
                    Instantiate(Models.CandyModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation,UserChoose.CharactorModel);
                    UserChoose.CharactorModel.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                    UserChoose.CharactorModel.transform.position = new Vector3(1.5f, -0.5f, 10.3f);
                    _bCreated = true;
                }           
                break;
            case 2:
                hash["Charactor"] = 2;
                CandyButton.GetComponent<Image>().color = Color.gray;
                ChocolateButton.GetComponent<Image>().color = Color.white;
                CanButton.GetComponent<Image>().color = Color.gray;
                IceCreamButton.GetComponent<Image>().color = Color.gray;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                UserChoose.Id = Chocolate.Id;
                UserChoose.Name.text = "傑克‧威爾森";
                UserChoose.Introduction.text = "技能效果：放置巧克力牆，當敵人接近時，蓋下來壓住敵人，造成敵人暈眩2秒。技能增強：巧克力牆有隱形效果。";
                if (!_bCreated)
                {
                    if (UserChoose.CharactorModel.childCount != 0)
                    {
                        GameObject pre =UserChoose.CharactorModel.GetChild(0).gameObject;
                        Destroy(pre);
                    }
                    Debug.Log(2);
                    Instantiate(Models.ChocolateModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation, UserChoose.CharactorModel);
                    UserChoose.CharactorModel.transform.localScale = new Vector3(6,6,6);
                    UserChoose.CharactorModel.transform.position = new Vector3(10.7f, 1.6f, 83.6f);                   
                    _bCreated = true;
                }
                break;
            case 3:
                hash["Charactor"] = 3;
                CandyButton.GetComponent<Image>().color = Color.gray;
                ChocolateButton.GetComponent<Image>().color = Color.gray;
                CanButton.GetComponent<Image>().color = Color.white;
                IceCreamButton.GetComponent<Image>().color = Color. gray;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                UserChoose.Id = Can.Id;
                UserChoose.Name.text = "南西‧梅森";
                UserChoose.Introduction.text = "技能效果：從玩家自身方圓5米內噴射一攤飲料，範圍內的敵人進入障目狀態。技能增強：增加緩速5秒效果。";
                if (!_bCreated)
                {
                    if (UserChoose.CharactorModel.childCount != 0)
                    {
                        GameObject pre = UserChoose.CharactorModel.GetChild(0).gameObject;
                        Destroy(pre);
                    }
                    Debug.Log(2);
                    Instantiate(Models.CanModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation, UserChoose.CharactorModel);
                    UserChoose.CharactorModel.transform.localScale = new Vector3(25, 25, 25);
                    UserChoose.CharactorModel.transform.position = new Vector3(10.7f, 1.6f, 83.6f);
                    _bCreated = true;
                }
                break;
        }
           
    }
    private void setCandyInfo()
    {
        Candy.Id = 1;
        //Candy.Name.text = "Candy";
        //Candy.Introduction.text = "一顆頑強的糖果";
    }
    private void setChocolateInfo()
    {
        Candy.Id = 2;
        //Candy.Name = "Chocolare";
        //Candy.Introduction.text = "一片高大的巧克力";
    }
    private void setCanInfo()
    {
        Can.Id = 3;
        //Candy.Name = "Chocolare";
        //Candy.Introduction.text = "一片高大的巧克力";
    }
    public void CandyChoosed()
    {
        Input.charactor_id = 1;
       //Debug.Log("Choose 1");
        _bCreated = false;
    }
    public void ChocolateChoosed()
    {
        Input.charactor_id = 2;
        //Debug.Log("Choose 2");
        _bCreated = false;
    }
    public void CanChoosed()
    {
        Input.charactor_id = 3;
        //Debug.Log("Choose 3");
        _bCreated = false;
    }
    public void NextBtn()
    {
        _bCreated = false;
        if (Input.charactor_id == 3)
        {
            Input.charactor_id = 1;
        }
        else
        {
            Input.charactor_id += 1;
        }
    }
    public void PreBtn()
    {
        _bCreated = false;
        if (Input.charactor_id == 1)
        {
            Input.charactor_id = 3;
        }
        else
        {
            Input.charactor_id -= 1;
        }
    }
}
