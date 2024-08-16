using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityLighter : MonoBehaviour
{
    private DayState dayState;
    private DayNightSycle cicleSystem;

    [Header("CityLights")]
    [SerializeField] private GameObject[] lights;

    // Start is called before the first frame update
    //Turns every light on the city on and off
    void Start()
    {
        cicleSystem = GetComponent<DayNightSycle>();
        dayState = new DayState();
        foreach(GameObject light in lights)
        {
            light.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLights();
    }
    void UpdateLights()
    {
        dayState = cicleSystem.GetDayState();
        switch (dayState)
        {
            case DayState.DAY:
                TurnLightsOff();
                break;
            case DayState.NIGHT:
                TurnLightsOn();
                break;
            default:
                Debug.LogError("Thats an error... Check the day enum");
                break;
        }
    }

    private void TurnLightsOn()
    {
        for(int i=0;i< lights.Length; i++)
        {
            lights[i].SetActive(true);
        }
    }

    private void TurnLightsOff()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
    }
}
