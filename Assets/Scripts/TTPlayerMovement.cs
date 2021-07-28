using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTPlayerMovement : MonoBehaviour
{
    //Animator anim;
    float speed = 7;
    public bool isCrouch = false;
    public GameObject rotation_Wall;
    public GameObject RedDoor;
    public GameObject BlueDoor;
    public bool isWalk;
    private GameObject camera;

    private float dis = 999999;
    private Transform target;
    //腳步音效組件
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator>();
        camera = GameObject.Find("Main Camera");
        audio = this.gameObject.AddComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/Foot");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded") == false) return;
        if (Input.GetKey(KeyCode.Space))
        {
            RotateCtrl();
        }

        //FootStep();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.Self);
            //transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));

            //讓角色的朝向跟相機的x-o-z面的朝向保持一致
            RotateCtrl();

            isWalk = true;
            //anim.SetBool("IsWalk", isWalk);
        }
        else
        {
            isWalk = false;
            //anim.SetBool("IsWalk", isWalk);
        }
        //下蹲
        if (Input.GetKeyDown(KeyCode.F))
        {
            isCrouch = !isCrouch;
            //anim.SetBool("IsCrouch", isCrouch);
        }
        //拾取火種的操作
        
    }
    private void RotateCtrl()
    {
        Vector3 tempRot = camera.transform.position;
        tempRot.y = transform.position.y;
        Vector3 targetRot = transform.position - tempRot;
        transform.rotation = Quaternion.LookRotation(targetRot);
    }
    //public CharacterMove GetCharacterMove()
    //{
    //    return this;
    //}
    float rotate=0;
    
    void OnTriggerStay(Collider other)
    {
        /*Rotation_Wall_Setting*/
            if(other.gameObject.name == "RotationDoorR") 
            {
                //Debug.Log(other.gameObject.name); 
                    rotate -= 2; 
            }
            if(other.gameObject.name == "RotationDoorL")
            {
                //Debug.Log(other.gameObject.name);
                    rotate += 2;   
            }
        /*Color_Door_Setting*/
            if (other.gameObject.name == "RedDoor")//only PlayerId==odd can pass
            {

                if (this.GetComponent<TTPlayerState>().PlayerId %2==0)
                    {
                        RedDoor.SetActive(true);
                    }
                else
                {
                        RedDoor.SetActive(false);
                }
            }
            if (other.gameObject.name == "BlueDoor")//only PlayerId==even can pass
             {
                
                if (this.GetComponent<TTPlayerState>().PlayerId % 2 == 0)
                    {
                        BlueDoor.SetActive(false);
                    }
                else
                    {
                        BlueDoor.SetActive(true);
                    }
            }
            if (other.gameObject.name == "Cookie3_cell" || other.gameObject.name == "Cookie3_cell_001"|| other.gameObject.name == "Cookie3_cell_002"|| other.gameObject.name == "Cookie3_cell_003"|| other.gameObject.name == "Cookie3_cell_004")
            {
            this.transform.position +=new Vector3(0, 10f*Time.deltaTime*50.0f, 0);
            }

        rotation_Wall.transform.rotation = Quaternion.Euler(0f, rotate, 0f);
        

            
    }
        
}
    //void FootStep()
    //{
    //    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.D))
    //     || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow)
    //       || Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        audio.Play();
    //    }
    //    else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))
    //     || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow)
    //       || Input.GetKey(KeyCode.RightArrow))
    //    {
    //        audio.loop = true;
    //    }
    //    else
    //    {
    //        audio.loop = false;
    //    }
    //}
//}
