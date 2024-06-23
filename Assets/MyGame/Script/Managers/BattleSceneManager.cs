using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleSceneManager : MonoBehaviour
{
    public BattleSceneUI playerBeast;
    public BattleSceneUI enemyBeast;

    public BeastManager beastManager;
    public BattleSequenceManager battleSequenceManager;
    public SpiritBagManager spiritBagManager;

    private const int MaxBattleCount = 5; 
    private int battleCount = 0;

    private List<SpiritualBeast> participatedBeasts = new List<SpiritualBeast>();


    private void Start()
    {
        InitializeBattle(); // 初始化战斗
    }

    private void InitializeBattle()
    {
        // 只在这里设置 encounteredBeast
        enemyBeast.UpdateBeastInfo(BeastComponent.encounteredBeast, true);

        // 在这里决定玩家的出战宠物
        BeastComponent.playerFirstBeast = GetNextValidPlayerBeast(ref battleCount);
        if (BeastComponent.playerFirstBeast != null)
        {
            playerBeast.UpdateBeastInfo(BeastComponent.playerFirstBeast, false);
            Debug.Log("playerFirstBeast: " + BeastComponent.playerFirstBeast.name);
            StartCoroutine(TestBattle());
        }
        else
        {
            Debug.Log("spiritBagManager或GetFirstBeast()返回空");
        }
    }

    private void UpdateBattlePanels()
    {
        // 更新敌人和玩家宠物的信息
        enemyBeast.UpdateBeastInfo(BeastComponent.encounteredBeast, true);
        playerBeast.UpdateBeastInfo(BeastComponent.playerFirstBeast, false); // 使用 BeastComponent.playerFirstBeast
    }

    private IEnumerator TestBattle()
    {
        Debug.Log("start fight");
        if (BeastComponent.encounteredBeast != null)
        {
            SpiritualBeast enemyBeast = BeastComponent.encounteredBeast;
            SpiritualBeast playerBeast = BeastComponent.playerFirstBeast;

            while (playerBeast != null && enemyBeast.currentHp > 0 && battleCount <= MaxBattleCount)
            {
          
                SpiritualBeast firstMove;
                SpiritualBeast secondMove;

                if (playerBeast.currentSpeed >= enemyBeast.currentSpeed)
                {
                    firstMove = playerBeast;
                    secondMove = enemyBeast;
                }
                else
                {
                    firstMove = enemyBeast;
                    secondMove = playerBeast;
                }

                while (playerBeast.currentHp > 0 && enemyBeast.currentHp > 0)
                {
                    Debug.Log("fighting");

                    // First move attack
                    yield return new WaitForSeconds(3); // 等待 3 秒
                    if (firstMove.currentAp >= 10)
                    {
                        if (firstMove == playerBeast)
                        {
                            enemyBeast.currentHp -= 300;
                        }
                        else
                        {
                            playerBeast.currentHp -= 10;
                        }
                        firstMove.currentAp -= 10;
                    }

                    UpdateBattlePanels();
                    yield return new WaitForSeconds(3); // 等待 3 秒

                    if (enemyBeast.currentHp <= 0 || playerBeast.currentHp <= 0)
                    {
                        break; // 停止战斗
                    }

                    // Swap first and second move
                    SpiritualBeast temp = firstMove;
                    firstMove = secondMove;
                    secondMove = temp;

                    // Second move attack (which is now the first move due to the swap)
                    yield return new WaitForSeconds(3); // 等待 3 秒
                    if (firstMove.currentAp >= 10)
                    {
                        if (firstMove == playerBeast)
                        {
                            enemyBeast.currentHp -= 300;
                        }
                        else
                        {
                            playerBeast.currentHp -= 10;
                        }
                        firstMove.currentAp -= 10;
                    }

                    UpdateBattlePanels();
                    yield return new WaitForSeconds(3); // 等待 3 秒

                    if (enemyBeast.currentHp <= 0 || playerBeast.currentHp <= 0)
                    {
                        break; // 停止战斗
                    }

                    // Swap back
                    temp = firstMove;
                    firstMove = secondMove;
                    secondMove = temp;
                }

                if (enemyBeast.currentHp <= 0)
                {
                    EndBattle();
                }
                else if (playerBeast.currentHp <= 0)
                {
                    playerBeast = GetNextValidPlayerBeast(ref battleCount);
                    BeastComponent.playerFirstBeast = playerBeast; // 更新 BeastComponent.playerFirstBeast
                    if (playerBeast != null)
                    {
                        UpdateBattlePanels(); // 刷新 UI 面板
                    }
                }
            }

            if (playerBeast == null || battleCount > MaxBattleCount)
            {
                EndBattle();
                Debug.Log("All player beasts are dead. Battle over.");
            }
        }

        yield return null;
    }


    private SpiritualBeast GetNextValidPlayerBeast(ref int battleCount)
    {
        Debug.Log("next" + battleCount);
        battleCount++;
        foreach (var beast in BeastManager.sequenceList)
        {
            if (beast != null && beast.currentHp > 0)
            {
                participatedBeasts.Add(beast);
                return beast;
            }
        }

        foreach (var beast in BeastManager.beasts)
        {
            if (beast != null && beast.currentHp > 0)
            {
                participatedBeasts.Add(beast);
                return beast;
            }
        }

        return null; // 如果没有有效的beast
    }



    private void EndBattle()
    {
        if (BeastComponent.playerFirstBeast.currentHp <= 0)
        {
            Debug.Log("Player lost the battle.");
        }
        else if (BeastComponent.encounteredBeast.currentHp <= 0)
        {
            Debug.Log("Enemy lost the battle.");

        }
        Debug.Log("调用 PlayerController 的 EndBattle 方法.");
        addExp();
        DecreaseIntimacyForParticipatedBeasts();
        PlayerController.Instance.EndBattle();
    }

    private void DecreaseIntimacyForParticipatedBeasts()
    {
        foreach (var beast in participatedBeasts)
        {
            beast.DecreaseIntimacy(3);
            Debug.Log(beast.name + "的亲密度减少了3");
           
        }
        participatedBeasts.Clear(); // 清空参与战斗的宠物列表
    }

    private void addExp()
    {

    }

}
