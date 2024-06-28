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
    private bool isDragging = false;
    private GameObject placeholder = null;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        layoutElement = GetComponent<LayoutElement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        isDragging = true;

        // 临时禁用LayoutElement，以便在拖动时调整位置
        layoutElement.ignoreLayout = true;

        // 创建一个占位符
        placeholder = new GameObject("Placeholder");
        RectTransform rectTransform = placeholder.AddComponent<RectTransform>();
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = layoutElement.preferredWidth;
        le.preferredHeight = layoutElement.preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetParent(startParent);
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        // 在原始位置放置占位符
        placeholder.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
        placeholder.GetComponent<RectTransform>().localScale = Vector3.one; // 确保缩放一致

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        transform.position = eventData.position;

        // 获取指针下的对象
        RectTransform parentRect = startParent.GetComponent<RectTransform>();
        int currentPlaceholderIndex = placeholder.transform.GetSiblingIndex();
        int newSiblingIndex = currentPlaceholderIndex;

        for (int i = 0; i < parentRect.childCount; i++)
        {
            RectTransform child = parentRect.GetChild(i).GetComponent<RectTransform>();
            
            if (child == transform || child == placeholder.transform) continue;

            if (RectTransformUtility.RectangleContainsScreenPoint(child, eventData.position, eventData.pressEventCamera))
            {
                newSiblingIndex = child.GetSiblingIndex();
                if (newSiblingIndex > currentPlaceholderIndex)
                {
                    newSiblingIndex--;
                }
                Debug.Log($"Detected position at child {i}: {child.name}, Moving placeholder to index {newSiblingIndex}");
                break;
            }
        }

        if (newSiblingIndex != currentPlaceholderIndex)
        {
            Debug.Log($"Moving placeholder from index {currentPlaceholderIndex} to {newSiblingIndex}"); // 添加调试日志
            placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }

   
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        canvasGroup.blocksRaycasts = true;
        isDragging = false;

        // 恢复LayoutElement的布局控制
        layoutElement.ignoreLayout = false;

        // 设置物体到占位符位置
        transform.SetParent(startParent);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        transform.position = startParent.GetChild(transform.GetSiblingIndex()).position;

        // 删除占位符
        Destroy(placeholder);

        // 刷新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(startParent.GetComponent<RectTransform>());

    }
}