using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入UI命名空间来操作Dropdown
using TMPro; // 如果使用TextMeshPro的Dropdown，引入此命名空间

public class LanguageManager : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // 用于TextMeshPro的Dropdown，如果是普通UI，使用public Dropdown languageDropdown

    public TMP_FontAsset englishFont;
    public TMP_FontAsset chineseFont;
    private List<TMP_Text> textComponents = new List<TMP_Text>();

    void Start()
    {
        InitializeLanguageDropdown();
        FindAllTextComponentsInCanvas();
    }

    // 初始化Dropdown列表
    void InitializeLanguageDropdown()
    {
        languageDropdown.options.Clear(); // 清除现有选项
        languageDropdown.options.Add(new TMP_Dropdown.OptionData("English"));
        languageDropdown.options.Add(new TMP_Dropdown.OptionData("中文"));

        // 设置默认语言为列表中的第一项
        languageDropdown.value = 0;

        // 添加监听事件
        languageDropdown.onValueChanged.AddListener(delegate { LanguageChanged(languageDropdown.value); });
    }

    // 查找并注册所有 TMP_Text 组件
    void FindAllTextComponentsInCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>(); // 获取当前组件的 Canvas，或者直接指定 Canvas
        if (canvas == null) {
            Debug.LogError("No Canvas found in parent hierarchy.");
            return;
        }

        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(canvas.transform);
        while (queue.Count > 0) {
            Transform current = queue.Dequeue();
            TMP_Text textComponent = current.GetComponent<TMP_Text>();
            if (textComponent != null) {
                textComponents.Add(textComponent);
            }
            foreach (Transform child in current) {
                queue.Enqueue(child);
            }
        }
    }


    // 根据下拉菜单的选择更新语言和字体
    void LanguageChanged(int index)
    {
        TMP_FontAsset selectedFont = index == 1 ? chineseFont : englishFont;
        ChangeFont(selectedFont);
    }

    // 更新所有文本组件的字体
    void ChangeFont(TMP_FontAsset newFont)
    {
        foreach (TMP_Text text in textComponents)
        {
            text.font = newFont;
            text.ForceMeshUpdate();
        }
    }
}
