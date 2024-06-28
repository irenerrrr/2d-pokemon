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

    public Image beastImage;
    public TextMeshProUGUI beastIntimacy;
    public TextMeshProUGUI beastmaxAttack, beastmaxArmor, beastmaxMR, beastmaxSpeed;
    public TextMeshProUGUI beastHP, beastAttack, beastArmor, beastAP, beastMR, beastSpeed;

    public Slider currhpSlider, currapSlider, levelSlider; //当前
    public Slider hpSlider, attackSlider, armorSlider, apSlider, mrSlider, speedSlider;//资质

    public Image drumstickImage;

    public Image genderImage; // 添加性别图像的引用
    public Sprite maleSprite; // 男性图像
    public Sprite femaleSprite; // 女性图像

    public Image lockImage;
    public Sprite lockSprite; // 男性图像
    public Sprite unlockSprite; // 女性图像
    public Button lockButton; 

    public FusionManager fusionManager; // 引用 FusionManager

    public GameObject slotPrefab; // Slot 模板的 Prefab
    public Transform slotContainer; // Scroll View 的 Content 容器

    public GameObject detailPanel; // 左半边详细信息面板
    public GameObject spiritBagPanel; // Bag Detail 面板
    public GameObject fusionPanel; 
    public GameObject battleSequencePanel;

    private List<SpiritualBeast> beasts;
    private List<GameObject> slots = new List<GameObject>();
    public int selectedBeastIndex = -1; // 记录当前选中的宠物索引
    public BattleSequenceManager battleSequenceManager; 
    public event Action<SpiritualBeast> BeastSelected; // 添加事件
    
    public TextMeshProUGUI hpSliderText, apSliderText, levelSliderText;  // 用于显示最大血量/当前血量, 显示最大AP/当前AP
   
    public GameObject Stat1;
    public GameObject Stat2;
    void Start()
    {
        // 初始化面板状态
        detailPanel.SetActive(false);
        spiritBagPanel.SetActive(false);
        currhpSlider.interactable = false;
        currapSlider.interactable = false;
        // intimacySlider.interactable = false;
        levelSlider.interactable = false;

        hpSlider.interactable = false;
        attackSlider.interactable = false;
        armorSlider.interactable = false;
        apSlider.interactable = false;
        mrSlider.interactable = false;
        speedSlider.interactable = false;

        beasts = BeastManager.beasts;

        Stat1.SetActive(true);
        Stat2.SetActive(false);

        if (beasts.Count > 0)
        {
            selectedBeastIndex = 0;
            UpdateCurrentBeastPanel();  // 更新面板信息
        }
    }

    public void SwitchPanels()
    {
        // 检查Panel1的显示状态，然后切换
        bool isStat1Active = Stat1.activeSelf;
        Stat1.SetActive(!isStat1Active);
        Stat2.SetActive(isStat1Active);
        UpdateCurrentBeastPanel();
    }

    public void UpdateCurrentBeastPanel()
    {
        if (selectedBeastIndex >= 0 && selectedBeastIndex < beasts.Count)
        {
            SpiritualBeast selectedBeast = beasts[selectedBeastIndex];
            UpdateBasicPanel(selectedBeast);
            if (Stat1.activeSelf)
            {
                UpdateBStat1Panel(selectedBeast);
            }
            else if (Stat2.activeSelf)
            {
                UpdateBStat2Panel(selectedBeast);
            }
        }
    }

    public SpiritualBeast GetFirstBeast()
    {
        if (beasts.Count > 0)
        {
            return beasts[0];
        }
        return null;
    }


    public void UpdateBasicPanel(SpiritualBeast beast)
    {
        beastImage.sprite = beast.image;
        beastName.text = beast.name;
        beastLevel.text = "Level " + beast.level;

        // 更新等级滑块和文本
        levelSlider.maxValue = beast.expToNextLevel; 
        levelSlider.value = beast.exp;  // 假设当前经验可以直接用
        levelSliderText.text = $"{beast.exp} / {beast.expToNextLevel}"; // 格式化显示经验文本
        UpdateDrumstickFill(beast.intimacy);
        beastIntimacy.text = beast.intimacy.ToString();

        if (beast.gender == "Male")
        {
            genderImage.sprite = maleSprite;
        }
        else if (beast.gender == "Female")
        {
            genderImage.sprite = femaleSprite;
        }

        if (beast.isLock == true)
        {
            lockImage.sprite = lockSprite;
        }
        else if (beast.isLock == false)
        {
            lockImage.sprite = unlockSprite;
        }
    }

    public void ToggleLock()
    {
        SpiritualBeast beast = beasts[selectedBeastIndex];
        beast.isLock = !beast.isLock;
        UpdateBasicPanel(beast);
    }

    private void UpdateDrumstickFill(int intimacy)
    {
        float fillAmount = Mathf.Clamp01(intimacy / 100f); // 将亲密度转换为0到1的值
        drumstickImage.fillAmount = fillAmount;
    }

    public void UpdateBStat1Panel(SpiritualBeast beast)
    {
        beastmaxAttack.text = "Attack: " + beast.maxAttack.ToString();
        beastmaxArmor.text = "Armor: " + beast.maxArmor.ToString();
        beastmaxMR.text = "MR: " + beast.maxMr.ToString();
        beastmaxSpeed.text = "Speed: " + beast.maxSpeed.ToString();

        UpdateSlider(currhpSlider, hpSliderText, beast.maxHp, beast.currentHp);
        UpdateSlider(currapSlider, apSliderText, beast.maxAp, beast.currentAp);
    }

    public void UpdateBStat2Panel(SpiritualBeast beast)
    {

        beastHP.text = "HP: " + beast.Hp.ToString();
        beastAttack.text = "Attack: " + beast.Attack.ToString();
        beastArmor.text = "Armor: " + beast.Armor.ToString();
        beastAP.text = "AP: " + beast.Ap.ToString();
        beastMR.text = "MR: " + beast.Mr.ToString();
        beastSpeed.text = "Speed: " + beast.Speed.ToString();

        var statLimits = BeastGenerator.GetStatLimits(beast.image.name);
        UpdateSlider(hpSlider, null, statLimits["Hp"], beast.Hp, false);
        UpdateSlider(attackSlider, null, statLimits["Attack"], beast.Attack, false);
        UpdateSlider(armorSlider, null, statLimits["Armor"], beast.Armor, false);
        UpdateSlider(apSlider, null, statLimits["Ap"], beast.Ap, false);
        UpdateSlider(mrSlider, null, statLimits["Mr"], beast.Mr, false);
        UpdateSlider(speedSlider, null, statLimits["Speed"], beast.Speed, false);

    }

    private void UpdateSlider(Slider slider, TextMeshProUGUI sliderText, float maxValue, float currentValue, bool updateText = true)
    {
        slider.maxValue = maxValue;
        slider.value = currentValue;
        if (updateText)
        {
            sliderText.text = $"{currentValue} / {maxValue}";
        }
    }


    public void CreateSlot(SpiritualBeast beast)
    {
        // Instantiate the slot
        GameObject newSlot = Instantiate(slotPrefab, slotContainer);
        slots.Add(newSlot);

        newSlot.transform.Find("Image").GetComponent<Image>().sprite = beast.image;

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
        else if (battleSequencePanel.activeSelf)
        {

        }
        else if (detailPanel.activeSelf)
        {
            // 更新详细信息面板
            detailPanel.SetActive(true);
            UpdateCurrentBeastPanel();     
        }

        // 触发事件
        BeastSelected?.Invoke(beast);
        
    }

    //强制刷新raderchartvalue
    public void TriggerBeastSelected()
    {
        if (beasts != null && beasts.Count > 0)
        {
            OnSlotClick(selectedBeastIndex);
        }
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