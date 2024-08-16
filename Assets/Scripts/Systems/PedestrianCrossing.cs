using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianCrossing : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextPoint;
    //info do sinais de transito se houver

    public Vector3 GetDestination()
    {
        return nextPoint.transform.position;
    }
}
