using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigDestroyandSound : MonoBehaviour
{

    // Start is called before the first frame update
    public AudioSource buffsound;
    void Start()
    {
        buffsound = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("playerbuffed");
            buffsound.Play();
            Destroy(collision.gameObject);
        }
    }
}
