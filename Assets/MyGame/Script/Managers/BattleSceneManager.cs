using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleSceneManager : MonoBehaviour
{
    // public BeastPanel playerBeastPanel;
    // public BeastPanel enemyBeastPanel;

    public TextMeshProUGUI enemyHpText;
    public TextMeshProUGUI enemyAttackText;
    public TextMeshProUGUI enemyArmorText;
    public TextMeshProUGUI enemyApText;
    public TextMeshProUGUI enemyMrText;
    public TextMeshProUGUI enemySpeedText;
    public Image enemyBeastImage;

    public Slider enemyHpSlider;

    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI playerAPText;
    public Image playerBeastImage;

    public Slider playerHpSlider;


    private void Start()
    {
        UpdateBattlePanels();
        TestBattle();
    }

    private void UpdateBattlePanels()
    {
        Debug.Log("updatepanel");
        if (BeastComponent.encounteredBeast != null)
        {
            UpdateBeastInfo(BeastComponent.encounteredBeast, true);
        }

        if (BeastComponent.playerFirstBeast != null)
        {
            UpdateBeastInfo(BeastComponent.playerFirstBeast, false);
        }
    }

    public void UpdateBeastInfo(SpiritualBeast beast, bool isEnemy)
    {
        if (isEnemy)
        {
            enemyHpSlider.maxValue = beast.maxHp;
            enemyHpSlider.value = beast.currentHp;
            if (beast.name.StartsWith("Spiritual"))
            {
                enemyHpText.text = $"HP: {beast.currentHp}/{beast.maxHp}";
                enemyAttackText.text = $"Attack: {beast.maxAttack}";
                enemyArmorText.text = $"Armor: {beast.maxArmor}";
                enemyApText.text = $"AP: {beast.currentAp}/{beast.maxAp}";
                enemyMrText.text = $"MR: {beast.maxMr}";
                enemySpeedText.text = $"Speed: {beast.maxSpeed}";
                enemyBeastImage.sprite = beast.image;
            }
            else if (beast.name.StartsWith("Normal"))
            {
                enemyHpText.text = $"HP: {beast.currentHp}/{beast.maxHp}";
                enemyApText.text = $"AP: {beast.currentAp}/{beast.maxAp}";
                enemyAttackText.text = "";
                enemyArmorText.text = "";
                enemyMrText.text = "";
                enemySpeedText.text = "";
                enemyBeastImage.sprite = beast.image;
            }
        }
        else
        {
            playerHPText.text = $"HP: {beast.currentHp}/{beast.maxHp}";
            playerAPText.text = $"AP: {beast.currentAp}/{beast.maxAp}";
            playerBeastImage.sprite = beast.image;

            playerHpSlider.maxValue = beast.maxHp;
            playerHpSlider.value = beast.currentHp;
        }
    }

    private void TestBattle()
    {
        if (BeastComponent.playerFirstBeast != null && BeastComponent.encounteredBeast != null)
        {
            // 玩家宠物攻击敌人，敌人掉50滴血
            BeastComponent.encounteredBeast.currentHp -= 50;
            if (BeastComponent.encounteredBeast.currentHp < 0)
            {
                BeastComponent.encounteredBeast.currentHp = 0;
            }
            UpdateBeastInfo(BeastComponent.encounteredBeast, true);

            // 检查敌人是否死亡
            if (BeastComponent.encounteredBeast.currentHp == 0)
            {
                Debug.Log("Enemy beast is dead. Battle stops.");
                return; // 停止战斗
            }

            // 敌人攻击玩家宠物，玩家宠物掉20滴血
            BeastComponent.playerFirstBeast.currentHp -= 20;
            if (BeastComponent.playerFirstBeast.currentHp < 0)
            {
                BeastComponent.playerFirstBeast.currentHp = 0;
            }
            UpdateBeastInfo(BeastComponent.playerFirstBeast, false);

            // 检查玩家宠物是否死亡
            if (BeastComponent.playerFirstBeast.currentHp == 0)
            {
                Debug.Log("Player's beast is dead. Battle stops.");
                return; // 停止战斗
            }
        }
    }
}
