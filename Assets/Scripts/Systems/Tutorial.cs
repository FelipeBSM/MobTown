using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> textList;
    public List<GameObject> tips;

    public Text quadText;
    public GameObject motorisrSpeakBox;
    public OpenWheatherMapAPI temp;
    public HudManager hud;
    public AudioSource tipsSource;

    private bool tuto_ONE_complete = false;
    private bool tuto_TWO_complete = false;
    private bool tuto_THREE_complete = false;
    private bool tuto_FOUR_complete = false;
    private bool ggTUTO = false;

    private bool doitOnce_ = false, running = false, hasEndedWriting = false;

    public bool canClickEnter, canClickArrow, canClickBackspace;

    public List<Button> buttons;
    private void Start()
    {
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
       

        if(tuto_ONE_complete == true && tuto_TWO_complete == true
            && tuto_THREE_complete == true && tuto_FOUR_complete == true)
        {
            hud.startMoving = true;
            hud.mainMusic.Play();
            this.gameObject.GetComponent<Tutorial>().enabled = false;
        }



        if (tuto_ONE_complete == false)
        {
            if (doitOnce_ == false)
            {
                motorisrSpeakBox.SetActive(true);
                if (running == false)
                {
                    StartCoroutine(TypeSentence(textList[0], quadText, 0.05f, tips[0]));
                    running = true;
                }
            }
            if(hasEndedWriting == true)
                FirstTuto();
        }
        if (tuto_ONE_complete == true && tuto_TWO_complete == false )
        {

            if (hasEndedWriting == false)
            {
                tips[0].SetActive(false);
                if (doitOnce_ == false)
                {
                    motorisrSpeakBox.SetActive(true);
                    if (running == false)
                    {
                        StartCoroutine(TypeSentence(textList[1], quadText, 0.05f, tips[1]));
                        running = true;
                    }
                }
            }

            if (hasEndedWriting == true)
                SecondTuto();

        }
        if (tuto_TWO_complete == true && tuto_THREE_complete == false)
        {

            if (hasEndedWriting == false)
            {
                tips[1].SetActive(false);
                if (doitOnce_ == false)
                {
                    motorisrSpeakBox.SetActive(true);
                    if (running == false)
                    {
                        tips[2].GetComponentInChildren<Text>().text = "Press " + GameInputManager.GM.exitApp.ToString().ToUpper() + " to exit applications. " +
                            "Press " + GameInputManager.GM.hidePhone.ToString().ToUpper() + " to close your phone.";
                        StartCoroutine(TypeSentence(textList[2], quadText, 0.05f, tips[2]));
                        running = true;
                    }
                }
            }
            if (hasEndedWriting == true)
                ThirdTuto();
            
           
        }
        if (tuto_THREE_complete == true && tuto_FOUR_complete == false)
        {

            if (hasEndedWriting == false)
            {
                tips[2].SetActive(false);
                if (doitOnce_ == false)
                {
                    motorisrSpeakBox.SetActive(true);
                    if (running == false)
                    {

                        StartCoroutine(TypeSentenceFinal(textList[3], quadText, 0.05f, tips[3]));
                        running = true;
                    }
                }
            }
            
            
            //FourthTuto();
        }
      

    }
  
    private void FourthTuto()
    {
      

        doitOnce_ = false;
        running = false;
        hasEndedWriting = false;
        tips[3].SetActive(false);
    }
    private void ThirdTuto()
    {
        canClickBackspace = true;
        hud.SetTutorialDone(canClickBackspace);
        if (Input.GetKeyDown(GameInputManager.GM.exitApp))
        {

            tuto_THREE_complete = true;

            doitOnce_ = false;
            running = false;
            hasEndedWriting = false;
            tips[2].SetActive(false);
        }
    }
    private void FirstTuto()
    {
        //pega o telefone.
        canClickArrow = true;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
          
            tuto_ONE_complete = true;
          
            doitOnce_ = false;
            running = false;
            hasEndedWriting = false;
            tips[0].SetActive(false);
        }


    }
    private void SecondTuto()
    {
        //press entrar em cada app.
        canClickEnter = true;
        foreach(Button b in buttons)
        {
            b.interactable = true;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            tuto_TWO_complete = true;
           
            doitOnce_ = false;
            running = false;
            tips[1].SetActive(false);
            hasEndedWriting = false;
        }

    }

    public IEnumerator TypeSentence(string _sentence, Text textObj, float time, GameObject tip)
    {
        textObj.text = "";
        foreach (char letter in _sentence.ToCharArray())
        {
            textObj.text += letter;
            yield return new WaitForSeconds(time);

        }
        tip.SetActive(true);
        tipsSource.Play();
        hasEndedWriting = true;
    }
    public IEnumerator TypeSentenceFinal(string _sentence, Text textObj, float time,GameObject tip)
    {
        textObj.text = "";
        foreach (char letter in _sentence.ToCharArray())
        {
            textObj.text += letter;
            yield return new WaitForSeconds(time);

        }
        tip.SetActive(true);
        tipsSource.Play();
        StartCoroutine(KillTextBox(tip));
        hasEndedWriting = true;
    }
    public IEnumerator KillTextBox(GameObject t)
    {
       
        yield return new WaitForSeconds(2.0f);

        motorisrSpeakBox.SetActive(false);
        t.SetActive(false);
        tuto_FOUR_complete = true;
        doitOnce_ = true;
       
    }

}
