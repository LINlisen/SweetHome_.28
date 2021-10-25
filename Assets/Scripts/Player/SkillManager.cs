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


    public GameObject Arrow;
    public Canvas SkillRange;
    private bool _bOpen = false;
    private Vector2 dir = Vector2.zero;
    private Vector3 PlayerPos;
    private Vector3 position;

    private bool _SkillbtnDown;
    private Vector2 DragDir;
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

        _SkillbtnDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bOpen == true)
        {

            Quaternion transRot = Quaternion.LookRotation(position - PlayerPos);
            transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
            SkillRange.transform.rotation = Quaternion.Lerp(transRot, SkillRange.transform.rotation, 0f);
        }
        if (_SkillbtnDown)
        {

        }
    }
    public void UseSkill(Vector3 pos,string name,float x,float y,Vector3 playerPos)
    {
        switch (name)
        {
            case "Candy":
                Arrow.SetActive(true);
                _bOpen = true;
                dir.x = x;
                dir.y = y;
                PlayerPos = playerPos;
                position = pos;
                break;
        }
    }
    public void ShowArrow()
    {
        Arrow.SetActive(true);
        _SkillbtnDown = true;
        Debug.Log("showwArrow");
    }
    public void Drag()
    {

    }
}
