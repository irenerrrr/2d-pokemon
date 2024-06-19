using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float detectionRadius = 5f; // 检测半径
    public LayerMask beastLayer; // 兽所在的层
    private Animator playerAnimation;

    public SpiritBagManager spiritBagManager;
    public BeastManager beastManager;
    public FusionManager fusionManager;

    private float stayTime = 0f;
    private bool playerInTrigger = false;

    private Rigidbody2D player;
    private Vector2 respawnPoint;
    private static string staticPreviousSceneName;
    private Vector3 playerPosition;

    public static PlayerController Instance; 
    public static bool isInBattle = false;
    private PersistenceController persistenceController;

    public static bool isReturningFromBattle = false;
    private BeastSpawner beastSpawner;

 
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this; // 初始化静态实例
            InitializeComponents(); // 初始化组件
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        persistenceController = FindObjectOfType<PersistenceController>();
    }
    
    private void InitializeComponents()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        persistenceController = PersistenceController.Instance;
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

    // 处理场景切换和保存信息
    private void SaveCurrentSceneInfo()
    {
        staticPreviousSceneName = SceneManager.GetActiveScene().name; // 使用静态变量
        playerPosition = transform.position;
        Debug.Log($"[SaveCurrentSceneInfo] Current scene {staticPreviousSceneName} saved with player position {playerPosition}");
    }

    private void LoadPreviousScene()
    {
        if (!string.IsNullOrEmpty(staticPreviousSceneName))
        {
            Debug.Log($"[LoadPreviousScene] Loading previous scene: {staticPreviousSceneName}");
            SceneManager.LoadScene(staticPreviousSceneName); // 使用静态变量
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.LogError("[LoadPreviousScene] Previous scene name is invalid.");
        }
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == staticPreviousSceneName) // 使用静态变量
        {
            StartCoroutine(DelayedSetPosition());
            transform.position = playerPosition;
            persistenceController.RestoreBeastsToScene();
            isReturningFromBattle = false;
            beastSpawner = FindObjectOfType<BeastSpawner>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    private IEnumerator DelayedSetPosition()
    {
        yield return null; // 等待一帧，确保所有对象已初始化
        transform.position = playerPosition;
        Debug.Log($"[OnSceneLoaded] Player position restored to {playerPosition}");

        persistenceController.RestoreBeastsToScene();
        isReturningFromBattle = false;
        beastSpawner = FindObjectOfType<BeastSpawner>();
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
            // 切换到城镇场景
            // SaveCurrentSceneInfo();
            if (persistenceController != null)
            {
                // 清理地图上的所有Beast对象
                persistenceController.ClearPersistedObjects();
            }
            SceneManager.LoadScene(0);
            respawnPoint = transform.position;
        }
        else if (collision.CompareTag("Town"))
        {
            SceneManager.LoadScene(1);
            respawnPoint = transform.position;
        }
        else if (collision.CompareTag("Beast"))
        {
            Debug.Log("碰到Beast");

            // 检查是否已经在战斗中
            if (isInBattle == true)
            {
                Debug.Log("已经在战斗中，不能传送");
                return;
            }
            beastSpawner = FindObjectOfType<BeastSpawner>();
            // if (beastSpawner != null)
            // {
            //     beastSpawner.EnableSpawning(false);
            //     Debug.Log("beastSpawner.enabled");
            // }
                    
            BeastComponent beastComponent = collision.GetComponentInParent<BeastComponent>();
            if (beastComponent != null)
            {
                Debug.Log("找到BeastComponent");
                
                BeastComponent.encounteredBeast = beastComponent.beast;
                beastComponent.beast.beastGameObject = beastComponent.gameObject; // 确保正确设置
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

                SaveCurrentSceneInfo();

                persistenceController.MoveBeastsToDontDestroyOnLoad(persistenceController.SpawnedBeasts);

                isInBattle = true;
                isReturningFromBattle = true;

                SceneManager.LoadScene("BattleScene");
                Debug.Log("切换到BattleScene");
            }
            else
            {
                Debug.Log("未找到BeastComponent");
            }
        }
           
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

    public void EndBattle()
    {
        DecreaseIntimacyForParticipatedBeasts();
        LoadPreviousScene();
        isInBattle = false;
        Debug.Log($"返回到场景 {staticPreviousSceneName}"); // 使用静态变量
    }

    private void DecreaseIntimacyForParticipatedBeasts()
    {
        foreach (var beastComponent in FindObjectsOfType<BeastComponent>())
        {
            if (beastComponent.participatedInBattle)
            {
                beastComponent.beast.DecreaseIntimacy(3);
                beastComponent.participatedInBattle = false; // 重置参战标记
                Debug.Log(beastComponent.beast.name + "的亲密度减少了3");
            }
        }
    }

}