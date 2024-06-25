using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private GameObject spiritBagCanvas;

    public GameObject beastBagButton;
    public GameObject spiritPanel;
    public GameObject spiritDetail;
    public GameObject ButtonsPanel;
    public GameObject inductor;
    
    public SpiritBagManager spiritbagManager;

    
  

    private void Awake()
    {
        GameObject canvas = GameObject.Find("UI/Canvas");
        spiritBagCanvas = canvas.transform.Find("SpiritBagCanvas").gameObject;
        inductor = canvas.transform.Find("inductor").gameObject;
        ButtonsPanel = inductor.transform.Find("ButtonsPanel").gameObject;
       
        spiritPanel = spiritBagCanvas.transform.Find("SpiritInfoPanel").gameObject;
        spiritDetail = spiritBagCanvas.transform.Find("InfoPanel").gameObject;


        
        // 初始时隐藏面板
        spiritPanel.SetActive(false);
        spiritDetail.SetActive(false);


    }

    // 初始时隐藏 Panel
    void Start()
    {

    }


    //abandon button
    public void OnAbandonButtonClicked()
    {
        spiritbagManager.RemoveSelectedBeast();
    }

    // 切换 SpiritPanel 的显示状态
    public void ToggleSpiritPanel()
    {

        bool isSpiritPanelActive = spiritPanel.activeSelf;
        bool isSpiritDetailActive = spiritDetail.activeSelf;

        if (!isSpiritPanelActive && !isSpiritDetailActive)
        {
            // 如果两个面板都没有显示，显示 spiritPanel
            spiritPanel.SetActive(true);
        }
        else
        {
            // 否则，关闭所有面板
            spiritPanel.SetActive(false);
            spiritDetail.SetActive(false);
        }
    }

    // 关闭 SpiritDetail 面板
    public void CloseSpiritDetail()
    {
        spiritDetail.SetActive(false);
        
    }

    // 关闭 SpiritPanel 面板
    public void CloseSpiritPanel()
    {
        Debug.Log("CloseSpiritPanel called");
        spiritPanel.SetActive(false);
        spiritDetail.SetActive(false);
    }
}
