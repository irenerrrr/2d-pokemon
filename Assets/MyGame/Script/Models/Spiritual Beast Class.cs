using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritualBeast
{
    public string name;
    public int level;
    public string gender;
    public string type;
    public Sprite image;

    public int intimacy;
    public int maxHp, maxAttack, maxArmor, maxAp, maxMr, maxSpeed;
    public int currentHp, currentAttack, currentArmor, currentAp, currentMr, currentSpeed;

    public int exp;
    public int expToNextLevel;

    public int battleSequence = -1;
    public bool participatedInBattle = false; 
    
    [System.NonSerialized]
    public GameObject beastGameObject; // 存储 GameObject 引用

    // 无参数构造函数
    public SpiritualBeast()
    {
    }

    public SpiritualBeast(string name, int level, string gender, string type, Sprite image, int intimacy,
    int maxHp, int maxAttack, int maxArmor, int maxAp, int maxMr, int maxSpeed)
    {
        this.name = name;
        this.level = level;
        this.gender = gender;
        this.type = type;
        this.image = image;

        this.intimacy = intimacy;
        this.maxHp = maxHp;
        this.maxAttack = maxAttack;
        this.maxArmor = maxArmor;
        this.maxAp = maxAp;
        this.maxMr = maxMr;
        this.maxSpeed = maxSpeed;

        this.currentHp = CalculateCurrent(maxHp, level);
        this.currentAttack = CalculateCurrent(maxAttack, level);
        this.currentArmor = CalculateCurrent(maxArmor, level);
        this.currentAp = CalculateCurrent(maxAp, level);
        this.currentMr = CalculateCurrent(maxMr, level);
        this.currentSpeed = CalculateCurrent(maxSpeed, level);
        
        CalculateExpToNextLevel(); 

    }

    private int CalculateCurrent(int maxValue, int level)
    {
        return Mathf.RoundToInt((maxValue + level) * 1.3f);
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
        }
    }

    public void DecreaseIntimacy(int amount)
    {
        intimacy -= amount;
        if (intimacy < 0)
        {
            intimacy = 0;
        }
        // 可以在这里添加其他的逻辑，比如亲密度下降到一定程度可以触发其他效果
    }

    private void CalculateExpToNextLevel()
    {

        if (level == 1)
        {
            expToNextLevel = 100; // 第1级到第2级初始所需经验
        }
        else
        {
            expToNextLevel = (int)(125 * (level * 3));
        }
        Debug.Log($"Level {level}: Total experience required for next level (accumulative): {expToNextLevel}.");
    }


    public void GainExp(int amount)
    {
        exp += amount;
        Debug.Log($"Gained {amount} EXP. Current EXP: {exp}.");

        while (exp >= expToNextLevel)
        {
            exp -= expToNextLevel;
            level++;
            CalculateExpToNextLevel(); // 更新到下一级所需经验
            Debug.Log($"Level up! New level: {level}. EXP needed for next level: {expToNextLevel}.");
        }
    }

}

