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
                public GameObject IceModel;
            }
    private GameObject Model;
    private Charactor Candy;
    private Charactor Chocolate;
    private Charactor Can;
    private Charactor Ice;
    public Button CandyButton;
    public Button ChocolateButton;
    public Button CanButton;
    public Button IceCreamButton;
    public UserInput Input;
    public Charactor UserChoose;
    public CharactorModels Models;
    public Color cb = new Color(0.72f, 0.72f, 0.72f, 1);

    private bool _bCreated = false;
    public GameObject PropertiesManager;
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
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Ready"] == false)
        {
            switch (Input.charactor_id)
            {
                case 1:
                    //hash["Charactor"] = 1;
                    CandyButton.GetComponent<Image>().color = Color.white;
                    ChocolateButton.GetComponent<Image>().color = Color.gray;
                    CanButton.GetComponent<Image>().color = Color.gray;
                    IceCreamButton.GetComponent<Image>().color = Color.gray;
                    //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                    if((int)PhotonNetwork.LocalPlayer.CustomProperties["Charactor"] != 1)
                    {
                        PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties("Charactor", 1);
                    }
                    UserChoose.Id = Candy.Id;
                    UserChoose.Name.text = "史帝夫‧哈靈頓";
                    UserChoose.Introduction.text = "技能效果：放置地雷，一次最多可以放4顆，當敵人踩到時會被緩速\n" +
                        "。技能增強：踩到後炸飛。";
                    if (!_bCreated)
                    {
                        if (UserChoose.CharactorModel.childCount != 0)
                        {
                            GameObject pre = UserChoose.CharactorModel.GetChild(0).gameObject;
                            Destroy(pre);
                        }
                        Vector3 newobjectPos = new Vector3(0f, 0f, 0f);
                        Debug.Log(1);
                        Model=Instantiate(Models.CandyModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation, UserChoose.CharactorModel);
                        UserChoose.CharactorModel.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        UserChoose.CharactorModel.transform.position = new Vector3(-0.2f, -1.63f, 15.91f);
                        _bCreated = true;
                    }
                    break;
                case 2:
                    //hash["Charactor"] = 2;
                    CandyButton.GetComponent<Image>().color = Color.gray;
                    ChocolateButton.GetComponent<Image>().color = Color.white;
                    CanButton.GetComponent<Image>().color = Color.gray;
                    IceCreamButton.GetComponent<Image>().color = Color.gray;
                    //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                    if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Charactor"] != 2)
                    {
                        PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties("Charactor", 2);
                    }
                    UserChoose.Id = Chocolate.Id;
                    UserChoose.Name.text = "傑克‧威爾森";
                    UserChoose.Introduction.text = "技能效果：放置巧克力牆，玩家本可以以推動巧克力牆去撞敵人，被撞倒後會緩速。\n" +
                        "技能增強：巧克力牆有隱形效果。";
                    if (!_bCreated)
                    {
                        if (UserChoose.CharactorModel.childCount != 0)
                        {
                            GameObject pre = UserChoose.CharactorModel.GetChild(0).gameObject;
                            Destroy(pre);
                        }
                        Debug.Log(2);
                        Instantiate(Models.ChocolateModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation, UserChoose.CharactorModel);
                        UserChoose.CharactorModel.transform.localScale = new Vector3(3, 3, 3);
                        UserChoose.CharactorModel.transform.position = new Vector3(0.2f, -3.4f, 55.3f);
                        _bCreated = true;
                    }
                    break;
                case 3:
                    //hash["Charactor"] = 3;
                    CandyButton.GetComponent<Image>().color = Color.gray;
                    ChocolateButton.GetComponent<Image>().color = Color.gray;
                    CanButton.GetComponent<Image>().color = Color.white;
                    IceCreamButton.GetComponent<Image>().color = Color.gray;
                    //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                    if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Charactor"] != 3)
                    {
                        PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties("Charactor", 3);
                    }
                    UserChoose.Id = Can.Id;
                    UserChoose.Name.text = "南西‧梅森";
                    UserChoose.Introduction.text = "技能效果：從玩家自身方圓5米內噴射一攤飲料，範圍內的敵人進入障目狀態。\n" +
                        "技能增強：增加緩速5秒效果。";
                    if (!_bCreated)
                    {
                        if (UserChoose.CharactorModel.childCount != 0)
                        {
                            GameObject pre = UserChoose.CharactorModel.GetChild(0).gameObject;
                            Destroy(pre);
                        }
                        Debug.Log(3);
                        Instantiate(Models.CanModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation, UserChoose.CharactorModel);
                        UserChoose.CharactorModel.transform.localScale = new Vector3(20, 20, 20);
                        UserChoose.CharactorModel.transform.position = new Vector3(0.0f, -5.4f, 83.6f);
                        _bCreated = true;
                    }
                    break;
                case 4:
                    //hash["Charactor"] = 4;
                    CandyButton.GetComponent<Image>().color = Color.gray;
                    ChocolateButton.GetComponent<Image>().color = Color.gray;
                    CanButton.GetComponent<Image>().color = Color.gray;
                    IceCreamButton.GetComponent<Image>().color = Color.white;
                    //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                    if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Charactor"] != 4)
                    {
                        PropertiesManager.GetComponent<PropertiesManager>().ChangeProperties("Charactor", 4);
                    }
                    UserChoose.Id = Ice.Id;
                    UserChoose.Name.text = "莉莉安";
                    UserChoose.Introduction.text = "技能效果：玩家擁有三發冰淇淋，當敵人被射到時會緩速。\n" +
                        "技能增強：緩速效果變成暈眩效果。";
                    if (!_bCreated)
                    {
                        if (UserChoose.CharactorModel.childCount != 0)
                        {
                            GameObject pre = UserChoose.CharactorModel.GetChild(0).gameObject;
                            Destroy(pre);
                        }
                        Debug.Log(4);
                        Instantiate(Models.IceModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation, UserChoose.CharactorModel);
                        UserChoose.CharactorModel.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                        UserChoose.CharactorModel.transform.position = new Vector3(4.4f, -4.9f, 83.6f);
                        _bCreated = true;
                    }
                    break;
            }
        }
        else
        {

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
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties);
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
    public void IceChoosed()
    {
        Input.charactor_id = 4;
        //Debug.Log("Choose 3");
        _bCreated = false;
    }
    public void NextBtn()
    {
        _bCreated = false;
        if (Input.charactor_id == 4)
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
            Input.charactor_id = 4;
        }
        else
        {
            Input.charactor_id -= 1;
        }
    }
}
