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

    public Image hpFillImage; // Fill Image for HP slider
    public Image apFillImage; // Fill Image for AP slider

    public GameObject enemyInfoPanel;


    public void UpdateBeastInfo(SpiritualBeast beast, bool isEnemy)
    {
        if (beast == null)
        {
            Debug.LogError("Beast is null");
            return;
        }


        UpdateSlider(hpSlider, hpSliderText, beast.maxHp, beast.currentHp, hpFillImage, Color.red);
        UpdateSlider(apSlider, apSliderText, beast.maxAp, beast.currentAp, apFillImage, Color.blue);

        beastImage.sprite = beast.image;
        UpdateText(nameText, beast.name);

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

    private void UpdateSlider(Slider slider, TextMeshProUGUI sliderText, float maxValue, float currentValue, Image fillImage, Color fillColor)
    {
        slider.maxValue = maxValue;
        slider.value = currentValue;
        sliderText.text = $"{maxValue} / {currentValue}";
        fillImage.color = fillColor;
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
