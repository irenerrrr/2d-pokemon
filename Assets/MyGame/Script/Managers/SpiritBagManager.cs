using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpiritBagManager : MonoBehaviour
{


    public GameObject slotPrefab; // Slot 模板的 Prefab
    public Transform slotContainer; // Scroll View 的 Content 容器
    public FusionManager fusionManager; // 引用 FusionManager
    
    public TextMeshProUGUI detailName;
    public TextMeshProUGUI detailLevel;
    public TextMeshProUGUI detailGender;
    public Image detailImage;

    public TextMeshProUGUI detailHP;
    public TextMeshProUGUI detailAttack;
    public TextMeshProUGUI detailArmor;
    public TextMeshProUGUI detailAP;
    public TextMeshProUGUI detailMR;
    public TextMeshProUGUI detailSpeed;

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

        if (fusionManager == null)
        {
            Debug.LogError("FusionManager reference is missing in SpiritBagManager.");
        }

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
            
            detailName.text = beast.name;
            detailLevel.text = "Level: " + beast.level;
            detailGender.text = beast.gender;
            detailImage.sprite = beast.image;

            detailHP.text = "HP: " + beast.hp;
            detailAttack.text = "Attack: " + beast.attack;
            detailArmor.text = "Armor: " + beast.armor;
            detailAP.text = "AP: " + beast.ap;
            detailMR.text = "MR: " + beast.mr;
            detailSpeed.text = "Speed: " + beast.speed;
        }
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