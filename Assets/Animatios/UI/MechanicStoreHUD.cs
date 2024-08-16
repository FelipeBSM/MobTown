using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechanicStoreHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite buttonNormal, buttonCan, buttonCant;
    public Texture2D cursorNormal, cursorGreen, cursorRed;
    public HudManager HUD_INTEL;
    public Button thisButton;
    public float buttonPrice;

    public GameObject activeIntel;
    public bool isAir,isRes,isQual;
    private float moneyIn;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isAir == false)
        {
            activeIntel.SetActive(true);
        }
        else
        {
            moneyIn = buttonPrice;
        }
          
        if (isRes)
            moneyIn = HUD_INTEL.initResistancePrice;
        if (isQual)
            moneyIn = HUD_INTEL.initQualityPrice;
        if (HUD_INTEL.GetMoneyzao() < moneyIn)
        {
            thisButton.GetComponent<Image>().sprite = buttonCant;
            Cursor.SetCursor(cursorRed, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            thisButton.GetComponent<Image>().sprite = buttonCan;
            Cursor.SetCursor(cursorGreen, Vector2.zero, CursorMode.Auto);
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.Auto);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAir == false)
            activeIntel.SetActive(false);
        thisButton.GetComponent<Image>().sprite = buttonNormal;
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.Auto);
      
    }
}
