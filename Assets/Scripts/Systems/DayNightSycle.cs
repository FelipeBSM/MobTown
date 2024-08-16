using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DayState
{
    DAY,NIGHT
};
public class DayNightSycle : MonoBehaviour
{
    public GameObject tipH;
    public AudioSource tipSound;
    public DayState dayState;
    [Header("Time Settings")]
    [Range(0.0f,1.0f)]public float time;
    public float dayLenght;
    public Text hourText;
    private float startTime = 0.3f;
    private float timeRate;

    public Vector3 noon; // midDay

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntesity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntesity;

    [Header("Lighting Settings")]
    public AnimationCurve lightingIntesityMul;
    public AnimationCurve reflectionsIntesityMul;

    bool firstNight = true;
    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / dayLenght; // value to update time
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLighting();
        Debug.Log(dayState);
    }
    private void UpdateLighting()
    {
        time += timeRate * Time.deltaTime;
        if (time >= 1f)
            time = 0f;
        ShowHourAndMinute();
        //rotation
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4f;
        //intensity
        sun.intensity = sunIntesity.Evaluate(time);
        moon.intensity = moonIntesity.Evaluate(time);
        //change colors
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        if (sun.intensity <= 0.5f && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
           
        }
        else if (sun.intensity > 0.5f && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);
           
        }
            

        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);
            dayState = DayState.DAY;
        }
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
        {
            if(firstNight == true)
            {
                tipH.SetActive(true);
                tipSound.Play();
                firstNight = false;
                StartCoroutine(EraseTip());
            }
            moon.gameObject.SetActive(true);
            dayState = DayState.NIGHT;
        }
           

        //lighting and reflection
        RenderSettings.ambientIntensity = lightingIntesityMul.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntesityMul.Evaluate(time);
    }
    IEnumerator EraseTip()
    {
        yield return new WaitForSeconds(5f);
        tipH.SetActive(false);
    }
    public DayState GetDayState()
    {
        return this.dayState;
    }
    public void ShowHourAndMinute()
    {
        float t = time * 24f;
        float hours = Mathf.Floor(t);
        t *= 60;
        float minutes = Mathf.Floor(t % 60);
        hourText.text = hours.ToString("00") + ":" + minutes.ToString("00");
        //Debug.Log(hours.ToString("00") + ":" + minutes.ToString("00"));
    }
}
