using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform startParent;
    private CanvasGroup canvasGroup;
    private LayoutElement layoutElement;
    private bool isDragging;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        layoutElement = GetComponent<LayoutElement>();

        // 你可以在Awake或Start中添加EventTrigger组件
        AddEventTrigger();
    }

    private void AddEventTrigger()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        // 添加BeginDrag事件
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
        trigger.triggers.Add(beginDragEntry);

        // 添加Drag事件
        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(dragEntry);

        // 添加EndDrag事件
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(endDragEntry);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        isDragging = true;

        // 临时禁用LayoutElement，以便在拖动时调整位置
        layoutElement.ignoreLayout = true;

        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        transform.position = eventData.position;

        // 获取指针下的对象
        RectTransform parentRect = startParent.GetComponent<RectTransform>();
        for (int i = 0; i < parentRect.childCount; i++)
        {
            RectTransform child = parentRect.GetChild(i).GetComponent<RectTransform>();
            if (child == transform) continue;

            if (RectTransformUtility.RectangleContainsScreenPoint(child, eventData.position, eventData.pressEventCamera))
            {
                int newSiblingIndex = child.GetSiblingIndex();
                if (transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }
                transform.SetSiblingIndex(newSiblingIndex);
                break;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        canvasGroup.blocksRaycasts = true;
        isDragging = false;

        // 恢复LayoutElement的布局控制
        layoutElement.ignoreLayout = false;

        // 如果没有放到新的位置，恢复到原始位置
        if (eventData.pointerEnter == null || eventData.pointerEnter.transform.parent != startParent)
        {
            transform.position = startPosition;
        }
        else
        {
            transform.SetParent(startParent);
            transform.position = startParent.GetChild(transform.GetSiblingIndex()).position;
        }

        // 刷新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(startParent.GetComponent<RectTransform>());
    }


}
