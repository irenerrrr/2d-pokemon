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

}

