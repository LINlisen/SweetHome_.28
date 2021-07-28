using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTCameraController : MonoBehaviour
{
    public Transform target;
    //相機與目標物體的距離
    public float distance;
    //橫向角度
    public float rotDegree = 0;
    public float rotSpeed = 20.0f;
    float rot; //弧度
    //縱向角度
    public float rollDegree = 30;
    public float rollSpeed = 0.2f;
    public float minRollDegree = -15;
    public float maxRollDegree = 15;
    float roll; //弧度
    //攝像機移動速度(滾輪控制)
    public float zoomSpeed = 1.5f;
    public float minDis = 8;
    public float maxDis = 22;
    //視角鎖定
    private bool isLock = true;

    // Use this for initialization
    void Start()
    {
        GetTarget(target.gameObject);
        Rotate();
        Roll();
        Zoom();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isLock == false)
        {
            Rotate();
            Roll();
            Zoom();
        }
        //角度轉換成弧度
        rot = rotDegree * Mathf.PI / 180;
        roll = rollDegree * Mathf.PI / 180;
        //計算相機的地面投影和相機高度
        float d = distance * Mathf.Cos(roll);
        float height = distance * Mathf.Sin(roll);
        Vector3 cameraPos = Vector3.zero;
        //計算相機的座標
        if (target != null)
        {
            cameraPos.x = target.transform.position.x + d * Mathf.Cos(rot);
            cameraPos.z = target.transform.position.z + d * Mathf.Sin(rot);
            cameraPos.y = target.transform.position.y + height;

            this.transform.position = cameraPos;
            this.transform.LookAt(target.transform.position);
        }
        if (Input.GetMouseButtonDown(1))
        {
            isLock = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isLock = true;
        }
    }
    //鼠標水平滑動調整視角
    void Rotate()
    {
        float value = Input.GetAxis("Mouse X");
        rotDegree -= value * rotSpeed*10;
    }
    //鼠標垂直方向調整視角
    void Roll()
    {
        float value = Input.GetAxis("Mouse Y");
        rollDegree += value * rollSpeed;
        rollDegree = Mathf.Clamp(rollDegree, minRollDegree, maxRollDegree);
    }
    //滾輪調整視角的遠近
    void Zoom()
    {
        float value = Input.GetAxis("Mouse ScrollWheel");
        distance -= value * zoomSpeed;
        distance = Mathf.Clamp(distance, minDis, maxDis);
    }

    //讓相機始終跟隨目標物體身上的某一點
    private void GetTarget(GameObject target)
    {
        if (target.transform.Find("cameraPoint") != null)
        {
            this.target = target.transform.Find("cameraPoint");
            Debug.Log("找到Target點");
        }
    }
    //恢復默認參數
    public void DefaultValue()
    {
        distance = 5;
        //橫向角度
        rotDegree = 270;
        //縱向角度
        rollDegree = -10;
    }
}
