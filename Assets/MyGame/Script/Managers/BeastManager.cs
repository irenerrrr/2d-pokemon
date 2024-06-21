using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastManager : MonoBehaviour
{
    public SpiritBagManager spiritbagManager;
    public BeastGenerator beastGenerator;

    public static BeastManager Instance { get; private set; }

    private List<GameObject> beasts = new List<GameObject>();

    private static bool first = true;


    void Start()
    {
        // 确保 spiritbagManager 和 beastGenerator 已初始化
        spiritbagManager = FindObjectOfType<SpiritBagManager>();
        if (first)
        {
            initial();
            first = false;
        }
    }
    
    private void initial() 
    {
        for (int i = 0; i < 7; i++) 
        {
            SpiritualBeast newBeast = beastGenerator.GenerateBeast();
            if (newBeast != null)
            {
                newBeast.name = "special";
                newBeast.currentHp = 500;
                newBeast.maxHp = 500;
                newBeast.currentAp = 500;
                newBeast.maxAp = 500;
                spiritbagManager.AddBeast(newBeast);
            }
            else
            {
                Debug.LogError("Failed to generate initial beast.");
                break;
            }
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
