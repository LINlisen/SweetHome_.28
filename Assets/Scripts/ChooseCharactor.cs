using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                //public GameObject CandyModel;
                //public GameObject CandyModel;
            }   
    private Charactor Candy;
    private Charactor Chocolate;
    public UserInput Input;
    public Charactor UserChoose;
    public CharactorModels Models;

    private bool _bCreated = false;
    // Start is called before the first frame update
    private void Start()
    {
        setCandyInfo();
        setChocolateInfo();
        Input.charactor_id = 1;
        UserChoose.Name.GetComponent<Text>();
        UserChoose.Introduction.GetComponent<Text>();
        UserChoose.CharactorModel.GetComponent<Transform>();
    }
    private void Update()
    {
        switch (Input.charactor_id)
        {
            case 1:
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
                    Vector3 newobjectPos = new Vector3(1.9f,-0.4f,11.4f);
                    Debug.Log(1);
                    Instantiate(Models.CandyModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation,UserChoose.CharactorModel);
                    UserChoose.CharactorModel.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                    UserChoose.CharactorModel.transform.position = newobjectPos;
                    _bCreated = true;
                }           
                break;
            case 2:
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
                    Vector3 newobjectPos = new Vector3(-82.2f, 2.2f, 4.4f);
                    Debug.Log(2);
                    Instantiate(Models.ChocolateModel, UserChoose.CharactorModel.position, UserChoose.CharactorModel.rotation, UserChoose.CharactorModel);
                    UserChoose.CharactorModel.transform.localScale = new Vector3(6,6,6);
                    UserChoose.CharactorModel.transform.position = new Vector3(12.2f, 2.2f, 87.5f);                   
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
    public void NextBtn()
    {
        _bCreated = false;
        if (Input.charactor_id == 2)
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
            Input.charactor_id = 2;
        }
        else
        {
            Input.charactor_id -= 1;
        }
    }
}
