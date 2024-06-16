using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpiritBagManager : MonoBehaviour
{
    public TextMeshProUGUI beastName;
    public TextMeshProUGUI beastLevel;
    public TextMeshProUGUI beastGender;
    public Image beastImage;
    public TextMeshProUGUI beastIntimacy;
    public TextMeshProUGUI beastMaxHP;
    public TextMeshProUGUI beastMaxAttack;
    public TextMeshProUGUI beastMaxArmor;
    public TextMeshProUGUI beastMaxAP;
    public TextMeshProUGUI beastMaxMR;
    public TextMeshProUGUI beastMaxSpeed;

    public FusionManager fusionManager; // 引用 FusionManager

    public GameObject slotPrefab; // Slot 模板的 Prefab
    public Transform slotContainer; // Scroll View 的 Content 容器

    public GameObject detailPanel; // 左半边详细信息面板
    public GameObject spiritBagPanel; // Bag Detail 面板
    public GameObject fusionPanel; 

    private List<SpiritualBeast> beasts = new List<SpiritualBeast>();
    private List<GameObject> slots = new List<GameObject>();
    private int selectedBeastIndex = -1; // 记录当前选中的宠物索引

   

    void Start()
    {
        // 初始化面板状态
        detailPanel.SetActive(false);
        spiritBagPanel.SetActive(false);


    }

    public SpiritualBeast GetFirstBeast()
    {
        if (beasts.Count > 0)
        {
            return beasts[0];
        }
        return null;
    }


    public void AddBeast(SpiritualBeast beast)
    {
        beasts.Add(beast);
        CreateSlot(beast);
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
    }
    
    public void UpdateBeastInfo(SpiritualBeast beast)
    {
        beastImage.sprite = beast.image;
        beastName.text = beast.name;
        beastLevel.text = "Level: " + beast.level;
        beastGender.text = beast.gender;
        
        beastIntimacy.text = "Intimacy: " + beast.intimacy.ToString();
        beastMaxHP.text = "Max HP: " + beast.maxHp.ToString();
        beastMaxAttack.text = "Max Attack: " + beast.maxAttack.ToString();
        beastMaxArmor.text = "Max Armor: " + beast.maxArmor.ToString();
        beastMaxAP.text = "Max AP: " + beast.maxAp.ToString();
        beastMaxMR.text = "Max MR: " + beast.maxMr.ToString();
        beastMaxSpeed.text = "Max Speed: " + beast.maxSpeed.ToString();
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
    
}