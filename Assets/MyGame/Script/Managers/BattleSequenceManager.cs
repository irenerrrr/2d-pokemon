using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BattleSequenceManager : MonoBehaviour

{
    public GameObject[] slots; // 拖拽到Inspector中的五个格子
    private GameObject currentActiveCircle; // 高亮圈圈  
    private SpiritBagManager spiritBagManager;
    private List<SpiritualBeast> sequenceList;

    public int prevIndex = -1;
   
    void Start()
    {
        //HighlightFirstEmptySlot();
        sequenceList = BeastManager.sequenceList;
      
    }

    void Awake()
    {
        // Initialize interaction on all slots once at startup
        for (int i = 0; i < slots.Length; i++)
        {
            AddClickEventToSlot(i);
        }
    }

    private void AddClickEventToSlot(int slotIndex)
    {
        Debug.Log($"Adding event listener to slot {slotIndex}");
        EventTrigger trigger = slots[slotIndex].GetComponent<EventTrigger>();
        trigger.triggers.Clear(); // Clear existing triggers to prevent duplicates

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((data) => { OnSlotClick(slotIndex); });
        trigger.triggers.Add(entry);
    }

    public void OnSlotClick(int index)
    {
        UpdateSlotCircle(index);
    }

    public void UpdateSlotCircle(int index)
    {
        Debug.Log($"Slot {index} clicked");
        
        // 隐藏之前选中格子的circle
        if (prevIndex != -1 && prevIndex < slots.Length)
        {
            Transform prevCircle = slots[prevIndex].transform.Find("Circle");
            prevCircle.gameObject.SetActive(false);
         
        }

        // 显示当前选中格子的circle
        if (index >= 0 && index < slots.Length)
        {
            Transform circle = slots[index].transform.Find("Circle");
            circle.gameObject.SetActive(true);
            // 更新prevIndex
            prevIndex = index;
        }

    }

    //用于设置 prevIndex
    public void SetPrevIndex(int newIndex)
    {
        prevIndex = newIndex;
        OnSlotClick(prevIndex);
    }

    //用于获取 prevIndex
    public int GetPrevIndex()
    {
        return prevIndex;
    }

    public void UpdateCurrentSlot(SpiritualBeast beast)
    {
        Debug.Log("update");
        if (prevIndex < slots.Length)
        {
            
            Transform slot = slots[prevIndex].transform;
            Transform blankPanel = slot.Find("BlankPanel");
            Transform infoPanel = slot.Find("InfoPanel");

         
            blankPanel.gameObject.SetActive(false);
            infoPanel.gameObject.SetActive(true);

            sequenceList[prevIndex] = beast;
            Debug.Log(sequenceList);
            // 更新infoPanel中的内容
            UpdateSlotInfo(infoPanel, beast);
            //HighlightFirstEmptySlot();

            
        }
    }

    private void UpdateSlotInfo(Transform infoPanel, SpiritualBeast beast)
    {
        Transform topPanel = infoPanel.Find("topPanel");
        Transform LevelTextPanel = infoPanel.Find("LevelTextPanel");
        Transform LevelPanel = infoPanel.Find("LevelPanel");
        Transform page_1 = infoPanel.Find("page_1");
        Transform HpPanel = page_1.Find("HpPanel");
        Transform ApPanel = page_1.Find("ApPanel");
        Transform IntimacyPanel = page_1.Find("IntimacyPanel");

        // 更新infoPanel中的内容
        Image beastImage = topPanel.Find("BeastImage").GetComponent<Image>();
        //TextMeshProUGUI beastName = infoPanel.Find("BeastName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI beastLevel = LevelTextPanel.Find("beastLevel").GetComponent<TextMeshProUGUI>();
        Slider levelSlider = LevelPanel.Find("Slider").GetComponent<Slider>();
        TextMeshProUGUI levelSliderText = LevelPanel.Find("LevelSliderText").GetComponent<TextMeshProUGUI>();

        Slider currhpSlider = HpPanel.Find("CurrentHPSlider").GetComponent<Slider>();
        TextMeshProUGUI hpSliderText = HpPanel.Find("HPSliderText").GetComponent<TextMeshProUGUI>();
        Slider currapSlider = ApPanel.Find("CurrentAPSlider").GetComponent<Slider>();
        TextMeshProUGUI apSliderText = ApPanel.Find("APSliderText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI beastIntimacy = IntimacyPanel.Find("BeastIntimacy").GetComponent<TextMeshProUGUI>();
        Image drumstickImage = IntimacyPanel.Find("drumstickImage").GetComponent<Image>();

        // 更新信息
        beastImage.sprite = beast.image;
        //beastName.text = beast.name;
        beastLevel.text = "Level " + beast.level;

        // 更新等级滑块和文本
        levelSlider.maxValue = beast.expToNextLevel;
        levelSlider.value = beast.exp;  // 假设当前经验可以直接用
        levelSliderText.text = $"{beast.exp} / {beast.expToNextLevel}"; // 格式化显示经验文本

        UpdateDrumstickFill(beast.intimacy, drumstickImage);
        beastIntimacy.text = beast.intimacy.ToString();

        UpdateSlider(currhpSlider, hpSliderText, beast.maxHp, beast.currentHp);
        UpdateSlider(currapSlider, apSliderText, beast.maxAp, beast.currentAp);
    }

    public void UpdateSlider(Slider slider, TextMeshProUGUI sliderText, float maxValue, float currentValue)
    {
        slider.maxValue = maxValue;
        slider.value = currentValue;
        sliderText.text = $"{currentValue} / {maxValue}";
    
    }

    public void UpdateDrumstickFill(int intimacy, Image drumstickImage)
    {
        float fillAmount = Mathf.Clamp01(intimacy / 100f); // 将亲密度转换为0到1的值
        drumstickImage.fillAmount = fillAmount;
    }

 
}







    // [SerializeField] private TMP_Dropdown dropdown;
    // public SpiritBagManager spiritBagManager;
    // private List<SpiritualBeast> sequenceList;
    // private SpiritualBeast currentBeast;
  

    // void Start()
    // {
    //     sequenceList = BeastManager.sequenceList;
    //     List<string> options = new List<string> {"Battle", "#1", "#2", "#3", "#4", "#5" };
    //     dropdown.ClearOptions();
    //     dropdown.AddOptions(options);
    //     if (spiritBagManager != null)
    //     {
    //         // 注册事件，确保只有在数据发生变化时才进行更新
    //         spiritBagManager.BeastSelected += UpdateDropdownDisplay;
    //     }
    //     UpdateDropdownDisplay(null);
    // }

    // void OnDestroy()
    // {
    //     if (spiritBagManager != null)
    //     {
    //         // 注销事件，防止内存泄漏
    //         spiritBagManager.BeastSelected -= UpdateDropdownDisplay;
    //     }
    // }

    // public void GetDropDownValue()
    // {
    //     int pickedValue = dropdown.value - 1; // 补偿因为"Battle"选项带来的偏移
    //     if (pickedValue < 0) pickedValue = -1; // 如果选择了"Battle"，则设置为-1

    //     SpiritualBeast selectedBeast = spiritBagManager.GetSelectedBeast();
    //     int currSequence = selectedBeast.battleSequence;

    //     if (currSequence != pickedValue)
    //     {
    //         if (pickedValue == -1) 
    //         {
    //             SortList(selectedBeast);
    //         } 
    //         else 
    //         {
    //             Check(currSequence, pickedValue, selectedBeast);
    //         }
           
    //         spiritBagManager.UpdateBStat1Panel(selectedBeast);
    //         UpdateDropdownDisplay(selectedBeast);
    //     }
    // }

    // private void SortList(SpiritualBeast selectedBeast)
    // {
    //     bool found = false;
    //     for (int i = 0; i < sequenceList.Count - 1; i++) 
    //     {
    //         if (sequenceList[i] == selectedBeast) 
    //         {
    //             found = true;
    //         }
    //         if (found && i < sequenceList.Count - 1) 
    //         {
    //             sequenceList[i] = sequenceList[i + 1];
    //             if (sequenceList[i] != null) 
    //             {
    //                 sequenceList[i].battleSequence -= 1;
    //             }
    //         }
    //     }

    //     // 设置最后一个位置为空
    //     if (found && sequenceList.Count > 1)
    //     {
    //         sequenceList[sequenceList.Count - 1] = null;
    //     }
    //     selectedBeast.battleSequence = -1;
    // }

    
    // public void UpdateDropdownDisplay(SpiritualBeast selectedBeast)
    // {
    //     dropdown.interactable = !PlayerController.isInBattle;  // 根据 isInBattle 状态禁用或启用 dropdown
    //     if (selectedBeast == null || selectedBeast.battleSequence == -1)
    //     {
    //         dropdown.captionText.text = "Battle";
    //         dropdown.value = 0; // 确保Dropdown的值与显示文本一致
    //     }
    //     else
    //     {
    //         dropdown.captionText.text = "#" + selectedBeast.battleSequence;
    //         dropdown.value = selectedBeast.battleSequence + 1; // 由于"Battle"占据了第一个选项，序列值需要加1
    //     }

    //     dropdown.RefreshShownValue();
    // }

    // public void Check(int currentSequence, int TargetSequence, SpiritualBeast currentBeast) 
    // {
  
    //     // 如果当前位置或目标位置有一个是空的，跳转到CheckBackward
    //     if (currentSequence == -1 || sequenceList[TargetSequence] == null) 
    //     {
    //         CheckBackward(currentSequence, TargetSequence, currentBeast, false);
    //         return;
    //     }
    //     // 位置调整
    //     if (currentSequence > TargetSequence)
    //     {
            
    //         // 把其他位置依次后移
    //         for (int i = currentSequence; i > TargetSequence; i--)
    //         {
    //             sequenceList[i] = sequenceList[i - 1];
    //             if (sequenceList[i] != null)
    //             {
    //                 sequenceList[i].battleSequence = i;
    //             }
    //         }
            
    //         // 把当前元素放到目标位置
    //         sequenceList[TargetSequence] = currentBeast;
    //         currentBeast.battleSequence = TargetSequence;
    //     }
    //     // 位置互换
    //     else if (currentSequence < TargetSequence)
    //     {
    //         SpiritualBeast tempBeast = sequenceList[TargetSequence];
    //         sequenceList[TargetSequence] = currentBeast;
    //         sequenceList[TargetSequence].battleSequence = TargetSequence;

    //         sequenceList[currentSequence] = tempBeast;
    //         sequenceList[currentSequence].battleSequence = currentSequence;
    //     }

    // }

    // //if already checked checkbackward, hasCheckedBackward = true
    // public void CheckBackward(int currentSequence, int targetSequence, SpiritualBeast currentBeast, bool hasCheckedBackward)
    // {
    //     //如果目标位置是6，则将当前精灵兽的序列设为-1，并返回
    //     if (targetSequence == 5 && hasCheckedBackward)
    //     {
    //         currentBeast.battleSequence = -1;
    //         return;
    //     }

    //     // 如果目标位置不为空，把新的替换原来的，然后拿原来的去找他后面的同学
    //     if (sequenceList[targetSequence] != null)
    //     {
    //         // 获取下一个位置的精灵兽
    //         SpiritualBeast nextBeast = sequenceList[targetSequence];

    //         // 将当前精灵兽移到目标位置
    //         sequenceList[targetSequence] = currentBeast;
    //         currentBeast.battleSequence = targetSequence;

    //         // 递归检查下一个位置
    //         CheckBackward(targetSequence , targetSequence + 1, nextBeast, true);
    //     }

    //     // 如果目标位置为空，并且已经进行了后退检查
    //     else if (sequenceList[targetSequence] == null && hasCheckedBackward)
    //     {
    //         sequenceList[targetSequence] = currentBeast;
    //         currentBeast.battleSequence = targetSequence;
    //     }

    //     // 如果目标位置为空，调用CheckForward方法
    //     else
    //     {
    //         CheckForward(currentSequence, targetSequence, currentBeast);
    //     }
    // }

    // public void CheckForward(int currentSequence, int targetSequence, SpiritualBeast currentBeast) 
    // {
    //     // 从当前序列开始向前检查，确保前一个位置不为空
    //     for (int i = targetSequence; i > currentSequence; i--) 
    //     {
    //         if (i == 0 || sequenceList[i - 1] != null) 
    //         {
    //             // 把当前精灵兽放到这个位置
    //             currentBeast.battleSequence = i;
    //             sequenceList[i] = currentBeast;
    //             return;
    //         }
    //     }
    // }



// }