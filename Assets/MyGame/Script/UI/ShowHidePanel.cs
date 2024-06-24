// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowHidePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;

    void Start() {
        panel.SetActive(false);
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null) {
            trigger = gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        Debug.Log("Mouse entered");
        panel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited");
        panel.SetActive(false);
    }
 
}