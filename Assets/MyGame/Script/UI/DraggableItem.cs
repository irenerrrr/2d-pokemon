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
    //private LayoutElement layoutElement;
    //private bool isDragging = false;
    private GameObject placeholder = null;
    
    private int startPositionIndex; 
    // private float updateInterval = 0.00f; // 更新间隔时间，单位：秒
    // private float timeSinceLastUpdate = 0f;
    //private int prevIndex;
    private BattleSequenceManager battleSequenceManager;
    private static List<int> itemOrder = new List<int> { 0, 1, 2, 3, 4 }; 

    void Start()
    {
        battleSequenceManager = FindObjectOfType<BattleSequenceManager>(); // 获取 BattleSequenceManager 的实例
    
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        //layoutElement = GetComponent<LayoutElement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startPositionIndex = transform.GetSiblingIndex(); 
        canvasGroup.blocksRaycasts = false;

        // 创建一个占位符
        placeholder = new GameObject("Placeholder");
        placeholder.transform.SetParent(transform.parent);
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        placeholder.AddComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;

        // 使当前拖拽的格子浮在其他元素之上
        transform.SetParent(transform.parent.parent);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        UpdatePotentialTargets(eventData);
    }

    private void UpdatePotentialTargets(PointerEventData eventData)
    {
        foreach (Transform child in placeholder.transform.parent)
        {
            Image image = child.GetComponent<Image>();
            if (image != null && child != transform && child != placeholder.transform)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(child.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 50 / 255f);  // 透明度调到50%
                else
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 100 / 255f);  // 还原透明度
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 获取目标位置
        Transform target = eventData.pointerEnter != null ? eventData.pointerEnter.transform : placeholder.transform;

        if (target != null && target.parent == placeholder.transform.parent && target != placeholder.transform)
        {
            int targetIndex = target.GetSiblingIndex();

            // 交换位置
            if (targetIndex != startPositionIndex)
            {
                // 将拖动的格子放回原始父对象并交换位置
                transform.SetParent(placeholder.transform.parent);
                transform.SetSiblingIndex(targetIndex);

                // 将目标格子移动到原始位置
                target.SetSiblingIndex(startPositionIndex);
            }
            else
            {
                // 直接放回原位置
                transform.SetParent(placeholder.transform.parent);
                transform.SetSiblingIndex(startPositionIndex);
            }
        }
        else
        {
            // 如果目标无效，则回到原始位置
            transform.SetParent(placeholder.transform.parent);
            transform.SetSiblingIndex(startPositionIndex);
        }

        // 移除占位符
        Destroy(placeholder);

        // 重置颜色
        ResetTargetColors();
    }



    private void ResetTargetColors()
    {
        foreach (Transform child in placeholder.transform.parent)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 100 / 255f);  // 还原透明度
        }
    }
}