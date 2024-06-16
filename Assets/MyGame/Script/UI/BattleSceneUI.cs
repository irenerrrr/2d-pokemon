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

    public Image hpFillImage; // Fill Image for HP slider
    public Image apFillImage; // Fill Image for AP slider


    public void UpdateBeastInfo(SpiritualBeast beast, bool isEnemy)
    {
        if (beast == null)
        {
            Debug.LogError("Beast is null");
            return;
        }

        hpSlider.maxValue = beast.maxHp;
        hpSlider.value = beast.currentHp;
        hpSliderText.text = $"{beast.maxHp} / {beast.currentHp}";

        apSlider.maxValue = beast.maxAp;
        apSlider.value = beast.currentAp;
        apSliderText.text = $"{beast.maxAp} / {beast.currentAp}";

        hpFillImage.color = Color.red; // 将 Fill Image 的颜色设置为红色
        apFillImage.color = Color.blue; // 可选：将 AP Fill Image 的颜色设置为蓝色


        beastImage.sprite = beast.image;

 
        if (isEnemy)
        {
            if (beast.type == "SpiritualBeast")
            {
                hpText.text = $"HP: {beast.maxHp}";
                apText.text = $"Mana: {beast.maxAp}";
                attackText.text = $"Attack: {beast.maxAttack}";
                armorText.text = $"Armor: {beast.maxArmor}";
                mrText.text = $"MR: {beast.maxMr}";
                speedText.text = $"Speed: {beast.maxSpeed}";

                hpText.gameObject.SetActive(true);
                apText.gameObject.SetActive(true);   
                attackText.gameObject.SetActive(true);
                armorText.gameObject.SetActive(true);
                mrText.gameObject.SetActive(true);
                speedText.gameObject.SetActive(true);
            }
            else
            {
                hpText.gameObject.SetActive(false);
                apText.gameObject.SetActive(false);  
                attackText.gameObject.SetActive(false);
                armorText.gameObject.SetActive(false);
                mrText.gameObject.SetActive(false);
                speedText.gameObject.SetActive(false);
            }
        }
        else
        {
            UpdateText(hpText, $"HP: {beast.maxHp}");
            UpdateText(apText, $"Mana: {beast.maxAp}");
            UpdateText(attackText, $"Attack: {beast.maxAttack}");
            UpdateText(armorText, $"Armor: {beast.maxArmor}");
            UpdateText(mrText, $"MR: {beast.maxMr}");
            UpdateText(speedText, $"Speed: {beast.maxSpeed}");
        }
   
        
    
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
