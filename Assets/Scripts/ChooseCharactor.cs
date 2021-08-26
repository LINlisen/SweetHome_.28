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
                UserChoose.Name.text = "Candy";
                UserChoose.Introduction.text = "一顆頑強的糖果";
                if (!_bCreated)
                {
                    if (UserChoose.CharactorModel.GetChildCount() != 0)
                    {
                        GameObject pre = UserChoose.CharactorModel.GetChild(0).gameObject;
                        Destroy(pre);
                    }
                    Instantiate(Models.CandyModel, UserChoose.CharactorModel.position,UserChoose.CharactorModel.rotation,UserChoose.CharactorModel);
                    _bCreated = true;
                }           
                break;
            case 2:
                UserChoose.Id = Chocolate.Id;
                UserChoose.Name.text = "Chocolate";
                UserChoose.Introduction.text = "一片高大的巧克力";
                if (!_bCreated)
                {
                    if (UserChoose.CharactorModel.GetChildCount() != 0)
                    {
                        GameObject pre =UserChoose.CharactorModel.GetChild(0).gameObject;
                        Destroy(pre);
                    }
                    Vector3 objectPos = UserChoose.CharactorModel.position;
                    Vector3 newobjectPos = new Vector3(objectPos.x + 0.67f, objectPos.y, objectPos.z + 13.38f);
                    Instantiate(Models.ChocolateModel, newobjectPos, UserChoose.CharactorModel.rotation, UserChoose.CharactorModel);
                    Vector3 objectscale = UserChoose.CharactorModel.GetChild(0).gameObject.transform.localScale;
                    UserChoose.CharactorModel.GetChild(1).gameObject.transform.localScale = new Vector3(3,3,3);
                    Debug.Log(UserChoose.CharactorModel.GetChild(1).gameObject.name);
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
}
