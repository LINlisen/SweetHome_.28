using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private struct SkillInfo
    {
        public float range;
        public string way;
        public string name;
    }
    private SkillInfo Candy;
    private SkillInfo Chocolate;
    private SkillInfo Can;
    private SkillInfo Cream;

    public GameObject RangeImage;
    public GameObject Arrow;
    private bool _bOpen = false;
    private Vector2 dir = Vector2.zero;
    private Vector3 PlayerPos;
    // Start is called before the first frame update
    void Start()
    {
        Candy.name = "Candy";
        Candy.range = 10.0f;
        Candy.way = "sector";

        Chocolate.name = "Candy";
        Chocolate.range = 10.0f;
        Chocolate.way = "sector";

        Can.name = "Candy";
        Can.range = 10.0f;
        Can.way = "sector";

        Cream.name = "Candy";
        Cream.range = 10.0f;
        Cream.way = "sector";
    }

    // Update is called once per frame
    void Update()
    {
        if(_bOpen == true)
        {
            Arrow.transform.position = PlayerPos;
            RangeImage.transform.position = PlayerPos;
            Arrow.transform.Rotate(0f, 0f, dir.x);
        }
    }
    public void UseSkill(Vector3 pos,string name,float x,float y)
    {
        switch (name)
        {
            case "Candy":
                RangeImage.SetActive(true);
                Arrow.SetActive(true);
                _bOpen = true;
                dir.x = x;
                dir.y = y;
                PlayerPos= pos;
                break;
        }
    }
}
