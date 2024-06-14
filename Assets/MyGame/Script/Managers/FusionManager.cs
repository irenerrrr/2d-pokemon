using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{
    public GameObject fusionPanel; // 拖动你的Fusion Panel到此字段
    public GameObject spiritPanel;

    void Start()
    {
        // 确保Fusion Panel一开始是隐藏的
        fusionPanel.SetActive(false);
    }

    public void ShowFusionPanel()
    {
        fusionPanel.SetActive(true);
        spiritPanel.SetActive(true);
    }

    public void HideFusionPanel()
    {
        fusionPanel.SetActive(false);
        spiritPanel.SetActive(false);
    }
}
