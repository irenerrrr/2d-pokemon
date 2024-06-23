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


    private void Start()
    {
        UpdateBattlePanels();
        StartCoroutine(TestBattle());
    }

    private void UpdateBattlePanels()
    {
        enemyBeast.UpdateBeastInfo(BeastComponent.encounteredBeast, true);
        playerBeast.UpdateBeastInfo(BeastComponent.playerFirstBeast, false);
    }

    private IEnumerator TestBattle()
    {
        Debug.Log("start fight");
        if (BeastComponent.encounteredBeast != null)
        {
            SpiritualBeast enemyBeast = BeastComponent.encounteredBeast;
            
            SpiritualBeast playerBeast = GetNextValidPlayerBeast(ref battleCount);

            while (playerBeast != null && enemyBeast.currentHp > 0 && battleCount <= MaxBattleCount)
            {

                playerBeast.participatedInBattle = true;
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
                            enemyBeast.currentHp -= 200;
                            playerBeast.currentAp -= 10;
                            Debug.Log("enemyBeast.currHP: " + enemyBeast.currentHp);
                            Debug.Log("playerBeast.currentAp: " + playerBeast.currentAp);
                        }
                        else
                        {
                            playerBeast.currentHp -= 50;
                            enemyBeast.currentAp -= 10;
                            Debug.Log("enemyBeast.currentAp: " + enemyBeast.currentAp);
                            Debug.Log("playerBeast.currentHp: " + playerBeast.currentHp);
                        }
                    }
                    
                    UpdateBattlePanels();
                    yield return new WaitForSeconds(3); // 等待 3 秒

                    if (enemyBeast.currentHp <= 0 || playerBeast.currentHp <= 0)
                    {
                        Debug.Log("One of the beasts is dead. Battle stops.");
                        break; // 停止战斗
                    }

                    // Second move attack
                    yield return new WaitForSeconds(3); // 等待 3 秒
                    if (secondMove.currentAp >= 10)
                    {
                        if (secondMove == playerBeast)
                        {
                            enemyBeast.currentHp -= 200;
                            playerBeast.currentAp -= 10;
                            Debug.Log("enemyBeast.currHP: " + enemyBeast.currentHp);
                            Debug.Log("playerBeast.currentAp: " + playerBeast.currentAp);
                        }
                        else
                        {
                            playerBeast.currentHp -= 50;
                            enemyBeast.currentAp -= 10;
                            Debug.Log("enemyBeast.currentAp: " + enemyBeast.currentAp);
                            Debug.Log("playerBeast.currentHp: " + playerBeast.currentHp);
                        }
                    }
                    
                    UpdateBattlePanels();
                    yield return new WaitForSeconds(3); // 等待 3 秒

                    if (enemyBeast.currentHp <= 0 || playerBeast.currentHp <= 0)
                    {
                        Debug.Log("One of the beasts is dead. Battle stops.");
                        break; // 停止战斗
                    }
                }

                if (enemyBeast.currentHp <= 0)
                {
                    Debug.Log("Enemy beast defeated.");
                    break;
                }
                else if (playerBeast.currentHp <= 0)
                {
                    Debug.Log("Player beast defeated. Switching to the next one.");
                    playerBeast = GetNextValidPlayerBeast(ref battleCount);
                }
            }

            if (playerBeast == null || battleCount > MaxBattleCount)
            {
                Debug.Log("All player beasts are dead. Battle over.");
            }
        }

        yield return null;
    }

    private SpiritualBeast GetNextValidPlayerBeast(ref int battleCount)
    {
        battleCount++;
        foreach (var beast in BattleSequenceManager.sequenceList)
        {
            if (beast != null && beast.currentHp > 0)
            {
                return beast;
            }
        }

        foreach (var beast in SpiritBagManager.beasts)
        {
            if (beast != null && beast.currentHp > 0)
            {
                return beast;
            }
        }

        return null; // 如果没有有效的beast
    }


    private void Update()
    {
        CheckBattleStatus();
    }

    private void CheckBattleStatus()
    {
        if (BeastComponent.playerFirstBeast.currentHp <= 0 || BeastComponent.encounteredBeast.currentHp <= 0)
        {
            EndBattle();
        }
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
        if (BeastComponent.playerFirstBeast != null && BeastComponent.playerFirstBeast.participatedInBattle)
        {
            BeastComponent.playerFirstBeast.DecreaseIntimacy(3);
            BeastComponent.playerFirstBeast.participatedInBattle = false; // 重置参战标记
            Debug.Log(BeastComponent.playerFirstBeast.name + "的亲密度减少了3");
        }
    }

    private void addExp()
    {

    }

}
