using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SpiritBagManager : MonoBehaviour
{
    public TextMeshProUGUI beastName;
    public TextMeshProUGUI beastLevel;
    public TextMeshProUGUI beastGender;
    public Image beastImage;
    public TextMeshProUGUI beastIntimacy;

    public TextMeshProUGUI beastHP, beastAttack, beastArmor, beastAP, beastMR, beastSpeed;

    public Slider hpSlider, apSlider, intimacySlider, levelSlider; 

    public FusionManager fusionManager; // 引用 FusionManager

    public GameObject slotPrefab; // Slot 模板的 Prefab
    public Transform slotContainer; // Scroll View 的 Content 容器

    public GameObject detailPanel; // 左半边详细信息面板
    public GameObject spiritBagPanel; // Bag Detail 面板
    public GameObject fusionPanel; 

    // public static List<SpiritualBeast> beasts = new List<SpiritualBeast>();
    private List<SpiritualBeast> beasts;
    private List<GameObject> slots = new List<GameObject>();
    private int selectedBeastIndex = -1; // 记录当前选中的宠物索引
    public BattleSequenceManager battleSequenceManager; 
    public event Action<SpiritualBeast> BeastSelected; // 添加事件
    
    public TextMeshProUGUI hpSliderText, apSliderText, levelSliderText;  // 用于显示最大血量/当前血量, 显示最大AP/当前AP


    void Start()
    {
        // 初始化面板状态
        detailPanel.SetActive(false);
        spiritBagPanel.SetActive(false);
        hpSlider.interactable = false;
        apSlider.interactable = false;
        intimacySlider.interactable = false;
        levelSlider.interactable = false;
        beasts = BeastManager.beasts;

    }

    public SpiritualBeast GetFirstBeast()
    {
        if (beasts.Count > 0)
        {
            return beasts[0];
        }
        return null;
    }


    public void UpdateBeastInfo(SpiritualBeast beast)
    {
        beastImage.sprite = beast.image;
        beastName.text = beast.name;
        beastLevel.text = "Level " + beast.level;
        beastGender.text = beast.gender;
        
        beastIntimacy.text = "Intimacy: " + beast.intimacy.ToString();
        beastHP.text = "Max HP: " + beast.Hp.ToString();
        beastAttack.text = "Attack: " + beast.Attack.ToString();
        beastArmor.text = "Armor: " + beast.Armor.ToString();
        beastAP.text = "Max Mana/AP: " + beast.Ap.ToString();
        beastMR.text = "MR: " + beast.Mr.ToString();
        beastSpeed.text = "Speed: " + beast.Speed.ToString();

        intimacySlider.maxValue = 100;
        intimacySlider.value = beast.intimacy;
        UpdateSlider(hpSlider, hpSliderText, beast.maxHp, beast.currentHp);
        UpdateSlider(apSlider, apSliderText, beast.maxAp, beast.currentAp);

        // 更新等级滑块和文本
        levelSlider.maxValue = beast.expToNextLevel; 
        levelSlider.value = beast.exp;  // 假设当前经验可以直接用
        levelSliderText.text = $"{beast.exp} / {beast.expToNextLevel}"; // 格式化显示经验文本

    }

    private void UpdateSlider(Slider slider, TextMeshProUGUI sliderText, float maxValue, float currentValue)
    {
        slider.maxValue = maxValue;
        slider.value = currentValue;
        sliderText.text = $"{currentValue} / {maxValue}";
    
    }


    public void CreateSlot(SpiritualBeast beast)
    {
        // Instantiate the slot
        GameObject newSlot = Instantiate(slotPrefab, slotContainer);
        slots.Add(newSlot);

        newSlot.transform.Find("Image").GetComponent<Image>().sprite = beast.image;
        newSlot.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = beast.name;
        newSlot.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level: " + beast.level;
        newSlot.transform.Find("Gender").GetComponent<TextMeshProUGUI>().text = beast.gender;

        // Add button click event
        int index = beasts.Count - 1; // 获取当前宠物的索引
        newSlot.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(index));

    }

    public void OnSlotClick(int index)
    {
        selectedBeastIndex = index; // 记录当前选中的宠物索引
        SpiritualBeast beast = beasts[index];

        if (fusionPanel.activeSelf)
        {
            fusionManager.UpdateFusionPanel(beast);
        }
        else
        {
            // 更新详细信息面板
            detailPanel.SetActive(true);
            UpdateBeastInfo(beast); 
        }

        // 触发事件
        BeastSelected?.Invoke(beast);

    }

    public SpiritualBeast GetSelectedBeast()
    {
        if (selectedBeastIndex >= 0 && selectedBeastIndex < beasts.Count) // 使用 Count 而不是 Length
        {
            return beasts[selectedBeastIndex];
        }
        return null;
    }

    public void RemoveSelectedBeast()
    {
        if (beasts.Count > 2 && selectedBeastIndex >= 0 && selectedBeastIndex < beasts.Count)
        {
            // 移除宠物
            beasts.RemoveAt(selectedBeastIndex);

            // 销毁对应的槽位
            Destroy(slots[selectedBeastIndex]);
            slots.RemoveAt(selectedBeastIndex);

            // 重置选中索引
            selectedBeastIndex = -1;

            // 隐藏详细信息面板
            detailPanel.SetActive(false);
            RefreshUI();
        }
    }

    public void RemoveBeastAt(int index)
    {
        if (index >= 0 && index < beasts.Count)
        {
            SpiritualBeast beast = beasts[index];
            beasts.RemoveAt(index);
            Destroy(slots[index]);
            slots.RemoveAt(index);

            selectedBeastIndex = -1;

            detailPanel.SetActive(false);
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            // 更新每个槽位的信息
            var beast = beasts[i];
            var slot = slots[i];

            slot.transform.Find("Image").GetComponent<Image>().sprite = beast.image;
            slot.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = beast.name;
            slot.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level: " + beast.level;
            slot.transform.Find("Gender").GetComponent<TextMeshProUGUI>().text = beast.gender;

            // 更新点击事件
            var button = slot.GetComponent<Button>();
            int index = i; // 防止闭包问题
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnSlotClick(index));
        }
    }

    public int GetBeastIndex(SpiritualBeast beast)
    {
        return beasts.IndexOf(beast);
    }

    // 获取最新的 currentBeast_1 引用
    public SpiritualBeast GetBeastAt(int index)
    {
        if (index >= 0 && index < beasts.Count)
        {
            return beasts[index];
        }
        return null;
    }

        
}