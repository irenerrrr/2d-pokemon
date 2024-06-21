using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleSceneUI : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI apText;
    public TextMeshProUGUI mrText;
    public TextMeshProUGUI speedText;
    public Image beastImage;

    public Slider hpSlider;
    public Slider apSlider;
    public TextMeshProUGUI hpSliderText;  // 新增，用于显示最大血量/当前血量
    public TextMeshProUGUI apSliderText;  // 新增，用于显示最大AP/当前AP

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    public GameObject enemyInfoPanel;


    void Start()
    {
        hpSlider.interactable = false;
        apSlider.interactable = false;
    }

    public void UpdateBeastInfo(SpiritualBeast beast, bool isEnemy)
    {
        if (beast == null)
        {
            Debug.LogError("Beast is null");
            return;
        }


        UpdateSlider(hpSlider, hpSliderText, beast.maxHp, beast.currentHp);
        UpdateSlider(apSlider, apSliderText, beast.maxAp, beast.currentAp);

        beastImage.sprite = beast.image;
        UpdateText(nameText, beast.name);
        UpdateText(levelText, $"Level {beast.level}");

        if (isEnemy && beast.type == "SpiritualBeast")
        {
            enemyInfoPanel.SetActive(true);
            UpdateText(hpText, $"HP {beast.maxHp}");
            UpdateText(apText, $"AP {beast.maxAp}");
            UpdateText(attackText, $"Attack {beast.maxAttack}");
            UpdateText(armorText, $"Armor {beast.maxArmor}");
            UpdateText(mrText, $"MR {beast.maxMr}");
            UpdateText(speedText, $"Speed {beast.maxSpeed}");
        }
        else
        {
            if (enemyInfoPanel != null)
            {
                enemyInfoPanel.SetActive(false);
            }

        }
    }

    private void UpdateSlider(Slider slider, TextMeshProUGUI sliderText, float maxValue, float currentValue)
    {
        slider.maxValue = maxValue;
        slider.value = currentValue;
        sliderText.text = $"{maxValue} / {currentValue}";
    
    }

    
    
    private void UpdateText(TextMeshProUGUI textComponent, string content)
    {
        if (textComponent != null)
        {
            textComponent.text = content;
            textComponent.gameObject.SetActive(true);
        }
    }
}
