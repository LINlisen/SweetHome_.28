using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLook : MonoBehaviour
{
    [SerializeField]

    public GameObject HighPad;

    public float maxOpen = 5f;//max door height
    public float maxClose = 0f;

    public float moveSpeedx = 0f;
    public float moveSpeedy = 0f;
    public float moveSpeedz = 0f;

    bool playerHere;

    Transform playerTransformParent;

    private void Start()
    {
        playerHere = false;


    }
    private void Update()
    {
        
        if (playerHere){
            if (moveSpeedx != 0) {
                if (HighPad.transform.position.x < maxOpen)//move LeftRight
                {
                    //Debug.Log(playerHere);
                    HighPad.transform.Translate(moveSpeedx * Time.deltaTime, moveSpeedy * Time.deltaTime, moveSpeedz * Time.deltaTime);
                }
            }
            if (moveSpeedy != 0)
            {
                if (HighPad.transform.position.y < maxOpen)//move LeftRight
                {
                    //Debug.Log(playerHere);
                    HighPad.transform.Translate(moveSpeedx * Time.deltaTime, moveSpeedy * Time.deltaTime, moveSpeedz * Time.deltaTime);
                }
            }
            if (moveSpeedz != 0)
            {
                if (HighPad.transform.position.z < maxOpen)//move LeftRight
                {
                    //Debug.Log(playerHere);
                    HighPad.transform.Translate(moveSpeedx * Time.deltaTime, moveSpeedy * Time.deltaTime, moveSpeedz * Time.deltaTime);
                }
            }
        }
        else{
            if (moveSpeedx != 0){
                if (HighPad.transform.position.x > maxClose){
                //Debug.Log(playerHere);
                HighPad.transform.Translate(-moveSpeedx * Time.deltaTime, -moveSpeedy * Time.deltaTime, -moveSpeedz * Time.deltaTime);

                }
            }
            if (moveSpeedy != 0)
            {
                if (HighPad.transform.position.y > maxClose)
                {
                    //Debug.Log(playerHere);
                    HighPad.transform.Translate(-moveSpeedx * Time.deltaTime, -moveSpeedy * Time.deltaTime, -moveSpeedz * Time.deltaTime);

                }
            }
            if (moveSpeedz != 0)
            {
                if (HighPad.transform.position.z > maxClose)
                {
                    //Debug.Log(playerHere);
                    HighPad.transform.Translate(-moveSpeedx * Time.deltaTime, -moveSpeedy * Time.deltaTime, -moveSpeedz * Time.deltaTime);

                }
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerHere = true;
            
            playerTransformParent = col.transform.parent;//set the player on the pad
            col.transform.parent = transform;
            //Debug.Log("Player's Parent: " + col.transform.parent.name);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerHere = false;
            col.transform.parent = playerTransformParent;//leave the pad
        }
    }
}
