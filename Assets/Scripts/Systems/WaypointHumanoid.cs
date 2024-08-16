using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointHumanoid : MonoBehaviour
{
    public WaypointHumanoid previousWaypoint;
    public WaypointHumanoid nextWaypoint;

    [Range(0f,5f)]
    public float width;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
