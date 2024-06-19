using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastComponent : MonoBehaviour
{
    public SpiritualBeast beast;

    public static SpiritualBeast encounteredBeast;
    public static SpiritualBeast playerFirstBeast;
    public bool participatedInBattle = false; 

    private BeastController beastController; // 假设 BeastMovement 是控制移动的脚本
    private bool isBeingDestroyed = false;

    void Awake()
    {
        if (beast == null)
        {
            beast = new SpiritualBeast();
        }
        // 确保 beastGameObject 被正确设置
        beast.beastGameObject = this.gameObject;
    }

    private void OnDestroy()
    {
        if (!isBeingDestroyed)
        {
            BeastSpawner spawner = FindObjectOfType<BeastSpawner>();
            if (spawner != null)
            {
                spawner.CheckAndGenerateBeasts();
            }
        }
    }

    public void MarkForDestruction()
    {
        isBeingDestroyed = true;
    }


}
