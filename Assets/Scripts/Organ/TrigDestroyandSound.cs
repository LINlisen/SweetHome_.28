using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigDestroyandSound : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject buffsound;
    public bool istouch;
    void Start()
    {
        istouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (istouch == true)
        {
            Debug.Log("playerbuffed");
            buffsound.SetActive(true);
            this.gameObject.SetActive(false);
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            istouch = true;
        }
    }
   
}
