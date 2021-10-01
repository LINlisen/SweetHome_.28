using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFormController : MonoBehaviour
{
    [SerializeField] Transform waypointsParent;

    [SerializeField] float travelTime = 5f;
    [SerializeField] float endPointStayTime = 3f;
    [SerializeField] float midPointStayTime = 1f;

    float dwellTimer = 0f;

    bool movingForward = true;

    int currentPointIndex = 0;

    [SerializeField] bool loopTrack = false;

    [SerializeField] bool stopAtEndingPointOnLoopTrack = false;

    float stoppingDistanceThreshold = 0.05f;//fix some bugs

    float t = 0f;

    Vector3 startingPoint;
    Vector3 targetPoint;

    List<Vector3> waypoints;

    // Update is called once per frame
    private void Start()
    {
        waypoints = new List<Vector3>();

        for(int i = 0; i < waypointsParent.childCount; i++)
        {
            waypoints.Add(waypointsParent.GetChild(i).position);
        }
        if (waypoints.Count < 1)
        {
            Debug.LogError("you need more points");
            enabled = false;
        }
        currentPointIndex = 0;
        startingPoint = GetWaypoint();
        GetNextWaypointIndex();
        targetPoint = GetWaypoint();

        transform.position = startingPoint;

    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPoint) <= stoppingDistanceThreshold)
        {
            dwellTimer += Time.deltaTime;

            bool continueTrack = false;

            if (IsAtEndpoint())
            {

                if (loopTrack && !stopAtEndingPointOnLoopTrack && (currentPointIndex == (waypoints.Count - 1)))
                {
                    continueTrack = true;
                }


                if (dwellTimer >= endPointStayTime)
                {
                    continueTrack = true;
                }
            }
            else
            {
                if (dwellTimer >= midPointStayTime)
                {
                    continueTrack = true;

                }
            }

            if (continueTrack)
            {
                startingPoint = targetPoint;
                GetNextWaypointIndex();
                targetPoint = GetWaypoint();

                t = 0f;
                dwellTimer = 0f;//reset value
            }
            return;
        }

        t += (Time.deltaTime / travelTime);

        transform.position = Vector3.Lerp(startingPoint, targetPoint, t);//important launch place
    }


    private Vector3 GetWaypoint()
    {
        //Debug.Log(waypoints[currentPointIndex]);
        return (waypoints[currentPointIndex]);
    }

    private bool IsAtEndpoint()
    {
        if (currentPointIndex == 0 || currentPointIndex == (waypoints.Count-1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GetNextWaypointIndex()
    {
        if (!loopTrack)
        {
            if (movingForward)
            {
                currentPointIndex++;
                if (currentPointIndex >= waypoints.Count)
                {
                    currentPointIndex = waypoints.Count - 2;//(waypoints.Count-1)
                    movingForward = false;
                }
            }
            else//move backward
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = 1;
                    movingForward = true;
                }
                
                
            }

        }
        else
        {
            currentPointIndex++;
            if (currentPointIndex >= waypoints.Count)
            {
                currentPointIndex = 0;
            }
        }

    }

    private void OnDrawGizmos()//track visibly
    {
        if (waypointsParent == null) return;

        Gizmos.color = Color.white;

        for(int i = 0; i < waypointsParent.childCount; i++)
        {
            Gizmos.DrawSphere(waypointsParent.GetChild(i).position, 0.5f);

            int nextWaypoint = (i + 1);
            if (nextWaypoint >= waypointsParent.childCount) break;

            Gizmos.DrawLine(waypointsParent.GetChild(i).position, waypointsParent.GetChild(nextWaypoint).position);

        }
    }
}
