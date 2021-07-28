using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlayersAction : MonoBehaviour
{
    private bool _bIsDash;
    private float dashTime;
    private Vector3 directionXOZ;
    public CharacterController playerController;
    public float dashDuration;// 控制冲刺时间
    public float dashSpeed;// 冲刺速度
    // Start is called before the first frame update
    void Start()
    {
        _bIsDash = false;
     
    }

    // Update is called once per frame
    void Update()
    {
        //if (_bIsDash == true&&dashTime<=dashDuration)
        //{
        //    dashTime += Time.deltaTime;
        //    playerController.Move(directionXOZ * dashTime * dashSpeed);
        //    if (dashTime > dashDuration)
        //    {
        //        _bIsDash = false;
        //        dashTime = 0f;
        //    }
            
        //}
    }
    //public void Dash(CharacterController playerController)
    //{
    //    _bIsDash = true;
    //    //Vector3 i = new Vector3(10f, 0f, 10f);
    //    //playerController.Move(i);
    //    Debug.Log("dash");
    //    Debug.Log(playerController.enabled);




    //    directionXOZ.y = 0f;// 只做平面的上下移动和水平移动，不做高度上的上下移动
    //    directionXOZ = playerController.transform.forward;// forward 指向物体当前的前方
       
        
       


    //}
    //public void clickDash()
    //{
    //    _bIsDash = true;
    //}
}
