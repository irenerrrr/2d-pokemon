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

    public Button battleSequenceButton;
    public GameObject battleSequencePanel;
    
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

        battleSequencePanel.SetActive(false);


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
        bool isbattleSequencePanelActive = battleSequencePanel.activeSelf;

        if (!isSpiritDetailActive)
        {
            // 如果两个面板都没有显示，显示 spiritPanel
            spiritPanel.SetActive(true);
            spiritDetail.SetActive(true);
            battleSequencePanel.SetActive(false);

            spiritbagManager.selectedBeastIndex = 0;
            spiritbagManager.UpdateCurrentBeastPanel();
        }
        else
        {
            // 否则，关闭所有面板
            spiritPanel.SetActive(false);
            spiritDetail.SetActive(false);
            battleSequencePanel.SetActive(false);
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

    public void ToggleBattleSequencePanel()
    {
        bool isbattleSequencePanelActive = battleSequencePanel.activeSelf;


        // 如果 spiritDetail 是打开状态，则关闭它
        if (!isbattleSequencePanelActive)
        {
            spiritPanel.SetActive(true);
            battleSequencePanel.SetActive(true);
            spiritDetail.SetActive(false);
            
        }

        else
        {
            // 否则，关闭所有面板
            spiritPanel.SetActive(false);
            spiritDetail.SetActive(false);
            battleSequencePanel.SetActive(false);
        }
    }

}
