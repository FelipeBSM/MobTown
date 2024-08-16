using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCarSound : MonoBehaviour
{
    // Start is called before the first frame update
    private Car myCar;
    private AudioSource mySource;

    private float minPitchValue= 0.5f;
    private float maxPitchValue = 2.5f;
    private float pitchCarValue;
    void Start()
    {
        myCar = GetComponent<Car>();
        mySource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        pitchCarValue = (myCar.GetCarSpeed())/10;
        //Debug.Log("PITCH VALUE: " + pitchCarValue);
        if (pitchCarValue < minPitchValue)
            mySource.pitch = minPitchValue;
        else
        {
            if (pitchCarValue > maxPitchValue)
                mySource.pitch = maxPitchValue;
            else
                mySource.pitch = pitchCarValue;
        }
           
    }
}
