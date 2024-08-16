using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChangerMenu : MonoBehaviour
{
    public List<Button> buttonsList;

    Event keyEvent;
    Text buttonText;
    public GameObject warning;
    KeyCode newKey, prevKey;
    KeyCode keyCantOne, keyCantTwo;


    private string bName;
    bool waitingForKey;

    public GameObject pressAnyKey;
    // Start is called before the first frame update
    void Start()
    {
        pressAnyKey.SetActive(false);
        warning.gameObject.SetActive(false);
        waitingForKey = false;
        for (int i = 0; i < buttonsList.Count; i++)
        {
            if (buttonsList[i].name == "BREAK")
                buttonsList[i].GetComponentInChildren<Text>().text = GameInputManager.GM.breakCar.ToString();
            else if (buttonsList[i].name == "EXITAPP")
                buttonsList[i].GetComponentInChildren<Text>().text = GameInputManager.GM.exitApp.ToString();
            else if (buttonsList[i].name == "HIDE")
                buttonsList[i].GetComponentInChildren<Text>().text = GameInputManager.GM.hidePhone.ToString();

        }
    }

    private void Update()
    {
        if (waitingForKey)
        {
            foreach (Button i in buttonsList)
            {
                i.interactable = false;
                pressAnyKey.SetActive(true);
            }
        }
        else
        {
            foreach (Button i in buttonsList)
            {
                i.interactable = true;
                pressAnyKey.SetActive(false);
            }
        }
    }
    private void OnGUI()
    {
        keyEvent = Event.current;

        if (bName == "break")
        {
            keyCantOne = GameInputManager.GM.exitApp;
            keyCantTwo = GameInputManager.GM.hidePhone;
        }
        else if (bName == "exit")
        {
            keyCantOne = GameInputManager.GM.breakCar;
            keyCantTwo = GameInputManager.GM.hidePhone;
        }
        else if (bName == "hide")
        {
            keyCantOne = GameInputManager.GM.exitApp;
            keyCantTwo = GameInputManager.GM.breakCar;
        }

        if (keyEvent.isKey && waitingForKey)
        {
            if ((keyEvent.keyCode != keyCantOne) && (keyEvent.keyCode != keyCantTwo)
           && (keyEvent.keyCode != KeyCode.W) && (keyEvent.keyCode != KeyCode.S) &&
           (keyEvent.keyCode != KeyCode.UpArrow) && (keyEvent.keyCode != KeyCode.DownArrow)
           && (keyEvent.keyCode != KeyCode.LeftArrow) && (keyEvent.keyCode != KeyCode.RightArrow)
           && (keyEvent.keyCode != KeyCode.A) && (keyEvent.keyCode != KeyCode.D) && (keyEvent.keyCode != KeyCode.Return) &&
           (keyEvent.keyCode != KeyCode.P))
            {
                newKey = keyEvent.keyCode;
                waitingForKey = false;
            }
            else
            {
                newKey = prevKey;
                warning.SetActive(true);
                waitingForKey = false;
            }

        }

    }
    public void OnClicked(Button button)
    {
        string name = button.
        GetComponentInChildren<Text>().text;

        prevKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), name);
    }
    public void StartAssigment(string keyName)
    {
        bName = keyName;
        warning.gameObject.SetActive(false);
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }
    public void SendText(Text text)
    {
        buttonText = text;
    }
    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;

    }
    IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;
        yield return WaitForKey();

        switch (keyName)
        {
            case "break":
                GameInputManager.GM.breakCar = newKey;
                buttonText.text = GameInputManager.GM.breakCar.ToString();
                PlayerPrefs.SetString("breakPCar", GameInputManager.GM.breakCar.ToString());
                break;
            case "exit":
                GameInputManager.GM.exitApp = newKey;
                buttonText.text = GameInputManager.GM.exitApp.ToString();
                PlayerPrefs.SetString("exitPapp", GameInputManager.GM.exitApp.ToString());
                break;
            case "hide":
                GameInputManager.GM.hidePhone = newKey;
                buttonText.text = GameInputManager.GM.hidePhone.ToString();
                PlayerPrefs.SetString("hidePphone", GameInputManager.GM.hidePhone.ToString());
                break;
        }
        yield return null;
    }
}
