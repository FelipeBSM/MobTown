using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationsIntel : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    [SerializeField] public string streetName;
    [SerializeField] public string locationName;
    public string GetStreetName()
    {
        return this.streetName;
    }
    public string GetLocationName()
    {
        return this.locationName;
    }
}
