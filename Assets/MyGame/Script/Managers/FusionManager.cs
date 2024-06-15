using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FusionManager : MonoBehaviour
{
    public GameObject fusionPanel; // 拖动你的Fusion Panel到此字段
    public GameObject spiritPanel;


    [System.Serializable]
    public struct BeastUI
    {
        public Image beastImage;
        public TextMeshProUGUI beastName;
        public TextMeshProUGUI beastHP;
        public TextMeshProUGUI beastAttack;
        public TextMeshProUGUI beastArmor;
        public TextMeshProUGUI beastAP;
        public TextMeshProUGUI beastMR;
        public TextMeshProUGUI beastSpeed;
    }

    public BeastUI beastUI_1;
    public BeastUI beastUI_2;

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

    private Dictionary<Button, bool> buttonStates = new Dictionary<Button, bool>();


    void Start()
    {
        fusionPanel.SetActive(false); // 确保Fusion Panel一开始是隐藏的

        selectButton1.interactable = false; // 禁用第一个选择按钮
        selectButton2.interactable = false; // 禁用第二个选择按钮
        BindButtonListeners();
        HideAttributeButtons();

    }

    private void BindButtonListeners()
    {
        selectButton1.onClick.AddListener(SelectSelection1);
        selectButton2.onClick.AddListener(SelectSelection2);
        hpButton.onClick.AddListener(OnHPButtonClick); // 绑定HP按钮的点击事件
        attackButton.onClick.AddListener(OnArmorButtonClick); // 绑定Armor按钮的点击事件
        armorButton.onClick.AddListener(OnArmorButtonClick); // 绑定Armor按钮的点击事件
        apButton.onClick.AddListener(OnAPButtonClick); // 绑定AP按钮的点击事件
        mrButton.onClick.AddListener(OnMRButtonClick); // 绑定MR按钮的点击事件
        speedButton.onClick.AddListener(OnSpeedButtonClick); // 绑定Speed按钮
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
            return;
        }
        if (!firstSlotSelected)
        {
            currentBeast_1 = beast;
            SetBeastUI(beastUI_1, beast);
            selectButton1.interactable = true;
        }
        else
        {
            if (beast == currentBeast_1 || beast.name != currentBeast_1.name)
            {
                return;
            }
            currentBeast_2 = beast;
            SetBeastUI(beastUI_2, beast);
            selectButton2.interactable = true;
            CalculateDifferences();
        }

        ShowFusionPanel();
    }

    private void SetBeastUI(BeastUI ui, SpiritualBeast beast)
    {
        ui.beastImage.gameObject.SetActive(true);
        ui.beastImage.sprite = beast.image;
        ui.beastName.text = beast.name;
        ui.beastHP.text = beast.hp.ToString();
        ui.beastAttack.text = beast.attack.ToString();
        ui.beastArmor.text = beast.armor.ToString();
        ui.beastAP.text = beast.ap.ToString();
        ui.beastMR.text = beast.mr.ToString();
        ui.beastSpeed.text = beast.speed.ToString();
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
        bool wasActiveAndNotInteractable = buttonStates.ContainsKey(button) && buttonStates[button];
        SetButtonState(button, difference > 0, wasActiveAndNotInteractable);
        buttonStates[button] = button.gameObject.activeSelf && !button.interactable;
    }

    private string FormatDifference(int difference)
    {
        return difference <= 0 ? "(+0)" : $"(+1~{difference})";
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
        foreach (var buttonState in buttonStates)
        {
            Button button = buttonState.Key;
            bool wasActiveAndNotInteractable = buttonState.Value;
            if (wasActiveAndNotInteractable)
            {
                SetButtonState(button, true, true);
            }
        }
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

    public void ResetFusionPanel()
    {
        // 清空第一个宠物槽的信息
        ResetBeastUI(beastUI_1);
        currentBeast_1 = null;

        // 清空第二个宠物槽的信息
        ResetBeastUI(beastUI_2);
        currentBeast_2 = null;

        // 禁用选择按钮
        selectButton1.interactable = false;
        selectButton2.interactable = false;

        // 隐藏属性按钮
        HideAttributeButtons();

        // 重置选择状态
        firstSlotSelected = false;
    }

    private void ResetBeastUI(BeastUI ui)
    {
        ui.beastImage.gameObject.SetActive(false); 
        ui.beastName.text = "-";
        ui.beastHP.text = "-";
        ui.beastAttack.text = "-";
        ui.beastArmor.text = "-";
        ui.beastAP.text = "-";
        ui.beastMR.text = "-";
        ui.beastSpeed.text = "-";
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