using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FusionManager : MonoBehaviour
{
    public GameObject fusionPanel; // 拖动你的Fusion Panel到此字段
    public GameObject spiritPanel;

    public Image beastImage_1; // 宠物图片
    public TextMeshProUGUI beastName_1; // 宠物名字
    public TextMeshProUGUI beastHP_1; // 宠物HP
    public TextMeshProUGUI beastAttack_1; // 宠物攻击力
    public TextMeshProUGUI beastArmor_1; // 宠物护甲
    public TextMeshProUGUI beastAP_1; // 宠物AP
    public TextMeshProUGUI beastMR_1; // 宠物MR
    public TextMeshProUGUI beastSpeed_1; // 宠物速度

    public Image beastImage_2; // 第二个宠物槽的宠物图片
    public TextMeshProUGUI beastName_2; // 第二个宠物槽的宠物名字
    public TextMeshProUGUI beastHP_2; // 第二个宠物槽的宠物HP
    public TextMeshProUGUI beastAttack_2; // 第二个宠物槽的宠物攻击力
    public TextMeshProUGUI beastArmor_2; // 第二个宠物槽的宠物护甲
    public TextMeshProUGUI beastAP_2; // 第二个宠物槽的宠物AP
    public TextMeshProUGUI beastMR_2; // 第二个宠物槽的宠物MR
    public TextMeshProUGUI beastSpeed_2; // 第二个宠物槽的宠物速度

    public Button selectButton1; // 第一个确认按钮
    public Button selectButton2; // 第二个确认按钮

    public Button hpButton;
    public Button attackButton;
    public Button armorButton;
    public Button apButton;
    public Button mrButton;
    public Button speedButton;

    public TextMeshProUGUI differenceInfoHP;
    public TextMeshProUGUI differenceInfoAttack;
    public TextMeshProUGUI differenceInfoArmor;
    public TextMeshProUGUI differenceInfoAP;
    public TextMeshProUGUI differenceInfoMR;
    public TextMeshProUGUI differenceInfoSpeed;

    private SpiritualBeast currentBeast_1; // 第一个宠物槽的当前选中的宠物
    private SpiritualBeast currentBeast_2; // 第二个宠物槽的当前选中的宠物

    private bool firstSlotSelected = false;

    void Start()
    {
        fusionPanel.SetActive(false); // 确保Fusion Panel一开始是隐藏的
        selectButton1.onClick.AddListener(SelectSelection1); // 绑定第一个确认按钮的点击事件
        selectButton2.onClick.AddListener(SelectSelection2); // 绑定第二个确认按钮的点击事件

        selectButton1.interactable = false; // 禁用第一个选择按钮
        selectButton2.interactable = false; // 禁用第二个选择按钮
            
        hpButton.onClick.AddListener(OnHPButtonClick); // 绑定HP按钮的点击事件
        attackButton.onClick.AddListener(OnArmorButtonClick); // 绑定Armor按钮的点击事件
        armorButton.onClick.AddListener(OnArmorButtonClick); // 绑定Armor按钮的点击事件
        apButton.onClick.AddListener(OnAPButtonClick); // 绑定AP按钮的点击事件
        mrButton.onClick.AddListener(OnMRButtonClick); // 绑定MR按钮的点击事件
        speedButton.onClick.AddListener(OnSpeedButtonClick); // 绑定Speed按钮
        HideAttributeButtons();
        
    }

    public void ShowFusionPanel()
    {
        fusionPanel.SetActive(true);
        spiritPanel.SetActive(true);
    }

    public void HideFusionPanel()
    {
        fusionPanel.SetActive(false);
        spiritPanel.SetActive(false);
    }

    public void UpdateFusionPanel(SpiritualBeast beast)
    {
        if (beast == null)
        {
            Debug.LogError("UpdateFusionPanel received a null beast.");
            return;
        }
        if (!firstSlotSelected)
        {
            currentBeast_1 = beast;
            beastImage_1.sprite = beast.image;
            beastName_1.text = beast.name;
            beastHP_1.text = beast.hp.ToString();
            beastAttack_1.text = beast.attack.ToString();
            beastArmor_1.text = beast.armor.ToString();
            beastAP_1.text = beast.ap.ToString();
            beastMR_1.text = beast.mr.ToString();
            beastSpeed_1.text = beast.speed.ToString();
            selectButton1.interactable = true;
        }
        else
        {
            if (beast == currentBeast_1)
            {
                return;
            }
            if (beast.name != currentBeast_1.name) 
            {
                return;
            }
            currentBeast_2 = beast;
            beastImage_2.sprite = beast.image;
            beastName_2.text = beast.name;
            beastHP_2.text = beast.hp.ToString();
            beastAttack_2.text = beast.attack.ToString();
            beastArmor_2.text = beast.armor.ToString();
            beastAP_2.text = beast.ap.ToString();
            beastMR_2.text = beast.mr.ToString();
            beastSpeed_2.text = beast.speed.ToString();
            selectButton2.interactable = true;
            CalculateDifferences();
          
        }

        ShowFusionPanel();
    }

    public void SelectSelection1()
    {

        if (currentBeast_1 == null)
        {
            Debug.LogError("No beast selected for the first slot.");
            return;
        }
        Debug.Log("Selected first slot beast: " + currentBeast_1.name);
        firstSlotSelected = true;
        selectButton1.interactable = false; // 禁用选择按钮


    }

    public void SelectSelection2()
    {
        selectButton2.interactable = false;// 禁用确认按钮
        EnableAttributeButtons(); // 启用所有属性按钮
        
    }

    private void CalculateDifferences()
    {
        int hpDifference = currentBeast_2.hp - currentBeast_1.hp;
        int attackDifference = currentBeast_2.attack - currentBeast_1.attack;
        int armorDifference = currentBeast_2.armor - currentBeast_1.armor;
        int apDifference = currentBeast_2.ap - currentBeast_1.ap;
        int mrDifference = currentBeast_2.mr - currentBeast_1.mr;
        int speedDifference = currentBeast_2.speed - currentBeast_1.speed;

        UpdateDifferenceUI(differenceInfoHP, hpDifference);
        UpdateDifferenceUI(differenceInfoAttack, attackDifference);
        UpdateDifferenceUI(differenceInfoArmor, armorDifference);
        UpdateDifferenceUI(differenceInfoAP, apDifference);
        UpdateDifferenceUI(differenceInfoMR, mrDifference);
        UpdateDifferenceUI(differenceInfoSpeed, speedDifference);

        UpdateButtonState(hpButton, hpDifference);
        UpdateButtonState(attackButton, attackDifference);
        UpdateButtonState(armorButton, armorDifference);
        UpdateButtonState(apButton, apDifference);
        UpdateButtonState(mrButton, mrDifference);
        UpdateButtonState(speedButton, speedDifference);
    }

    private void UpdateDifferenceUI(TextMeshProUGUI ui, int difference)
    {
        ui.text = FormatDifference(difference);
    }

    private void UpdateButtonState(Button button, int difference)
    {
        SetButtonState(button, difference > 0, false);
    }

    private string FormatDifference(int difference)
    {
        return difference <= 0 ? "(+0)" : $"(+0~{difference})";
    }

    private void HideAttributeButtons()
    {
        SetButtonActive(hpButton, false);
        SetButtonActive(attackButton, false);
        SetButtonActive(armorButton, false);
        SetButtonActive(apButton, false);
        SetButtonActive(mrButton, false);
        SetButtonActive(speedButton, false);
    }



    private void EnableAttributeButtons()
    {
        UpdateButtonState(hpButton, currentBeast_2.hp - currentBeast_1.hp);
        UpdateButtonState(attackButton, currentBeast_2.attack - currentBeast_1.attack);
        UpdateButtonState(armorButton, currentBeast_2.armor - currentBeast_1.armor);
        UpdateButtonState(apButton, currentBeast_2.ap - currentBeast_1.ap);
        UpdateButtonState(mrButton, currentBeast_2.mr - currentBeast_1.mr);
        UpdateButtonState(speedButton, currentBeast_2.speed - currentBeast_1.speed);
    }


    private void SetButtonState(Button button, bool active, bool interactable)
    {
        button.gameObject.SetActive(active);
        button.interactable = interactable;
        ColorBlock colors = button.colors;
        if (active && !interactable)
        {
            colors.normalColor = new Color(0.75f, 0.75f, 0.75f, 0.5f); // 灰色显示，半透明
            colors.highlightedColor = new Color(0.75f, 0.75f, 0.75f, 0.5f); // 灰色显示，半透明
        }
        else
        {
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.white;
        }
        button.colors = colors;
    }
    
    private void SetButtonActive(Button button, bool active)
    {
        button.gameObject.SetActive(active);
    }


 

        
    void OnHPButtonClick()
    {
        Debug.Log("HP Button Clicked");
        // 处理HP按钮点击的逻辑
    }

    void OnArmorButtonClick()
    {
        Debug.Log("Armor Button Clicked");
        // 处理Armor按钮点击的逻辑
    }

    void OnAPButtonClick()
    {
        Debug.Log("AP Button Clicked");
        // 处理AP按钮点击的逻辑
    }

    void OnMRButtonClick()
    {
        Debug.Log("MR Button Clicked");
        // 处理MR按钮点击的逻辑
    }

    void OnSpeedButtonClick()
    {
        Debug.Log("Speed Button Clicked");
        // 处理Speed按钮点击的逻辑
    }
}