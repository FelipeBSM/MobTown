using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetInformation:MonoBehaviour
{
   [SerializeField]public string streetName;
   public string GetStreetName()
    {
        return this.streetName;
    }
}
