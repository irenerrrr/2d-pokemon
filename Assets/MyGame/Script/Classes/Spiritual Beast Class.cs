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
    public int hp;
    public int attack;
    public int armor;
    public int ap;
    public int mr;
    public int speed;


    public SpiritualBeast(string name, int level, string gender, string type, Sprite image, int intimacy,
    int hp, int attack, int armor, int ap, int mr, int speed)
    {
        this.name = name;
        this.level = level;
        this.gender = gender;
        this.type = type;
        this.image = image;

        this.intimacy = intimacy;
        this.hp = hp;
        this.attack = attack;
        this.armor = armor;
        this.ap = ap;
        this.mr = mr;
        this.speed = speed;
    }

}

