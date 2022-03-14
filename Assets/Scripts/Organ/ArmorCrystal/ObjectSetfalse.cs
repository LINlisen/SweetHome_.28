using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSetfalse : MonoBehaviour
{
    private bool isTouched;
    // Start is called before the first frame update
    void Start()
    {
        isTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouched == true)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isTouched = true;
    }
}
