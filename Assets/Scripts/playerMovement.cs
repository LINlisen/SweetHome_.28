using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 20.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private float turner;
    private float looker;
    public float sensitivity = 5;

    //Seesaw

    [SerializeField] private GameObject SeesawSet;

    //[SerializeField] private float Speed = 5;
    //boost speed var
    private float normalSpeed;
    public float boostedSpeed;
    public float speedCooldown;

    private float angle = 20.0f;

    private bool playerOnLeftSeesaw;
    private bool playerOnRightSeesaw;
    public float maxAngle;
    public float minAngle;

    private bool animated;//trueR falseL can be trigger

    //treasure
    [SerializeField] private GameObject treasure;

    private void Start()
    {
        normalSpeed = speed;

        //seesaw init
        playerOnLeftSeesaw = false;
        playerOnRightSeesaw = false;

        animated = true;
       

        

    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        turner = Input.GetAxis("Mouse X") * sensitivity;
        looker = -Input.GetAxis("Mouse Y") * sensitivity;
        if (turner != 0)
        {
            //Code for action on mouse moving right
            transform.eulerAngles += new Vector3(0, turner, 0);
        }
        if (looker != 0)
        {
            //Code for action on mouse moving right

            //transform.eulerAngles += new Vector3(looker, 0, 0);
        }
        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);

        //seesaw update
        Debug.Log(SeesawSet.transform.localRotation.eulerAngles.z);
        if (playerOnRightSeesaw==true)
        {
            if (SeesawSet.transform.localRotation.eulerAngles.z>maxAngle)
            {
                //SeesawSet.transform.RotateAround(SeesawSet.transform.position, SeesawSet.transform.parent.forward, angle*Time.deltaTime);

                //SeesawSet.transform.Rotate(Vector3.forward, angle * Time.deltaTime);
            }
        }
        if (playerOnLeftSeesaw==true)
        {
            SeesawSet.transform.Rotate(Vector3.forward, -angle * Time.deltaTime);
            if (SeesawSet.transform.localRotation.eulerAngles.z==minAngle)
            {
                playerOnLeftSeesaw = false;
                //SeesawSet.transform.RotateAround(SeesawSet.transform.position, SeesawSet.transform.parent.forward, angle*Time.deltaTime);
                

            }
        }

        




    }

    private void OnTriggerEnter(Collider other)
    {
        //booster
        if (other.CompareTag("SpeedBooster"))
        {
            speed = boostedSpeed;
            StartCoroutine("BoostDuration");
        }
        if (other.CompareTag("SlowDowner"))
        {
            speed = speed/2;
            StartCoroutine("BoostDuration");
        }

        
        //seesaw
        Debug.Log(SeesawSet.transform.localRotation.eulerAngles.z);
        if (other.CompareTag("RSeesaw") )      
        {
            Debug.Log("touched");
            playerOnLeftSeesaw = true;
        }
        if (other.CompareTag("LSeesaw")) 
        {
            Debug.Log("l");
            playerOnLeftSeesaw = true;
        }

        //animated seesaw
        if (other.tag == "AnimRSeesaw")
        {
            
            if (animated == true)
            {
                Animator anim = other.GetComponentInParent<Animator>();
                anim.SetTrigger("moveOC2");
                animated = false;
            }
        }
        if (other.tag == "AnimLSeesaw")
        {
            if (animated==false)
            {
                Animator anim = other.GetComponentInParent<Animator>();
                anim.SetTrigger("moveOC");
                animated = true;
            }
            
        }
        //treasure
        if (other.tag == "TreasureNormal")
        {
                Animator boxAnim = treasure.GetComponent<Animator>();
                boxAnim.SetBool("openbox",true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        //seesaw
        if (other.CompareTag("RSeesaw"))       
        {
            playerOnLeftSeesaw = false;
        }
        if (other.CompareTag("LSeesaw"))        
        {
            playerOnLeftSeesaw = false;            
        }
    }

    private void OnTriggerStay(Collider other)
    {
      
    }

    IEnumerator BoostDuration()
    {
        //boost cooldown
        yield return new WaitForSeconds(speedCooldown);
        speed = normalSpeed;

    }
}