using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OpenWheatherMapAPI : MonoBehaviour
{
    public CloudsManagement cloudScript;
    public GameObject rain;
    public AudioSource rainSource;
    public AudioSource thunderSource;
    public string APP_ID = "9ed0f3b20d499bf6bcd98ca1b5c7c411";
    public Text wheatherText;
    public WheatherData weatherInfo;
    public float temp_;

    private float latitude = -2.904004f;//-2.904004f;-30.0277f; // roma
    private float longitude = -79.011063f;//-79.011063f;-51.2285f; // roma
    public string wheatherCondition;

    [Range(0,1)]
    public float currentTime = 1f;

    public float timeToUpdate = 120f;
    private float timeRate;
    // public RawImage weatherIcon;
    // Start is called before the first frame update
    private float timeRateToThunder,totalwaithunder =60;
    [Range(0, 1)]
    public float currentTimeThunder = 1f;

    void Start()
    {
        timeRateToThunder = 1.0f / totalwaithunder;
        timeRate = 1.0f / timeToUpdate;
        ChechCityWeather();
    }
    private void Update()
    {
       
        if(currentTime <= 0)
        {
            ChechCityWeather();
            currentTime = 1f;
        }
        else
        {
            currentTime -= timeRate * Time.deltaTime;
           
        }
        if(wheatherCondition == "Rain" || wheatherCondition == "Thunderstorm")
        {
            if(currentTimeThunder <= 0)
            {
                float per = 0.8f;
                if (Random.value < per)
                {
                    thunderSource.Play();
                }
                currentTimeThunder = 1f;
            }
            else
            {
                currentTimeThunder -= timeRateToThunder * Time.deltaTime;
            }
          
        }
        
    }

    public void ChechCityWeather()
    {
      
        StartCoroutine(GetWheather(1,1));
    }
    IEnumerator GetWheather(float lat,float lon)
    {

        //city = UnityWebRequest.EscapeURL(city);
        Debug.LogError("CU");
        string url = "http://api.openweathermap.org/data/2.5/weather?";
        url += "q=" +GameInputManager.GM.cityName.ToLower() + "&units=metric&lang=pt_br"+"&appid=" + APP_ID;

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        //wheatherText.text = www.downloadHandler.text;
        string json = www.downloadHandler.text;
        json = json.Replace("\"base\":", "\"basem\":");
        weatherInfo = JsonUtility.FromJson<WheatherData>(json);
        //wheatherText.text = weatherInfo.name +"\n";

        temp_ = weatherInfo.main.temp;
        wheatherText.text = weatherInfo.main.temp.ToString("N1") + "°C";
        wheatherCondition = weatherInfo.weather[0].main;
        if(wheatherCondition != "Rain" && wheatherCondition != "Thunderstorm")
        {
            rain.SetActive(false);
            rainSource.Stop();
            
            cloudScript.SetCloudsCondition(false);
        }
        else
        {
            rain.SetActive(true);
            rainSource.Play();

            cloudScript.SetCloudsCondition(true);
        }
        cloudScript.canLerpClouds = true;
      
        //if (weatherInfo.weather.Length > 0)
        //{
        //    StartCoroutine(GetWheatherIcon(weatherInfo.weather[0].icon));
        //}

    }
    //IEnumerator GetWheatherIcon(string icon)
    //{
    //    string url = "http://openweathermap.org/img/wn/"+icon+"@2x.png";
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    //    yield return www.SendWebRequest();

    //    weatherIcon.gameObject.SetActive(true);
    //    weatherIcon.texture = DownloadHandlerTexture.GetContent(www);
    //}
    public string GetWheatherCondition()
    {
        return wheatherCondition;
    }
}
