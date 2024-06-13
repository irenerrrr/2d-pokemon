using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 根据对象的 y 位置调整 Order in Layer
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
}
