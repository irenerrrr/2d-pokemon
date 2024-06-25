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
    public int Hp, Attack, Armor, Ap, Mr, Speed;
    public int currentHp, currentAttack, currentArmor, currentAp, currentMr, currentSpeed;
    public int maxHp, maxAttack, maxArmor, maxAp, maxMr, maxSpeed;

    public int exp;
    public int expToNextLevel { get; private set; }

    public int battleSequence = -1;
    public bool participatedInBattle = false; 
    public string ethnicity;
    [System.NonSerialized]
    public GameObject beastGameObject; // 存储 GameObject 引用

    // 无参数构造函数
    public SpiritualBeast()
    {
    }

    public SpiritualBeast(string name, string ethnicity, int level, string gender, string type, Sprite image, int intimacy,
    int Hp, int Attack, int Armor, int Ap, int Mr, int Speed)
    {
        this.name = name;
        this.ethnicity = ethnicity;
        this.level = level;
        this.gender = gender;
        this.type = type;
        this.image = image;

        this.intimacy = intimacy;
        this.Hp = Hp;
        this.Attack = Attack;
        this.Armor = Armor;
        this.Ap = Ap;
        this.Mr = Mr;
        this.Speed = Speed;
        

        InitializeStats(Hp, Attack, Armor, Ap, Mr, Speed);
        CalculateExpToNextLevel(); 

    }
    private void InitializeStats(int Hp, int Attack, int Armor, int Ap, int Mr, int Speed)
    {
        this.maxHp = CalculateCurrent(Hp, level);
        this.maxAttack = CalculateCurrent(Attack, level);
        this.maxArmor = CalculateCurrent(Armor, level);
        this.maxAp = CalculateCurrent(Ap, level);
        this.maxMr = CalculateCurrent(Mr, level);
        this.maxSpeed = CalculateCurrent(Speed, level);

        this.currentHp = CalculateCurrent(Hp, level);
        this.currentAttack = CalculateCurrent(Attack, level);
        this.currentArmor = CalculateCurrent(Armor, level);
        this.currentAp = CalculateCurrent(Ap, level);
        this.currentMr = CalculateCurrent(Mr, level);
        this.currentSpeed = CalculateCurrent(Speed, level);
    }

    private int CalculateCurrent(int Value, int level)
    {
        return Mathf.RoundToInt((Value + level) * 1.3f);
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

    public void CalculateExpToNextLevel()
    {

        if (level == 1)
        {
            expToNextLevel = 100; // 第1级到第2级初始所需经验
        }
        else
        {
            expToNextLevel = (int)(125 * (level * 3));
        }
        //Debug.Log($"Level {level}: Total experience required for next level (accumulative): {expToNextLevel}.");
    }


    public void GainExp(int amount)
    {
        exp += amount;
        Debug.Log($"Gained {amount} EXP. Current EXP: {exp}.");

        while (exp >= expToNextLevel)
        {
            exp -= expToNextLevel;
            level++;

            bool wasDead = currentHp == 0;
            InitializeStats(Hp, Attack, Armor, Ap, Mr, Speed);
            if (wasDead)
            {
                currentHp = 0; // 如果在升级前已经死亡，则保持死亡状态
            }

            CalculateExpToNextLevel(); // 更新到下一级所需经验
            Debug.Log($"Level up! New level: {level}. EXP needed for next level: {expToNextLevel}.");
        }
    }

}

