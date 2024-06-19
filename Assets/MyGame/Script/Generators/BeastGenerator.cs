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
        string name = "";
        int level = Random.Range(1, 10);
        string gender = Random.Range(0, 2) == 0 ? "Male" : "Female";

        // 生成所有属性
        int maxHp = GenerateStat();
        int maxAttack = GenerateStat();
        int maxArmor = GenerateStat();
        int maxAp = GenerateStat();
        int maxMr = GenerateStat();
        int maxSpeed = GenerateStat();

        // 检查是否有任何属性值在 300-350 之间
        bool isSpiritual = maxHp > 299 || maxAttack > 299 || maxArmor > 299 || maxAp > 299 || maxMr > 299 || maxSpeed > 299;
        string type = isSpiritual ? "SpiritualBeast" : "NormalBeast";
        Sprite image = possibleImages[Random.Range(0, possibleImages.Length)];
        if (type == "NormalBeast")
        {
            name = image.name;
        }
        else
        {
            name = "Spiritual " + image.name;
        }

        SpiritualBeast beast = new SpiritualBeast(name, level, gender, type, image, 100, maxHp, maxAttack, maxArmor, maxAp, maxMr, maxSpeed);
        // DebugBeast(beast); // 输出生成的beast的数据
        return beast;
    }


    private int GenerateStat()
    {
        float rand = Random.value; // 生成一个0.0到1.0之间的随机浮点数

        if (rand < 0.15f)
        {
            return Random.Range(0, 101); // 0-100
        }
        else if (rand < 0.60f) // 0.15 + 0.45
        {
            return Random.Range(100, 201); // 100-200
        }
        else if (rand < 0.90f) // 0.15 + 0.45 + 0.30
        {
            return Random.Range(200, 301); // 200-300
        }
        else
        {
            return Random.Range(300, 351); // 300-350
        }
    }

//     private void DebugBeast(SpiritualBeast beast)
//     {
//         Debug.Log("Generated Beast: " +
//                 "\nType: " + beast.type 
//                 // "\nMax HP: " + beast.maxHp +
//                 // "\nMax Attack: " + beast.maxAttack +
//                 // "\nMax Armor: " + beast.maxArmor +
//                 // "\nMax AP: " + beast.maxAp +
//                 // "\nMax MR: " + beast.maxMr +
//                 // "\nMax Speed: " + beast.maxSpeed
//                 );
//     }
}


