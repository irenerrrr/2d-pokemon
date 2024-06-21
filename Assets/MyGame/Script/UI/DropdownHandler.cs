using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DropdownHandler : MonoBehaviour

{
    [SerializeField] private TMP_Dropdown dropdown;
    public SpiritBagManager spiritBagManager;

    public static List<SpiritualBeast> sequenceList = new List<SpiritualBeast> { null, null, null, null, null, null };
    // private SpiritualBeast[] beasts;
    private SpiritualBeast currentBeast;
  

    void Start()
    {
        List<string> options = new List<string> {"Battle", "#1", "#2", "#3", "#4", "#5" };
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        if (spiritBagManager != null)
        {
            // 注册事件，确保只有在数据发生变化时才进行更新
            Debug.Log("update");
            spiritBagManager.BeastSelected += UpdateDropdownDisplay;
            // spiritBagManager.UpdateDropdownEvent += UpdateDropdownDisplay; // 订阅事件
            // Update();
        }
        UpdateDropdownDisplay(null);
    }

    void OnDestroy()
    {
        if (spiritBagManager != null)
        {
            // 注销事件，防止内存泄漏
            spiritBagManager.BeastSelected -= UpdateDropdownDisplay;
            // spiritBagManager.UpdateDropdownEvent -= UpdateDropdownDisplay; // 取消订阅
        }
    }

    public void GetDropDownValue()
    {
        int pickedValue = dropdown.value - 1; // 补偿因为"Battle"选项带来的偏移
        if (pickedValue < 0) pickedValue = -1; // 如果选择了"Battle"，则设置为-1

        SpiritualBeast selectedBeast = spiritBagManager.GetSelectedBeast();
        int currSequence = selectedBeast.battleSequence;

        if (currSequence != pickedValue)
        {
            Check(currSequence, pickedValue, selectedBeast);
            spiritBagManager.UpdateBeastInfo(selectedBeast);
            UpdateDropdownDisplay(selectedBeast);
        }
    }


    
    public void UpdateDropdownDisplay(SpiritualBeast selectedBeast)
    {
        if (selectedBeast == null || selectedBeast.battleSequence == -1)
        {
            dropdown.captionText.text = "Battle";
            dropdown.value = 0; // 确保Dropdown的值与显示文本一致
        }
        else
        {
            dropdown.captionText.text = "#" + selectedBeast.battleSequence;
            dropdown.value = selectedBeast.battleSequence + 1; // 由于"Battle"占据了第一个选项，序列值需要加1
        }

        dropdown.RefreshShownValue();
    }

    //selectedindex = beastindex
    public void Check(int currentSequence, int TargetSequence, SpiritualBeast currentBeast) 
    {
  
        // 如果当前位置或目标位置有一个是空的，跳转到CheckBackward
        if (currentSequence == -1 || sequenceList[TargetSequence] == null) 
        {
            Debug.Log("One of the positions is empty, jumping to CheckBackward");
            CheckBackward(currentSequence, TargetSequence, currentBeast, false);
            return;
        }
        // 位置调整
        if (currentSequence > TargetSequence)
        {
            Debug.Log($"Moving beast from sequence {currentSequence} to {TargetSequence}");
            
            // 把其他位置依次后移
            for (int i = currentSequence; i > TargetSequence; i--)
            {
                sequenceList[i] = sequenceList[i - 1];
                if (sequenceList[i] != null)
                {
                    sequenceList[i].battleSequence = i;
                    Debug.Log($"Moved beast from sequence {i - 1} to {i}");
                }
            }
            
            // 把当前元素放到目标位置
            sequenceList[TargetSequence] = currentBeast;
            currentBeast.battleSequence = TargetSequence;
            Debug.Log($"Placed beast at sequence {TargetSequence}");
        }
        // 位置互换
        else if (currentSequence < TargetSequence)
        {
            Debug.Log($"Swapping beasts at sequence {currentSequence} and {TargetSequence}");
            
            SpiritualBeast tempBeast = sequenceList[TargetSequence];
            sequenceList[TargetSequence] = currentBeast;
            sequenceList[TargetSequence].battleSequence = TargetSequence;

            sequenceList[currentSequence] = tempBeast;
            sequenceList[currentSequence].battleSequence = currentSequence;

            Debug.Log($"Swapped beasts at sequence {currentSequence} and {currentSequence}");
        }

    }

    //if already checked checkbackward, hasCheckedBackward = true
    public void CheckBackward(int currentSequence, int targetSequence, SpiritualBeast currentBeast, bool hasCheckedBackward)
    {
        Debug.Log("target " + targetSequence + "curr " + currentSequence);
        //如果目标位置是6，则将当前精灵兽的序列设为-1，并返回
        if (targetSequence == 5 && hasCheckedBackward)
        {
            currentBeast.battleSequence = -1;
            Debug.Log("backward1 Changed sequence: " + currentBeast.battleSequence);
            return;
        }

        // 如果目标位置不为空，把新的替换原来的，然后拿原来的去找他后面的同学
        if (sequenceList[targetSequence] != null)
        {
            // 获取下一个位置的精灵兽
            SpiritualBeast nextBeast = sequenceList[targetSequence];

            // 将当前精灵兽移到目标位置
            sequenceList[targetSequence] = currentBeast;
            currentBeast.battleSequence = targetSequence;
            Debug.Log("backward2 Changed sequence: " + currentBeast.battleSequence);

            // 递归检查下一个位置
            CheckBackward(targetSequence , targetSequence + 1, nextBeast, true);
        }
        // 如果目标位置为空，并且已经进行了后退检查
        else if (sequenceList[targetSequence] == null && hasCheckedBackward)
        {
            sequenceList[targetSequence] = currentBeast;
            currentBeast.battleSequence = targetSequence;
            Debug.Log("backward3 Changed sequence: " + currentBeast.battleSequence);
        }
        // 如果目标位置为空，调用CheckForward方法
        else
        {
            CheckForward(currentSequence, targetSequence, currentBeast);
            Debug.Log("Checkforward");
        }
    }

    public void CheckForward(int currentSequence, int targetSequence, SpiritualBeast currentBeast) 
    {
        Debug.Log($"CheckForward called with selectedIndex: {currentSequence}, targetSequence: {targetSequence}");

        // 从当前序列开始向前检查，确保前一个位置不为空
        for (int i = targetSequence; i > currentSequence; i--) 
        {
            if (i == 0 || sequenceList[i - 1] != null) 
            {
                Debug.Log($"sequenceList[{i - 1}] is { (i == 0 ? "checked as first position" : "not null") }, assigning beast to sequenceList[{i}]");

                // 把当前精灵兽放到这个位置
                currentBeast.battleSequence = i;
                sequenceList[i] = currentBeast;

                Debug.Log("forward Changed sequence: " + i);
                return;
            }
            else 
            {
                Debug.Log($"sequenceList[{i - 1}] is null, continuing to check previous positions");
            }
        }
    }



}