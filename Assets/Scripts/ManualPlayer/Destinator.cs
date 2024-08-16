using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destinator : MonoBehaviour
{
    //funcao de indicar para a UI e para o player, o lcal de destino...
    //precisa saber quem eh UI -> ref p UI
    // Start is called before the first frame update
    GameObject destination;
    string streetName, locationName;

    public bool destinationSet;

    string currentStreet;
    bool showMessage;
    public void SetDestination(GameObject _position, string _streetName)
    {
        destination = _position;
        streetName = _streetName;
        destinationSet = true;
    }
    public void SetDestination(GameObject _position, string _streetName,string _locationName)
    {
        destination = _position;
        streetName = _streetName;
        locationName = _locationName;
        destinationSet = true;
    }
    public Transform GetDestinationPosition()
    {
        return this.destination.transform;
    }
    public string GetDestinationStreet()
    {
        return this.streetName;
    }
    public string GetDestinationLocation()
    {
        return this.locationName;
    }
    public void SetCurrentStreet(string street,bool canShowMessage)
    {
        currentStreet = street;
        showMessage = canShowMessage;
    }
    public string GetCurrentStreet()
    {
        return this.currentStreet;
    }
    public bool GetShowState()
    {
        return this.showMessage;
    }
    public void ResetBool()
    {
        showMessage = false;
    }
    public void DirectMessage()
    {
        ResetBool();
    }



}
