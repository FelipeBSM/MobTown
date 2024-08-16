using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HoverMenu : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    // Start is called before the first frame update
    private Button button;
    public AudioSource audioSource;
    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.GetComponent<Animator>().Play("Hover In");
        audioSource.Play();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        button.GetComponent<Animator>().Play("Hover Out");
        audioSource.Stop();
    }
}
