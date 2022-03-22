using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtObj : MonoBehaviour
{
    public Transform target;
    public GameObject[] playerr;
    void Start()
    {
        //playerr = GameObject.FindGameObjectsWithTag("player");
    }

    void Update()
    {
        
        transform.LookAt(target);

    }
}
