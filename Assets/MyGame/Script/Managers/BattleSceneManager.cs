using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq; 

public class BattleSceneManager : MonoBehaviour
{
    public BattleSceneUI playerBeast;
    public BattleSceneUI enemyBeast;

    public BeastManager beastManager;
    public BattleSequenceManager battleSequenceManager;
    public SpiritBagManager spiritBagManager;

    private int battleCount = 0; // 当前beast的索引
    private int count;
    private List<SpiritualBeast> normal = new List<SpiritualBeast>(); 
    private List<SpiritualBeast> targetList;

    private List<SpiritualBeast> participatedBeasts = new List<SpiritualBeast>();


    private void Start()
    {
        CheckBeastStatusInSequenceList();
        InitializeBattle(); // 初始化战斗
        spiritBagManager = FindObjectOfType<SpiritBagManager>();
      
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
            Debug.Log("No beast can fight");
            EndBattle();
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

            while (playerBeast != null && enemyBeast.currentHp > 0 && battleCount <= 5)
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
                    Debug.Log("Current player beast has died. Attempting to switch.");
                    playerBeast = GetNextValidPlayerBeast(ref battleCount);
                    if (playerBeast == null)
                    {
                        Debug.Log("No valid player beast left. Ending battle.");
                        EndBattle(); // 如果没有有效的 Beast，结束战斗
                    }
                    else
                    {
                        BeastComponent.playerFirstBeast = playerBeast;
                        UpdateBattlePanels(); // 刷新 UI 面板
                        Debug.Log($"Continuing battle with new player beast: {playerBeast.name}");
                    }
                }
            }

            if (BeastComponent.playerFirstBeast == null || participatedBeasts.Count == 5)
            {
                EndBattle();
                Debug.Log("All player beasts are dead. Battle over.");
            }
        }

        yield return null;
    }

    

    private SpiritualBeast GetNextValidPlayerBeast(ref int battleCount)
    {
        battleCount++;
     
        foreach (var beast in BeastManager.sequenceList)
        {
            if (beast != null && beast.currentHp > 0 && beast.intimacy >= 3)
            {
                participatedBeasts.Add(beast);
                return beast;
            }
        }
        foreach (var beast in BeastManager.beasts)
        {
            if (beast != null && beast.currentHp > 0 && beast.intimacy >= 3)
            {
                participatedBeasts.Add(beast);
                return beast;
            }
        }
        Debug.Log("No more beasts available to fight.");
        return null; // 如果没有更多的beast，返回null
    }

 

    private void EndBattle()
    {

        if (BeastComponent.encounteredBeast.currentHp <= 0)
        {
            Debug.Log("Enemy lost the battle.");
            addExp();
            PlayerController.Instance.EndBattle(true);
        }
        else
        {
            Debug.Log("Player lost the battle.");
            PlayerController.Instance.EndBattle(false);
        }
        DecreaseIntimacyForParticipatedBeasts();
        Debug.Log("调用 PlayerController 的 EndBattle 方法.");
        participatedBeasts.Clear(); // 清空参与战斗的宠物列表
    }


    private void DecreaseIntimacyForParticipatedBeasts()
    {
        foreach (var beast in participatedBeasts)
        {
            if (beast.currentHp <= 0) 
            {
                beast.DecreaseIntimacy(20);
                Debug.Log(beast.name + "的亲密度减少了20");
            }
            else
            {
                beast.DecreaseIntimacy(3);
                Debug.Log(beast.name + "的亲密度减少了3");
            }
           
        }
    }

    private void CheckBeastStatusInSequenceList()
    {
        normal.Clear(); // 清除列表，以便重新填充

        for (int i = 0; i < BeastManager.sequenceList.Count; i++) // 使用 for 循环以便能够获取索引
        {
            var beast = BeastManager.sequenceList[i];
            if (beast != null && beast.currentHp > 0 && beast.intimacy >= 3)
            {
                normal.Add(beast); // 直接添加符合条件的 beast
            }
        }

        // 打印所有活跃的 beast 的索引
        Debug.Log("Active beasts at indices: " + string.Join(", ", normal));
    }

    private void addExp()
    {
        int battleCount = BeastManager.sequenceList.Count(beast => beast != null);
        int participatedCount = participatedBeasts.Count;

        //如果开战前的出战列表里一个没死，对比实际出战数多还是出战列表里的beast多
        if (normal.Count == battleCount)
        {
            targetList = participatedCount > battleCount ? participatedBeasts : BeastManager.sequenceList;
            count = Mathf.Max(participatedCount, battleCount); // 选择较大的数量
        }
        //如果开战前出战列表里已经有嗝屁了，但是出战列表的数量还是比实际出战多
        else if (battleCount > normal.Count && normal.Count >= participatedCount)
        {
            targetList = normal;
            count = normal.Count;
        }
        //开战前出战列表已经有嗝屁了，而且出战列表的数量没实际出战多
        else 
        {
            targetList = participatedBeasts;
            count = participatedBeasts.Count;
        }


        // 调用 CalculateExpToNextLevel 确保 expToNextLevel 是最新的
        BeastComponent.encounteredBeast.CalculateExpToNextLevel(); 
        int nextLevelExp = BeastComponent.encounteredBeast.expToNextLevel; // 获取下一级所需经验
        Debug.Log("野怪的经验: " + nextLevelExp);
        float expCanGet = nextLevelExp * 0.6f;  // 首发获得的总经验

        // 定义不同怪物数量下的经验分配比例
        float[][] expRatios = new float[][]
        {
            new float[] {1.0f},  // 一只时，全部经验给首发
            new float[] {0.65f, 0.35f},  // 两只时的分配比例
            new float[] {0.55f, 0.45f, 0.20f},
            new float[] {0.45f, 0.35f, 0.25f, 0.15f},
            new float[] {0.35f, 0.30f, 0.25f, 0.15f, 0.15f}
        };

        // 计算并分配经验
        if (count > 0 && count <= expRatios.Length)  // 确保有参战怪物并且比例数组包含该数量的怪物
        {
            float[] ratios = expRatios[count - 1];  // 获取当前怪物数量对应的经验分配比例
            for (int i = 0; i < count; i++)
            {
                int expToGive = Mathf.FloorToInt(expCanGet * ratios[i]);  // 计算每只怪物的经验
                targetList[i].GainExp(expToGive);  // 分配经验

            }
        }
 
    }


}
