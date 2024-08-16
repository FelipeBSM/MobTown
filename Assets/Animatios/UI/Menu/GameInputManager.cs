using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager GM;

    public KeyCode exitApp { get; set; }
    public KeyCode breakCar { get; set; }
    public KeyCode hidePhone { get; set; }

    public string cityName { get; set; }

    private void Awake()
    {
        if(GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else if (GM != this)
        {
            Destroy(gameObject);
        }

        exitApp = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("exitPapp", "Backspace"));
        breakCar = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("breakPCar", "Space"));
        hidePhone = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("hidePphone", "Escape"));

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
