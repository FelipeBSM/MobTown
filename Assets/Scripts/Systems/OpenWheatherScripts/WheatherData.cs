[System.Serializable]
public class OWM_Coord 
{
    public float lon;
    public float lat;
}

[System.Serializable]
public class OWM_Wheather
{
    public int id;
    public string main;
    public string description;
    public string icon;
}
[System.Serializable]
public class OWM_Main
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public int pressure;
    public int humidity;
}
[System.Serializable]
public class OWM_Wind
{
    public float speed;
    public int deg;

}
[System.Serializable]
public class OWM_Clouds
{
    public int all;


}
[System.Serializable]
public class OWM_Sys
{
    public int type;
    public int id;
    public string country;
    public int sunrise;
    public int sunset;


}
[System.Serializable]
public class WheatherData
{
    public OWM_Coord coord;
    public OWM_Wheather[] weather;
    public string basem;
    public OWM_Main main;
    public int visibility;
    public OWM_Wind wind;
    public OWM_Clouds clouds;
    public int dt;
    public OWM_Sys sys;
    public int timezone;
    public int id;
    public string name;
    public int cod;

}
