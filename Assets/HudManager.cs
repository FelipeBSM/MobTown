using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HudManager : MonoBehaviour
{
    public GameObject textBox;
    public Text textBox_Text;
    public Text money_Text;

    public Destinator destinator;
    private bool doiItOnce;
    private string sentence;
    public Animator phoneAnim;

    public GameObject mainScreen, bankScreen, mailScreen, mapScreen,storeScreen;
    public Button publicList, GPS,mail;

    public GameObject textBox_Mail;
    public GameObject bolita,bolitaStore;
    public Text contentMessage;
    public Text warningMessage;
    int moneyzao;

    float restoreValue;

    public bool publicListBought;
    public GameObject tip1,tip2,tip3,tip4,tip5;
    public bool startMoving;
    public EventSystem eventSystem;
    private GameObject fSelected;
    public GameObject buttonMap, buttonBank, buttonMail, buttonStore;
    public AudioSource phoneRing,mainMusic;
    public GameObject MechanicalPanel, storePanel, restorePanel, quittingPanel;
    public Health health;
    public Text restoreButtonText;
    public GameObject spawnWhenLeave;
    bool playedSound;
    bool javiu;

    private GameObject mectrigger;
    private GameObject camera,player;

    bool doneTUUT;
    int timesForgot = 0;

    public bool hasAirConditioner = false;
    public Text noMoneyWarning;

    float qualityLevel = 5;
    float qualityMax = 50;

    int resistanceLevel = 5;
    float maxResistance = 50;

    public Image qualityImageBar,resistanceImageBar;
    public Button qualityButton, resistanceButton;

    public float initQualP, initResisP;
    [HideInInspector]public float initQualityPrice, initResistancePrice;

    public GameObject notification,notificationStore;
    public Text notificationText;
    public Text whoIsSpeaking;
    float moneyToLerp;
    int moneyRecived;
    bool lerpTimeNotification;
    int preADDmoneyzao;

    bool doitOnceSotroeNot = true;
    public GameObject faded,pausePanel,optionsPanel,sureQuitPanel;
    // Start is called before the first frame update

    bool isPaused;
    public GameObject tipPause,systemRef;
    public GameObject exitButtonInRestore;
    void Start()
    {
        tipPause.SetActive(false);
        pausePanel.SetActive(false);
        isPaused = false;
        //faded.SetActive(false);
        initQualityPrice = initQualP;
        initResistancePrice = initResisP;
        Cursor.visible = false;
        //MechanicalPanel.SetActive(false);
        javiu = false;
        playedSound = false;
        fSelected = buttonStore;//eventSystem.firstSelectedGameObject;
        eventSystem.firstSelectedGameObject = fSelected;
        startMoving = false;
        //StartCoroutine(EnableTips());
        //mail.interactable = false;
        bolita.SetActive(false);
        bolitaStore.SetActive(false);
        publicListBought = false;
        mainScreen.SetActive(true);
        bankScreen.SetActive(false);
        mailScreen.SetActive(false);
        mapScreen.SetActive(false);
        storeScreen.SetActive(false);
        textBox.SetActive(false);
        doneTUUT = false;
        noMoneyWarning.text = "";
        qualityImageBar.fillAmount = QualityLevelNormalized();
        resistanceImageBar.fillAmount = ResistanceLevelNormalized(); 
    }
  
    public void SetTutorialDone(bool doneTUT)
    {
        doneTUUT = doneTUT;
    }
    private bool doitOnceAfter = true;

    // Update is called once per frame
    public void MakeSelected()
    {
        eventSystem.SetSelectedGameObject(buttonBank);
    }
    public void ExitPhone()
    {
        mainScreen.SetActive(true);
        bankScreen.SetActive(false);
        mailScreen.SetActive(false);
        mapScreen.SetActive(false);
        storeScreen.SetActive(false);
    }
    void Update()
    {
        if (systemRef.GetComponent<Tutorial>().enabled == false&& doitOnceAfter == true)
        {
            tipPause.SetActive(true);
            StartCoroutine(EnableTips());
            doitOnceAfter = false;
        }
            

        if (Input.GetKeyDown(GameInputManager.GM.exitApp) && doneTUUT == true)
        {
            eventSystem.firstSelectedGameObject = fSelected;
            eventSystem.SetSelectedGameObject(fSelected); 
            mainScreen.SetActive(true);
            bankScreen.SetActive(false);
            mailScreen.SetActive(false);
            mapScreen.SetActive(false);
            storeScreen.SetActive(false);


        }
        if (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1))
        {
            eventSystem.firstSelectedGameObject = fSelected;
            eventSystem.SetSelectedGameObject(fSelected);
        }

        if (destinator.destinationSet == true && doiItOnce == false)
        {
            UpdateTextBox();
        }
        
        if(publicListBought == true)
        {
            
            //ja comprei perk
            if (destinator.GetShowState() == true)
            {
                bolita.SetActive(true);
                if(playedSound == false)
                {
                    phoneRing.Play();
                    playedSound = true;
                }
                   
                textBox_Mail.SetActive(true);
                warningMessage.gameObject.SetActive(false);
                contentMessage.text = "Hey! Pick me up now at: "+ destinator.GetCurrentStreet();
                
            }
            else
            {
                contentMessage.text = "OMG! U were to late... Pay attention... I text u back later!";
                textBox_Mail.SetActive(false);
                bolita.SetActive(false);
                playedSound = false;
                warningMessage.gameObject.SetActive(true);
              
            }
        }
        else
        {
            if(moneyzao >= 25 && javiu == false)
            {
                if (doitOnceSotroeNot == true)
                {
                    bolitaStore.SetActive(true);
                    notificationStore.SetActive(true);
                    notificationStore.GetComponent<AudioSource>().Play();
                    StartCoroutine(KillNotification());
                    doitOnceSotroeNot = false;
                }
               
                //playa o som.
            }
        }
        if (qualityLevel >= qualityMax)
            qualityButton.interactable = false;
        if (resistanceLevel >= maxResistance)
            resistanceButton.interactable = false;

        if (lerpTimeNotification == true)
        {
           
            notificationText.text = "+ $"+ moneyRecived.ToString() + "= $"+(preADDmoneyzao+moneyRecived).ToString();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            isPaused = !isPaused;
            Cursor.visible = isPaused;
            optionsPanel.SetActive(false);
            pausePanel.SetActive(isPaused);
            if(isPaused == true)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }

        }

    }
    
    IEnumerator EnableTips()
    {
        yield return new WaitForSeconds(2f);
        tipPause.SetActive(false);
        
    }
    public void UpdateMoney(int money)
    {
        moneyzao += money;
        money_Text.text = "My account: $" + moneyzao.ToString();
        
    }
    private void UpdateTextBox()
    {
        textBox.SetActive(true);
        sentence = "Hello, please take me to: " + destinator.GetDestinationLocation() +
            " at: " + destinator.GetDestinationStreet();
        StartCoroutine(TypeSentence(sentence, textBox_Text, 0.02f));
        
        doiItOnce = true;
        StartCoroutine(KillTextBox());
    }
    public void UpdateBoxWhenInCar()
    {

        textBox.SetActive(true);
        if(timesForgot == 0)
        {
            sentence = "You forgot??? I want to go to " + destinator.GetDestinationLocation() +
          " at: " + destinator.GetDestinationStreet();
        }
        else if(timesForgot == 1)
        {
            sentence = "HEY... YOU LOST YOUR MIND??? I said that i want to go to " + destinator.GetDestinationLocation() +
         " at: " + destinator.GetDestinationStreet();
        }
        else if (timesForgot == 2)
        {
            sentence = "You got to be kidding me! What are you doing? Please take me to" + destinator.GetDestinationLocation() +
         " at: " + destinator.GetDestinationStreet();
            timesForgot = -1;
        }

        StartCoroutine(TypeSentence(sentence, textBox_Text, 0.02f));

        timesForgot++;
        StartCoroutine(KillTextBox());
    }
    IEnumerator KillTextBox()
    {
        yield return new WaitForSeconds(6f);
        textBox.SetActive(false);
        textBox_Text.text = "";
        whoIsSpeaking.text = "Passenger:";
        doiItOnce = false;
        destinator.destinationSet = false; 
    }

    public IEnumerator TypeSentence(string _sentence, Text textObj, float time)
    {
        textObj.text = "";
        foreach (char letter in _sentence.ToCharArray())
        {
            textObj.text += letter;
            yield return new WaitForSeconds(time);

        }

    }
    public void PhoneMove(bool activate)
    {
        phoneAnim.SetBool("phoneMove",activate);
    }
    public void OpenStoreAPP()
    {
        fSelected = buttonStore;
        mainScreen.SetActive(false);
        eventSystem.SetSelectedGameObject(publicList.gameObject);
        Debug.Log(eventSystem.currentSelectedGameObject);
        storeScreen.SetActive(true);
        if (moneyzao >= 25 && javiu == false)
        {
            javiu = true;
            bolitaStore.SetActive(false);
        }
            
            
    }
    public void OpenBankAPP()
    {
        mainScreen.SetActive(false);
        fSelected = buttonBank;
        bankScreen.SetActive(true);
    }
    public void OpenMapAPP()
    {
        mainScreen.SetActive(false);
        fSelected = buttonMap;
        mapScreen.SetActive(true);
    }
    public void OpenMailAPP()
    {
        fSelected = buttonMail;
        mainScreen.SetActive(false);
        bolita.SetActive(false);
        mailScreen.SetActive(true);
    }
    public void UpgardePublicListName()
    {
        if(moneyzao >= 25)
        {
           UpdateMoney(-25);
            money_Text.text = "My account: $" + moneyzao.ToString();
            warningMessage.text = "No new messages! Wait until somebody text you!";
            //mail.interactable = true;
            publicList.interactable = false;
            publicListBought = true;
            //Destroy(publicList);
        }


    }

    public void EnableMechanic(GameObject trig,GameObject cam,GameObject _player,bool F)
    {
        Cursor.visible = true;
        MechanicalPanel.SetActive(true);
        mectrigger = trig;
        camera = cam;
        this.player = _player;
        health.InMechanicalHUD();

        if (health.GetCurrentHealthNormalized() < 1f)
        {
            if (health.currentHealth <= 0)
                exitButtonInRestore.SetActive(false);
            else
                exitButtonInRestore.SetActive(true);
            restorePanel.SetActive(true);

            if (F == true && health.currentHealth <= 0)
            {
                restoreValue = 0f;
                
                _player.GetComponent<Car>().SetFDeath(false);
            }
            else
               restoreValue = 98 * health.GetCurrentMoneyHealthNormalized();

            restoreButtonText.text = "FIX IT - $" +  ((int)restoreValue).ToString();
            storePanel.SetActive(false);
            quittingPanel.SetActive(false);
        }
        else
        {
            restorePanel.SetActive(false);
            storePanel.SetActive(true);
            quittingPanel.SetActive(false);
        }
        
    }
    public void RestoreCar()
    {
       
        player.GetComponent<Car>().onceAndDone = true;
        player.GetComponent<Car>().onceText = true;
        UpdateMoney(-(int)restoreValue);
        health.SetCurrentHealth(health.totalHealth);
        health.SetCurrentDamage(0f);
        restoreValue = 0f;
        //UpdateMoney(moneyzao);
        quittingPanel.SetActive(true);
        restorePanel.SetActive(false);

    }
    public void NoRestore()
    {
        storePanel.SetActive(true);
        restorePanel.SetActive(false);
    }

    public void NoQuit()
    {
        storePanel.SetActive(true);
        quittingPanel.SetActive(false);
    }
    public void ExitMechanic() 
    {
        Cursor.visible = false;
        MechanicalPanel.SetActive(false);
        restorePanel.SetActive(false);
        storePanel.SetActive(false);
        quittingPanel.SetActive(false);
        health.OutMechanicalHUD();
        player.GetComponent<Car>().SetCanMove(true);
        player.transform.position = spawnWhenLeave.transform.position;
        mectrigger.GetComponent<MeshRenderer>().enabled = true;
        mectrigger.GetComponent<BoxCollider>().enabled = true;
        camera.SetActive(false);
        
    }
    public void BuyAirConditioner(Button button) //air
    {
        if(moneyzao >= 100)
        {
            button.interactable = false;
            UpdateMoney(-100);
            hasAirConditioner = true;
        }
        else
        {

            noMoneyWarning.text = "You have no money to buy that...";
            StartCoroutine(KillMoneyWarning());
        }
       
    }
    public void IncreaseQualityLevel(Button button) //confort
    {
        if (moneyzao >= initQualityPrice)
        {
           
            UpdateMoney(Mathf.RoundToInt(-initQualityPrice));
            qualityLevel += 5;
            initQualityPrice += 20;
            button.GetComponentInChildren<Text>().text = "Increase Confort -$" + initQualityPrice.ToString();
            UpdateQualityBar();
        }
        else
        {

            noMoneyWarning.text = "You have no money to buy that...";
            StartCoroutine(KillMoneyWarning());
        }
       
    }
    public void UpdateQualityBar()
    {
        qualityImageBar.fillAmount = QualityLevelNormalized();
    }
    public void UpdateResistanceBar()
    {
        resistanceImageBar.fillAmount = ResistanceLevelNormalized();
    }
    public float QualityLevelNormalized()
    {
        float n = qualityLevel / qualityMax;
        return n;
    }
    public float ResistanceLevelNormalized()
    {
        float n = resistanceLevel / maxResistance;
        return n;
    }
    public float ResistanceLevelNormalizedInverse()
    {
       
        float min = 5f;
        float n = (maxResistance - resistanceLevel) / (maxResistance - min);
        //Debug.Log(n);
        return n;
    }
    public void IncreaseResistanceLevel(Button button) //resistence
    {
        if (moneyzao >= initResistancePrice)
        {

            UpdateMoney(Mathf.RoundToInt(-initResistancePrice));
            resistanceLevel += 5;
            initResistancePrice += 15;
            button.GetComponentInChildren<Text>().text = "Increase Resistence -$" + initResistancePrice.ToString();
            UpdateResistanceBar();
        }
        else
        {

            noMoneyWarning.text = "You have no money to buy that...";
            StartCoroutine(KillMoneyWarning());
        }
       


    } 
    IEnumerator KillMoneyWarning()
    {
        yield return new WaitForSeconds(1.0f);
        noMoneyWarning.text = "";
    }
    public float GetMoneyzao()
    {
        return this.moneyzao;
    }
    public void ShowSpecialText(string text,string _w)
    {
        whoIsSpeaking.text = _w;
        textBox.SetActive(true);
        StartCoroutine(TypeSentence(text, textBox_Text, 0.02f));

        StartCoroutine(KillTextBox());
    }
    public void ShowNotification(float finalMoneyValue)
    {
        preADDmoneyzao = moneyzao;
        moneyRecived = Mathf.RoundToInt(finalMoneyValue);
        //moneyRecived += moneyzao;
        notification.SetActive(true);
        //sound.play
        notification.GetComponent<AudioSource>().Play();
        lerpTimeNotification = true;
        StartCoroutine(KillNotification());
    }
    IEnumerator KillNotification()
    {
        yield return new WaitForSeconds(6f);
        notification.SetActive(false);
        notificationStore.SetActive(false);
        lerpTimeNotification = false;
    }
    public void FadeAnimation(bool _fadeActivate)
    {
        //faded.SetActive(false);
        faded.GetComponent<Animator>().SetBool("ACT", _fadeActivate);
    }
    public void Resume()
    {
        this.pausePanel.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
    public void Options()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void ReturnToMenu()
    {
        sureQuitPanel.SetActive(true);
       
    }
    public void SureQuit()
    {
        Application.Quit();
    }
    public void NoQuitGame()
    {
        sureQuitPanel.SetActive(false);
    }
    public void BackToPause()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

}
