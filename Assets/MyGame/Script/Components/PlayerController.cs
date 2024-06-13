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
        beastInfoUI = FindObjectOfType<BeastInfoUI>(); // 找到 BeastInfoUI 组件
        if (beastInfoUI == null)
        {
            Debug.LogError("BeastInfoUI component not found in the scene.");
        }

    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, beastLayer);
        if (hit != null)
        {
            BeastComponent beastComponent = hit.GetComponent<BeastComponent>();
            if (beastComponent != null)
            {
                // 获取兽的属性并更新 UI 面板
                SpiritualBeast beast = beastComponent.beast;
                beastInfoUI.UpdateBeastInfo(beast);
            }
        }
        else
        {
            beastInfoUI.HideBeastInfo();
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
        if (collision.tag == "Wild")
        {
            SceneManager.LoadScene(0);
            respawnPoint = transform.position;
        }
        else if (collision.tag == "Town")
        {
            SceneManager.LoadScene(1);
            respawnPoint = transform.position;
        }
    }

}
