using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManagement : MonoBehaviour
{
    public bool cloudyMoment;
    public bool canLerpClouds = false;
    private Material cloudsMat;

    [Header("System")]
    private DayState dayState;
    public DayNightSycle system;

    

    [Header("Clouds Configs")]
    
    [ColorUsage(true, true)]
    public Color dayCloudsColor;
    [ColorUsage(true, true)]
    public Color nightCloudsColor;
    [ColorUsage(true, true)]
    public Color cloudyDayCloudsColor;
    [ColorUsage(true, true)]
    public Color cloudyNightCloudsColor;

    public float clodyPower;
    public float sunnyPower = 1.8f;

    private float lerpTime = 0f;
    

    private void Start()
    {
        cloudsMat = GetComponent<MeshRenderer>().material;
        cloudyMoment = false; // por enqaunto
        dayState = system.GetDayState();
    }
    private void Update()
    {
        if (canLerpClouds == true)
            RunGod();
    }      
    
    public void RunGod()
    {
        dayState = system.GetDayState();

        if (cloudyMoment == false)
        {
            if (dayState == DayState.DAY)
            {
                cloudsMat.SetColor("Color_c38de1a8b8f54d1ca1cc22c6ed1e3d66", dayCloudsColor);
            }
            else
            {
                cloudsMat.SetColor("Color_c38de1a8b8f54d1ca1cc22c6ed1e3d66", nightCloudsColor);
            }
            cloudsMat.SetFloat("Vector1_ef315f4acc484f02a15d433357996a08", Mathf.Lerp(
                cloudsMat.GetFloat("Vector1_ef315f4acc484f02a15d433357996a08"),sunnyPower,lerpTime));
            //alphaclip
            cloudsMat.SetFloat("Vector1_30cde28449f0464591594bcdd734f727", Mathf.Lerp(
               cloudsMat.GetFloat("Vector1_30cde28449f0464591594bcdd734f727"), 1.85f, lerpTime));
            lerpTime += 0.2f * Time.deltaTime;
            if (lerpTime > 1f)
            {
                lerpTime = 0f;
                canLerpClouds = false;
            }
        }
        else
        {
            //chuva
            if (dayState == DayState.DAY)
            {
                cloudsMat.SetColor("Color_c38de1a8b8f54d1ca1cc22c6ed1e3d66", cloudyDayCloudsColor);
            }
            else
            {
                cloudsMat.SetColor("Color_c38de1a8b8f54d1ca1cc22c6ed1e3d66", cloudyNightCloudsColor);
            }
            
            cloudsMat.SetFloat("Vector1_ef315f4acc484f02a15d433357996a08", Mathf.Lerp(
                cloudsMat.GetFloat("Vector1_ef315f4acc484f02a15d433357996a08"), clodyPower, lerpTime));
            // alphaclip
            cloudsMat.SetFloat("Vector1_30cde28449f0464591594bcdd734f727", Mathf.Lerp(
                cloudsMat.GetFloat("Vector1_30cde28449f0464591594bcdd734f727"), 0f, lerpTime));
            lerpTime += 0.2f * Time.deltaTime;
            if (lerpTime > 1f)
            {
                lerpTime = 0f;
                canLerpClouds = false;
            }
        }
    }
    public void SetCloudsCondition(bool _condition)
    {
        this.cloudyMoment = _condition;
    }
}
