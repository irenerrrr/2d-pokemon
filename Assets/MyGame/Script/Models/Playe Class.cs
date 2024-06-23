using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeClass : MonoBehaviour
{
    public int level = 1;
    public int exp = 0;
    public int expToNextLevel = 1000;

    public List<SpiritualFire> spiritualFires = new List<SpiritualFire>();

    // public void AddExperience(int experience)
    // {
    //     exp += experience;
    //     TryLevelUp();
    // }

    // private void TryLevelUp()
    // {
    //     while (exp >= expToNextLevel)
    //     {
    //         exp -= expToNextLevel;
    //         level++;
    //         expToNextLevel = (int)(expToNextLevel * 1.2f);
    //         Debug.Log($"Player Level Up! New level: {level}, Experience to next level: {expToNextLevel}");
    //     }
    // }
    
}

public class SpiritualFire
{
    // 假设你有一个类SpiritualFire，你可以在这里定义它的属性和方法
    public string name;
    public int powerLevel;
    // 其他属性和方法
}
