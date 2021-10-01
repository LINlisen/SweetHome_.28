using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toast : MonoBehaviour
{
    public GameObject LT;
    public GameObject RT;
    public GameObject LD;
    public GameObject RD;
    private int randnum1;
    private int randnum2;
    private float countTime=0;
    private float DurationTime=5.0f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(countTime);
        if (countTime < DurationTime)
        {
            countTime += Time.deltaTime;
        }
        else
        {
            randnum1 = UnityEngine.Random.Range(1, 3);
            randnum2 = UnityEngine.Random.Range(3, 5);
            switch (randnum1)
            {
                case 1:
                    LT.SetActive(true);
                    RT.SetActive(false);
                   // Debug.Log(1);
                    break;
                case 2:
                    RT.SetActive(true);
                    LT.SetActive(false);
                   // Debug.Log(2);
                    break;
                default:
                    break;
            }
            switch (randnum2)
            {
                case 3:
                    LD.SetActive(true);
                    RD.SetActive(false);
                   // Debug.Log(3);
                    break;
                case 4:
                    RD.SetActive(true);
                    LD.SetActive(false);
                   // Debug.Log(4);
                    break;
                default:
                    break;
            }
                countTime = 0;
        }
    }
}
