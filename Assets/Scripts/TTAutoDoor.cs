using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTAutoDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(AutoDoor());
    }
    IEnumerator AutoDoor()
    {
        for(float i = 0; i <= 5; i+=Time.deltaTime)
        {
            Debug.Log("Wait"+i);
            if (i >4.7)
            {
                Debug.Log("Open");
                if (this.transform.position.z < 14)
                {
                    this.transform.position += new Vector3(0f, 0f, 0.5f * Time.deltaTime);
                }
            }
            yield return 0;

        }
        
           
            
            
         
            
            
        
        for (float i = 0; i <= 5; i += Time.deltaTime)
        {
            Debug.Log("Keep"+i);
            if (i > 4.7)
            {
                Debug.Log("Close");
                if (this.transform.position.z > 12)
                {
                    this.transform.position -= new Vector3(0f, 0f, 0.5f * Time.deltaTime);
                }
                
            }
            yield return 0;
        }
       
       
           
            


       
    }
}
