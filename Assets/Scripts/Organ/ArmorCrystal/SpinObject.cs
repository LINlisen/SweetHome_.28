using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] public float RotatingSpeed = 40;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, RotatingSpeed * Time.deltaTime, 0); //rotates 50 degrees per second around z axis
    }
}
