using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float detectionRadius = 5f; // 检测半径
    public LayerMask beastLayer; // 兽所在的层
    private BeastInfoUI beastInfoUI;
    private Animator playerAnimation;
    private Rigidbody2D player;
    private Vector2 respawnPoint;

    public static PlayerController Instance; 

    private SpiritualBeast beast; // 引用当前的 Beast 对象
    public SpiritBagManager spiritBagManager;

    public FusionManager fusionManager;
    private float stayTime = 0f;
    private bool playerInTrigger = false;

 
    private void Awake()
    {
        InitializeComponents(); // 初始化组件
      
    }
    
    private void InitializeComponents()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        Debug.Log("Components initialized once.");
    }
    void Start()
    {
        beastInfoUI = FindObjectOfType<BeastInfoUI>();  // 确保找到 BeastInfoUI 组件
        fusionManager = FindObjectOfType<FusionManager>(); 


    }

    void Update()
    {
        if (playerInTrigger)
        {
            stayTime += Time.deltaTime;
            if (stayTime >= 0.5f)
            {
                fusionManager.ShowFusionPanel();
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    //handle teleport scene
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Fusion"))
        {
            // 当玩家进入触发器时，显示Fusion Panel
            playerInTrigger = true;
            stayTime = 0f; // 重置计时器
        }
        else if (collision.CompareTag("Wild"))
        {
            // 切换到野外场景
            SceneManager.LoadScene(0);
            respawnPoint = transform.position;
        }
        else if (collision.CompareTag("Town"))
        {
            // 切换到城镇场景
            SceneManager.LoadScene(1);
            respawnPoint = transform.position;
        }
        else if (collision.CompareTag("Beast"))
        {
        if (collision.CompareTag("Beast"))
        {
            Debug.Log("碰到Beast");
            
            BeastComponent beastComponent = collision.GetComponentInParent<BeastComponent>();
            if (beastComponent != null)
            {
                Debug.Log("找到BeastComponent");
                
                BeastComponent.encounteredBeast = beastComponent.beast;
                Debug.Log("encounteredBeast: " + BeastComponent.encounteredBeast.name);
                
                if (spiritBagManager != null && spiritBagManager.GetFirstBeast() != null)
                {
                    BeastComponent.playerFirstBeast = spiritBagManager.GetFirstBeast();
                    Debug.Log("playerFirstBeast: " + BeastComponent.playerFirstBeast.name);
                }
                else
                {
                    Debug.Log("spiritBagManager或GetFirstBeast()返回空");
                }
                
                SceneManager.LoadScene("BattleScene");
                Debug.Log("切换到BattleScene");
            }
            else
            {
                Debug.Log("未找到BeastComponent");
            }
        }
           
        }
        //check beast
        // if (collision.CompareTag("Beast"))  // 这里使用 CompareTag 方法检查标签是否为 "Beast"
        // {
        //     BeastComponent beastComponent = collision.GetComponent<BeastComponent>();
        //     if (beastComponent != null)
        //     {
        //         SpiritualBeast beast = beastComponent.beast;
        //         beastInfoUI.UpdateBeastInfo(beast);
        //     }
        // }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Fusion"))
        {
            playerInTrigger = false;
            stayTime = 0f; // 重置计时器
            fusionManager.HideFusionPanel();
            fusionManager.ResetFusionPanel();
        }
    }
    //     if (collision.CompareTag("Beast"))  // 同样使用 CompareTag 方法
    //     {
    //         beastInfoUI.HideBeastInfo();
    //     }
    // }

}