using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastManager : MonoBehaviour
{
    public SpiritBagManager spiritbagManager;
    public BeastGenerator beastGenerator;
    void Start()
    {
        // 检查是否是第一次启动游戏
        if (PlayerPrefs.GetInt("IsFirstTime", 1) == 1)
        {
            // 第一次启动游戏，添加初始宠物
            SpiritualBeast initialBeast = beastGenerator.GenerateBeast();
            initialBeast.name = "special";
            spiritbagManager.AddBeast(initialBeast);
            
            // 设置 IsFirstTime 为 0，表示不再是第一次启动
            PlayerPrefs.SetInt("IsFirstTime", 0);
        }
    }


    public void CaptureBeast(string name, int level, string gender, string type, Sprite image, 
    int hp, int attack, int armor, int ap, int mr, int speed)
    {
        SpiritualBeast newBeast = new SpiritualBeast(name, level, gender, type, 
        image, hp, attack, armor, ap, mr, speed);
        spiritbagManager.AddBeast(newBeast);
        Debug.Log("Captured new beast: " + newBeast.name);
    }

    public void ReleaseBeast()
    {
        spiritbagManager.RemoveSelectedBeast();
        Debug.Log("Released selected beast");
    }
}
