using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastManager : MonoBehaviour
{
    public SpiritBagManager spiritbagManager;
    public BeastGenerator beastGenerator;

    void Start()
    {
        // 确保 spiritbagManager 和 beastGenerator 已初始化
        spiritbagManager = FindObjectOfType<SpiritBagManager>();
        if (spiritbagManager == null)
        {
            Debug.LogError("SpiritBagManager is not assigned in the Inspector.");
            return;
        }

        if (beastGenerator == null)
        {
            Debug.LogError("BeastGenerator is not assigned in the Inspector.");
            return;
        }

        // 检查是否是第一次启动游戏
        SpiritualBeast initialBeast = beastGenerator.GenerateBeast();
        if (initialBeast != null)
        {
            initialBeast.name = "special";
            spiritbagManager.AddBeast(initialBeast);
            Debug.Log("Added initial beast: " + initialBeast.name);
        }
        else
        {
            Debug.LogError("Failed to generate initial beast.");
        }
     
    }


    public void CaptureBeast(string name, int level, string gender, string type, Sprite image, int intimacy,
    int maxHp, int maxAttack, int maxArmor, int maxAp, int maxMr, int maxSpeed)
    {
        SpiritualBeast newBeast = new SpiritualBeast(name, level, gender, type, 
        image, intimacy, maxHp, maxAttack, maxArmor, maxAp, maxMr, maxSpeed);
        spiritbagManager.AddBeast(newBeast);
        Debug.Log("Captured new beast: " + newBeast.name);
    }

    public void ReleaseBeast()
    {
        spiritbagManager.RemoveSelectedBeast();
        Debug.Log("Released selected beast");
    }
}
