using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamDoor : MonoBehaviour
{


    private bool IsRedTeam;
    public GameObject theDoor;

    void Start()
    {

        
        IsRedTeam = false;
    }

    
    void Update()
    {
        //Debug.Log(IsRedTeam);
        if (IsRedTeam==true)
        {
            
            theDoor.GetComponent<BoxCollider>().enabled = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")//the name of the player(different team)
        {
            IsRedTeam = true;
        }
    }
}
