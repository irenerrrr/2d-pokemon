using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastGenerator : MonoBehaviour
{

// 定义 BeastGenerator 类

    public Sprite[] possibleImages; 

    public GameObject beastPrefab; // 新增Prefab引用
    private BeastManager beastManager;
    public int k = 0;

    void Start()
    {
        // 尝试找到 BeastManager
        beastManager = FindObjectOfType<BeastManager>();
        if (beastManager == null)
        {
            Debug.LogError("BeastManager not found in the scene.");
        }
        else
        {
            Debug.Log("BeastManager found.");
        }
    }
    public SpiritualBeast GenerateBeast()
    {
        Sprite image = possibleImages[Random.Range(0, possibleImages.Length)];
        Dictionary<string, int> statLimits = GetStatLimits(image.name);

        string name = "";
        int level = Random.Range(1, 10);
        string gender = Random.Range(0, 2) == 0 ? "Male" : "Female";

        // 生成所有属性
        int Hp = GenerateStatWithLimits(statLimits["Hp"]);
        int Attack = GenerateStatWithLimits(statLimits["Attack"]);
        int Armor = GenerateStatWithLimits(statLimits["Armor"]);
        int Ap = GenerateStatWithLimits(statLimits["Ap"]);
        int Mr = GenerateStatWithLimits(statLimits["Mr"]);
        int Speed = GenerateStatWithLimits(statLimits["Speed"]);


        bool isSpiritual = Random.value < 0.1; // 10% 概率
        string type = isSpiritual ? "SpiritualBeast" : "NormalBeast";
        string ethnicity = image.name;
    
        if (type == "NormalBeast")
        {
            name = image.name;
        }
        else
        {
            name = "Spiritual " + image.name;
        }

        SpiritualBeast beast = new SpiritualBeast(name, ethnicity, level, gender, type, image, 100, 
        Hp, Attack, Armor, Ap, Mr, Speed);
        // DebugBeast(beast); // 输出生成的beast的数据
        return beast;
    }


    private int GenerateStatWithLimits(int limit)
    {
        float rand = Random.value; // 生成一个0.0到1.0之间的随机浮点数
        if (rand < 0.40f)
        {
            // 生成 0 到 30% 上限的值
            return Random.Range(5, Mathf.FloorToInt(limit * 0.3f) + 1);
        }
        else if (rand < 0.80f)
        {
            // 生成 30% 到 60% 上限的值
            return Random.Range(Mathf.FloorToInt(limit * 0.3f), Mathf.FloorToInt(limit * 0.6f) + 1);
        }
        else if (rand < 0.94f)
        {
            // 生成 60% 到 90% 上限的值
            return Random.Range(Mathf.FloorToInt(limit * 0.6f), Mathf.FloorToInt(limit * 0.9f) + 1);
        }
        else
        {
            // 生成 90% 到 100% 上限的值
            return Random.Range(Mathf.FloorToInt(limit * 0.9f), limit + 1);
        }
    }

    //属性字典
    public static Dictionary<string, int> GetStatLimits(string imageName)
    {
        if (imageName == "A1") //飞鱼
        {
            return new Dictionary<string, int>()
            {
                {"Hp", 422},
                {"Ap", 417},
                {"Attack", 186},
                {"Armor", 178},
                {"Mr", 376},
                {"Speed", 521}
            };
        }

        // 默认属性上限
        return new Dictionary<string, int>()
        {
            {"Hp", 350},
            {"Ap", 350},
            {"Attack", 350},
            {"Armor", 350},
            {"Mr", 350},
            {"Speed", 350}
        };
    }

}


