using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FusionManager : MonoBehaviour
{
    public SpiritBagManager spiritBagManager;
    private int currentBeast1Index = -1; // 记录 currentBeast_1 的索引
    private int currentBeast2Index = -1; // 记录 currentBeast_2 的索引

    public GameObject fusionPanel; // 拖动你的Fusion Panel到此字段
    public GameObject spiritPanel;
    public GameObject fusionSuccessPanel; 
    
    public Button confirmButton; 

    public TextMeshProUGUI beastNameText; // 拖动你的Fusion success panel中的beast name文本元素到此字段
    public Image beastImage; // 拖动你的Fusion success panel中的beast image元素到此字段
    public TextMeshProUGUI beastHPText; // 拖动你的Fusion success panel中的beast HP文本元素到此字段
    public TextMeshProUGUI beastAttackText; // 拖动你的Fusion success panel中的beast attack文本元素到此字段
    public TextMeshProUGUI beastArmorText; // 拖动你的Fusion success panel中的beast armor文本元素到此字段
    public TextMeshProUGUI beastAPText; // 拖动你的Fusion success panel中的beast AP文本元素到此字段
    public TextMeshProUGUI beastMRText; // 拖动你的Fusion success panel中的beast MR文本元素到此字段
    public TextMeshProUGUI beastSpeedText; // 拖动你的Fusion success panel中的beast speed文本元素到此字段



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
        fusionSuccessPanel.SetActive(false);
        ResetFusionPanel();

        selectButton1.interactable = false; // 禁用第一个选择按钮
        selectButton2.interactable = false; // 禁用第二个选择按钮
        BindButtonListeners();
        HideAttributeButtons();
        confirmButton.onClick.AddListener(OnConfirmButtonClick); 

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
            currentBeast1Index = spiritBagManager.GetBeastIndex(beast); // 记录 currentBeast_1 的索引
            SetBeastUI(beastUI_1, beast);
            selectButton1.interactable = true;
        }
        else
        {
            if (beast == currentBeast_1 || beast.name != currentBeast_1.name)
            {
                return;
            }
            if (currentBeast_2 != null && beast == currentBeast_2) {
                return;
            }
            ResetPanelAndButtonStates();
            currentBeast_2 = beast;
            currentBeast2Index = spiritBagManager.GetBeastIndex(beast); // 记录 currentBeast_2 的索引
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
        ui.beastHP.text = beast.maxHp.ToString();
        ui.beastAttack.text = beast.maxAttack.ToString();
        ui.beastArmor.text = beast.maxArmor.ToString();
        ui.beastAP.text = beast.maxAp.ToString();
        ui.beastMR.text = beast.maxMr.ToString();
        ui.beastSpeed.text = beast.maxSpeed.ToString();
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
        int hpDifference = currentBeast_2.maxHp - currentBeast_1.maxHp;
        int attackDifference = currentBeast_2.maxAttack - currentBeast_1.maxAttack;
        int armorDifference = currentBeast_2.maxArmor - currentBeast_1.maxArmor;
        int apDifference = currentBeast_2.maxAp - currentBeast_1.maxAp;
        int mrDifference = currentBeast_2.maxMr - currentBeast_1.maxMr;
        int speedDifference = currentBeast_2.maxSpeed - currentBeast_1.maxSpeed;

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

    private void HideAttributeButtons()
    {
        SetButtonActive(hpButton, false);
        SetButtonActive(attackButton, false);
        SetButtonActive(armorButton, false);
        SetButtonActive(apButton, false);
        SetButtonActive(mrButton, false);
        SetButtonActive(speedButton, false);
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

        currentBeast1Index = -1; // 重置索引
        currentBeast2Index = -1; // 重置索引

        // 隐藏属性按钮
        HideAttributeButtons();

        // 重置选择状态
        firstSlotSelected = false;

        UpdateDifferenceUI(differenceInfoHP, 0);
        UpdateDifferenceUI(differenceInfoAttack, 0);
        UpdateDifferenceUI(differenceInfoArmor, 0);
        UpdateDifferenceUI(differenceInfoAP, 0);
        UpdateDifferenceUI(differenceInfoMR, 0);
        UpdateDifferenceUI(differenceInfoSpeed, 0);
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

    private void ResetPanelAndButtonStates()
    {
        // 重置按钮状态
        HideAttributeButtons();
        buttonStates.Clear();

        // 重置UI信息
        ResetBeastUI(beastUI_2);
      
    }

 

        
    void OnHPButtonClick()
    {
        Debug.Log("HP Button Clicked");
  
        int hpDifference = currentBeast_2.maxHp - currentBeast_1.maxHp;

        int maxN = CalculateMaxN(hpDifference, currentBeast_1.maxHp);
  
        int randomHPDifference = Random.Range(1, maxN + 1);

        Debug.Log(randomHPDifference);
        UpdateFusionSuccessPanel();
        beastHPText.text = "HP " + currentBeast_1.maxHp.ToString() + " -> " + (currentBeast_1.maxHp + randomHPDifference).ToString();
        currentBeast_1.maxHp += randomHPDifference;
     
        ShowFusionSuccessPanel();
    }

    void OnAttackButtonClick()
    {
        Debug.Log("Attack Button Clicked");
        int attackDifference = currentBeast_2.maxAttack - currentBeast_1.maxAttack;
        int maxN = CalculateMaxN(attackDifference, currentBeast_1.maxAttack);
        int randomAttackDifference = Random.Range(1, maxN + 1);
        Debug.Log(randomAttackDifference);
        UpdateFusionSuccessPanel();
        beastAttackText.text = "Attack " + currentBeast_1.maxAttack.ToString() + " -> " + (currentBeast_1.maxAttack + randomAttackDifference).ToString();
        currentBeast_1.maxAttack += randomAttackDifference;
      
        ShowFusionSuccessPanel();
    }


    void OnArmorButtonClick()
    {
        Debug.Log("Armor Button Clicked");
        int armorDifference = currentBeast_2.maxArmor - currentBeast_1.maxArmor;
        int maxN = CalculateMaxN(armorDifference, currentBeast_1.maxArmor);
        int randomArmorDifference = Random.Range(1, maxN + 1);
        Debug.Log(randomArmorDifference);
        UpdateFusionSuccessPanel();
        beastArmorText.text = "Armor " + currentBeast_1.maxArmor.ToString() + " -> " + (currentBeast_1.maxArmor + randomArmorDifference).ToString();
        currentBeast_1.maxArmor += randomArmorDifference;
       
        ShowFusionSuccessPanel();
    }

    void OnAPButtonClick()
    {
        Debug.Log("AP Button Clicked");
        int apDifference = currentBeast_2.maxAp - currentBeast_1.maxAp;
        int maxN = CalculateMaxN(apDifference, currentBeast_1.maxAp);
        int randomApDifference = Random.Range(1, maxN + 1);
        Debug.Log(randomApDifference);
        UpdateFusionSuccessPanel();
        beastAPText.text = "AP " + currentBeast_1.maxAp.ToString() + " -> " + (currentBeast_1.maxAp + randomApDifference).ToString();
        currentBeast_1.maxAp += randomApDifference;
   
        ShowFusionSuccessPanel();
    }

    void OnMRButtonClick()
    {
        Debug.Log("MR Button Clicked");
        int mrDifference = currentBeast_2.maxMr - currentBeast_1.maxMr;
        int maxN = CalculateMaxN(mrDifference, currentBeast_1.maxMr);
        int randomMrDifference = Random.Range(1, maxN + 1);
        Debug.Log(randomMrDifference);
        UpdateFusionSuccessPanel();
        beastMRText.text = "MR " + currentBeast_1.maxMr.ToString() + " -> " + (currentBeast_1.maxMr + randomMrDifference).ToString();
        currentBeast_1.maxMr += randomMrDifference;
   
        ShowFusionSuccessPanel();
    }

    void OnSpeedButtonClick()
    {
        Debug.Log("Speed Button Clicked");
        int speedDifference = currentBeast_2.maxSpeed - currentBeast_1.maxSpeed;
        int maxN = CalculateMaxN(speedDifference, currentBeast_1.maxSpeed);
        int randomSpeedDifference = Random.Range(1, maxN + 1);
        Debug.Log(randomSpeedDifference);
        UpdateFusionSuccessPanel();
        beastSpeedText.text = "Speed " + currentBeast_1.maxSpeed.ToString() + " -> " + (currentBeast_1.maxSpeed + randomSpeedDifference).ToString();
        currentBeast_1.maxSpeed += randomSpeedDifference;
   
        ShowFusionSuccessPanel();
    }

    private int CalculateMaxN(int difference, int currentBeast1MaxStat)
    {
        int maxN;
        if (currentBeast1MaxStat <= 100)
        {
            maxN = Mathf.Min(difference, 200);
        }
        else if (currentBeast1MaxStat <= 200)
        {
            maxN = Mathf.Min(difference, 150);
        }
        else if (currentBeast1MaxStat <= 300)
        {
            maxN = Mathf.Min(difference, 100);
        }
        else // currentBeast1MaxStat <= 350
        {
            maxN = Mathf.Min(difference, 30);
        }
        return maxN;
    }

    private void UpdateFusionSuccessPanel()
    {
        Debug.Log("update");
        beastNameText.text = currentBeast_1.name;
        beastImage.sprite = currentBeast_1.image;
        beastHPText.text = "HP " + currentBeast_1.maxHp.ToString();
        beastAttackText.text = "Attack " + currentBeast_1.maxAttack.ToString();
        beastArmorText.text = "Armor " + currentBeast_1.maxArmor.ToString();
        beastAPText.text = "AP " + currentBeast_1.maxAp.ToString();
        beastMRText.text = "MR " + currentBeast_1.maxMr.ToString();
        beastSpeedText.text = "Speed " + currentBeast_1.maxSpeed.ToString();
    }


    private void ShowFusionSuccessPanel()
    {
        fusionSuccessPanel.SetActive(true);
    }

    private void OnConfirmButtonClick()
    {
        // 检查 currentBeast_2 是否为 null
        if (currentBeast2Index != -1)
        {
            // 从背包中删除 currentBeast_2
            Debug.Log("Removing currentBeast_2 from bag: " + spiritBagManager.GetBeastAt(currentBeast2Index).name);
            spiritBagManager.RemoveBeastAt(currentBeast2Index);
        }
        else
        {
            Debug.LogError("currentBeast_2 index is invalid, cannot remove from bag");
        }

        // 更新 currentBeast_1 的数据后重置 FusionPanel
        if (currentBeast1Index != -1)
        {
            currentBeast_1 = spiritBagManager.GetBeastAt(currentBeast1Index); // 获取最新的 currentBeast_1 引用

            // 重置 FusionPanel
            ResetFusionPanel();
        }
        else
        {
            Debug.LogError("currentBeast_1 index is invalid, cannot update");
        }
        fusionSuccessPanel.SetActive(false);

    }
}
    