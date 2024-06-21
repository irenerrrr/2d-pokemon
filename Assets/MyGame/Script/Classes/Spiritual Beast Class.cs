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
    public int maxHp;
    public int maxAttack;
    public int maxArmor;
    public int maxAp;
    public int maxMr;
    public int maxSpeed;

    public int currentHp;
    public int currentAttack;
    public int currentArmor;
    public int currentAp;
    public int currentMr;
    public int currentSpeed;

    public int exp;
    public int expToNextLevel = 100;
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

        this.currentHp = maxHp;
        this.currentAttack = maxAttack;
        this.currentArmor = maxArmor;
        this.currentAp = maxAp;
        this.currentMr = maxMr;
        this.currentSpeed = maxSpeed;
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

    // public void AddExperience(int experience, int playerLevel)
    // {
    //     exp += experience;
    //     TryLevelUp(playerLevel);
    // }

    // private void TryLevelUp(int playerLevel)
    // {
    //     while (exp >= expToNextLevel && level < playerLevel)
    //     {
    //         exp -= expToNextLevel;
    //         level++;
    //         expToNextLevel = (int)(expToNextLevel * 1.2f);
    //         Debug.Log($"Beast {name} Level Up! New level: {level}, Experience to next level: {expToNextLevel}");
    //     }
    // }

}

