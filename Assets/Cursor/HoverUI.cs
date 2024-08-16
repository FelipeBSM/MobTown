using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    public Texture2D tex,texnormal;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
        Debug.Log("entrei");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Cursor.SetCursor(texnormal, Vector2.zero, CursorMode.Auto);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(texnormal, Vector2.zero, CursorMode.Auto);
        Debug.Log("sai");
    }
}
