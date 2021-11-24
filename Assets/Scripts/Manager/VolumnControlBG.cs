using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class VolumnControlBG : MonoBehaviour
{
    public AudioMixer mixer;
    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) *20 );

    }
    public void SetPos()
    {
        if (GameObject.Find("PlayerManager(Clone)").transform.GetChild(0) != null)
        {
            GameObject.Find("PlayerManager(Clone)").transform.GetChild(0).gameObject.GetComponent<PlayerController>().ResetPost();
            
        }
       
    }
    
}
