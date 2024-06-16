using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleSceneManager : MonoBehaviour
{
    public BattleSceneUI playerBeast;
    public BattleSceneUI enemyBeast;

    private void Start()
    {
        if (playerBeast == null)
        {
            Debug.LogError("playerBeastPanel is not assigned in the Inspector.");
        }
        if (enemyBeast == null)
        {
            Debug.LogError("enemyBeastPanel is not assigned in the Inspector.");
        }

        UpdateBattlePanels();
        StartCoroutine(TestBattle());
    }

    private void UpdateBattlePanels()
    {
        Debug.Log("updatepanel");
        if (BeastComponent.encounteredBeast != null)
        {
            enemyBeast.UpdateBeastInfo(BeastComponent.encounteredBeast, true);
        }

        if (BeastComponent.playerFirstBeast != null)
        {
            playerBeast.UpdateBeastInfo(BeastComponent.playerFirstBeast, false);
        }
    }

    private IEnumerator TestBattle()
    {
        Debug.Log("start fight");
        if (BeastComponent.playerFirstBeast != null && BeastComponent.encounteredBeast != null)
        {

            SpiritualBeast playerBeast = BeastComponent.playerFirstBeast;
            SpiritualBeast enemyBeast = BeastComponent.encounteredBeast;

            Debug.Log("Player Beast HP: " + playerBeast.currentHp);
            Debug.Log("Enemy Beast HP: " + enemyBeast.currentHp);
            Debug.Log("Player Beast Speed: " + playerBeast.currentSpeed);
            Debug.Log("Enemy Beast Speed: " + enemyBeast.currentSpeed);

            while (playerBeast.currentHp > 0 && enemyBeast.currentHp > 0)
            {
                
                Debug.Log("fighting");
                if (playerBeast.currentSpeed >= enemyBeast.currentSpeed)
                {
                    yield return new WaitForSeconds(3); // 等待 1 秒
                    // 玩家宠物先攻击敌人
                    if (playerBeast.currentAp >= 10)
                    {
                        enemyBeast.currentHp -=100;
                        playerBeast.currentAp -= 10;
                        Debug.Log("enemyBeast.currHP" + enemyBeast.currentHp);
                        Debug.Log("playerBeast.currentAp" + playerBeast.currentAp);
                    }
                    
                    UpdateBattlePanels();
                    yield return new WaitForSeconds(3); // 等待 1 秒

                    if (enemyBeast.currentHp <= 0)
                    {
                        Debug.Log("Enemy beast is dead. Battle stops.");
                        yield break; // 停止战斗
                    }

                    // 敌人攻击玩家宠物
                    if (enemyBeast.currentAp >= 10)
                    {
                        playerBeast.currentHp -=50;
                        enemyBeast.currentAp -= 10;
                        Debug.Log("enemyBeast.currentAp" + enemyBeast.currentAp);
                        Debug.Log("playerBeast.currentHp" + playerBeast.currentHp);
                    }
                    
                    UpdateBattlePanels();
                    yield return new WaitForSeconds(3); // 等待 1 秒


                    if (playerBeast.currentHp <= 0)
                    {
                        Debug.Log("Player's beast is dead. Battle stops.");
                        yield break; // 停止战斗
                    }
                }
                else
                {
                    // 敌人先攻击玩家宠物
                    if (enemyBeast.currentAp >= 10)
                    {
                        playerBeast.currentHp -=50;
                        enemyBeast.currentAp -= 10;
                        Debug.Log("enemyBeast.currentAp" + enemyBeast.currentAp);
                        Debug.Log("playerBeast.currentHp" + playerBeast.currentHp);
                    }
                    
                    UpdateBattlePanels();
                    yield return new WaitForSeconds(3); // 等待 1 秒

                    if (playerBeast.currentHp <= 0)
                    {
                        Debug.Log("Player's beast is dead. Battle stops.");
                        yield break; // 停止战斗
                    }

                    // 玩家宠物攻击敌人
                    if (playerBeast.currentAp >= 10)
                    {
                        enemyBeast.currentHp -=100;
                        playerBeast.currentAp -= 10;
                        Debug.Log("enemyBeast.currHP" + enemyBeast.currentHp);
                        Debug.Log("playerBeast.currentAp" + playerBeast.currentAp);
                    }
                    
                    UpdateBattlePanels();
                    yield return new WaitForSeconds(3); // 等待 1 秒

                    if (enemyBeast.currentHp <= 0)
                    {
                        Debug.Log("Enemy beast is dead. Battle stops.");
                        yield break; // 停止战斗
                    }
                }
            }
        }
    }

}
