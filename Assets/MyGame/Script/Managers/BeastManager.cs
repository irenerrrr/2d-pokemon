using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastManager : MonoBehaviour
{
    public SpiritBagManager spiritbagManager;
    public BeastGenerator beastGenerator;

    public static BeastManager Instance { get; private set; }

    public static List<SpiritualBeast> beasts = new List<SpiritualBeast>();
    public static List<SpiritualBeast> sequenceList = new List<SpiritualBeast> {null, null, null, null, null };
    
    private static bool first = true;

    void Start()
    {
        spiritbagManager = FindObjectOfType<SpiritBagManager>();
        if (first)
        {
            initial();
            first = false;
        }
    }
    
    private void initial() 
    {
        for (int i = 0; i < 5; i++) 
        {
            SpiritualBeast newBeast = beastGenerator.GenerateBeast();
            if (newBeast != null)
            {
                newBeast.name = "special_" + i;
                newBeast.currentHp = 10;
                newBeast.maxHp = 10;
                newBeast.currentAp = 200;
                newBeast.maxAp = 200;
                CaptureBeast(newBeast);
                
            }

        }
    }

    public void CaptureBeast(SpiritualBeast beast) 
    {
        beasts.Add(beast);
        spiritbagManager.CreateSlot(beast);
    }



    // public void ReleaseBeast()
    // {
    //     spiritbagManager.RemoveSelectedBeast();
    //     Debug.Log("Released selected beast");
    // }


}
