using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SortBeastDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public SpiritBagManager spiritBagManager;
    private List<SpiritualBeast> beasts;

    void Start()
    {
        beasts = BeastManager.beasts;
 
        
    }

    public void GetDropdownValue() 
    {

        int pickedIndex = dropdown.value;
        // 0 = Level↓, 1 = Level↑, 2 = Ethnicity, 3 = BattleSeq
        Debug.Log(pickedIndex);

        switch(pickedIndex)
        {
            case 0:
                SortByLevelDescending();
                break;
            case 1:
                SortByLevelAscending();
                break;
            case 2:
                // Implement Ethnicity sorting
                break;
            case 3:
                // Implement BattleSeq sorting
                break;
        }

        // Refresh the display after sorting
        dropdown.RefreshShownValue();
        spiritBagManager.RefreshUI();
    }

    private void SortByLevelAscending()
    {
        beasts.Sort((a, b) => a.level.CompareTo(b.level));
    }

    private void SortByLevelDescending()
    {
        beasts.Sort((a, b) => b.level.CompareTo(a.level));
    }


}
