using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class BeastSpawner : MonoBehaviour
{

    public PolygonCollider2D spawnArea; // 使用 PolygonCollider2D
    public GameObject BeastPrefab;

    public int spawnCount = 10; // 每次生成的Beast数量
    private BeastGenerator beastGenerator;
    private SpiritBagManager spiritbagManager; // 添加 SlotManager 的引用
    private bool enableSpawning = true; 


    
    // void Awake()
    // {
    //     Debug.Log("BeastSpawner Awake");
    //     beastGenerator = GetComponent<BeastGenerator>();
        
    // }
    void Start()
    {
        beastGenerator = GetComponent<BeastGenerator>();
        CheckAndGenerateBeasts();
    }

    public void EnableSpawning(bool enable)
    {
        enableSpawning = enable;
        if (enableSpawning)
        {
            CheckAndGenerateBeasts();
        }
    }


    public void CheckAndGenerateBeasts()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("当前场景: " + currentSceneName);
        if (PlayerController.isInBattle)
        {
            Debug.Log("当前在BattleScene中,不执行Beast生成逻辑");
            return;
        }
        if (SceneManager.GetActiveScene().name != "Countryside")
        {
            Debug.Log("当前不在Countryside Scene中, 不执行Beast生成逻辑");
            return;
        } 
        Debug.Log("当前在Countryside Scene中, 执行Beast生成逻辑");
      

        // 检查场景中现有的Beast数量
        int currentBeastCount = FindObjectsOfType<BeastComponent>().Length;
        Debug.Log("当前场景中的Beast数量: " + currentBeastCount);

        // 如果场景中的 Beast 数量少于 5，则生成新的 Beast
        if (currentBeastCount <= 6)
        {
            Debug.Log("生成新的Beast, 需要生成的数量: " + (spawnCount - currentBeastCount));
            GenerateBeastsInArea(spawnCount - currentBeastCount);
        }
    }


    public void GenerateBeastsInArea(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Debug.Log("生成beast_" + i);
            Vector2 randomPosition = GetRandomPositionWithinPolygonCollider(spawnArea);
            GameObject beastInstance = Instantiate(BeastPrefab, randomPosition, Quaternion.identity);
            if (beastInstance != null)
            {
                SpiritualBeast beast = beastGenerator.GenerateBeast();
                BeastComponent beastComponent = beastInstance.GetComponent<BeastComponent>();
                beastComponent.beast = beast;
                beast.beastGameObject = beastInstance;

                SpriteRenderer spriteRenderer = beastInstance.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = beast.image;

                // Debug.Log("Beast成功生成并初始化 at position " + beastInstance.transform.position);
            }
            else
            {
                Debug.LogError("生成Beast失败");
            }
        }
    }

    private Vector2 GetRandomPositionWithinPolygonCollider(PolygonCollider2D collider)
    {
        // 获取 collider 的边界
        Bounds bounds = collider.bounds;
        Vector2 randomPoint;

        // 生成随机点直到找到一个在 PolygonCollider2D 内的点
        do
        {
            float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            randomPoint = new Vector2(randomX, randomY);
        } while (!PointInPolygon(collider, randomPoint));

        return randomPoint;
    }

    private bool PointInPolygon(PolygonCollider2D collider, Vector2 point)
    {
        int numPoints = collider.GetTotalPointCount();
        int j = numPoints - 1;
        bool inside = false;
        Vector2[] points = collider.points;

        for (int i = 0; i < numPoints; j = i++)
        {
            if (((points[i].y > point.y) != (points[j].y > point.y)) &&
                (point.x < (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
            {
                inside = !inside;
            }
        }
        return inside;
    }


}
