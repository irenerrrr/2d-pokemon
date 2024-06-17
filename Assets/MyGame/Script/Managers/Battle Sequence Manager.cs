using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSequenceManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject dropdownPanel; // 下拉面板
    public Button dropdownButton; // 下拉按钮

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;

    void Start()
    {
        dropdownPanel.SetActive(false); // 初始状态隐藏下拉面板

        // 添加按钮点击事件
        dropdownButton.onClick.AddListener(ToggleDropdownPanel);

        button1.onClick.AddListener(() => OnButtonClick(1));
        button2.onClick.AddListener(() => OnButtonClick(2));
        button3.onClick.AddListener(() => OnButtonClick(3));
        button4.onClick.AddListener(() => OnButtonClick(4));
        button5.onClick.AddListener(() => OnButtonClick(5));
        button6.onClick.AddListener(() => OnButtonClick(6));
    }

    void ToggleDropdownPanel()
    {
        dropdownPanel.SetActive(!dropdownPanel.activeSelf); // 切换面板显示状态
    }

    void OnButtonClick(int buttonNumber)
    {
        Debug.Log("Button #" + buttonNumber + " clicked");
        // 在这里添加每个按钮的具体功能
    }
}
