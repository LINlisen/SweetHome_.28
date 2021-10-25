using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MysticShot : MonoBehaviour
{
    //Ability range
    RaycastHit hit;
    PlayerController moveScript;
   

    //public Image mysticShotImage;
    //Ability Input Variable
    Vector3 position;
    public Canvas abilityCanvas;
    //public Image skillshot;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<PlayerController>();
        abilityCanvas = GameObject.Find("AbilityCanvas").GetComponent<Canvas>();
        player = GameObject.Find("CandyCharactor(Clone)").transform;
    }

    // Update is called once per frame
    void Update()
    {
        SkillshotAbility();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }

        //Ability 1 Canvas Input
        Quaternion transRot = Quaternion.LookRotation(position - player.transform.position);
        transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z) * 10f;

        abilityCanvas.transform.rotation = Quaternion.Lerp(transRot, abilityCanvas.transform.rotation, 0f);
        abilityCanvas.transform.position = player.transform.GetChild(0).position;
    }

    public void SkillshotAbility()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Rotate
            Quaternion rotationToLookAt = Quaternion.LookRotation(position - transform.position);
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y,ref moveScript.rotateVelocity,0);

            //transform.eulerAngles = new Vector3(0, rotationY, 0);
            //if (moveScript.agent.isOnNavMesh)
            //{
            //    moveScript.agent.SetDestination(transform.position);
            //    moveScript.agent.stoppingDistance = 0;

            //}

        }
    }
  
}
