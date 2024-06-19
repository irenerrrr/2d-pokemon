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
    public float minSpawnDistance = 1.0f; 

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
        List<Vector2> spawnedPositions = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            // Debug.Log("生成beast_" + i);
            Vector2 randomPosition = GetRandomPositionWithinPolygonCollider(spawnArea, spawnedPositions);
            GameObject beastInstance = Instantiate(BeastPrefab, randomPosition, Quaternion.identity);
            if (beastInstance != null)
            {
                SpiritualBeast beast = beastGenerator.GenerateBeast();
                BeastComponent beastComponent = beastInstance.GetComponent<BeastComponent>();
                beastComponent.beast = beast;
                beast.beastGameObject = beastInstance;

                SpriteRenderer spriteRenderer = beastInstance.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = beast.image;

                spawnedPositions.Add(randomPosition);
                Debug.Log($"Beast {i} successfully generated and initialized at position {beastInstance.transform.position}");
            }
            else
            {
                Debug.LogError("生成Beast失败");
            }
        }
    }

    private Vector2 GetRandomPositionWithinPolygonCollider(PolygonCollider2D collider, List<Vector2> existingPositions)
    {
        Vector2 randomPoint;
        Bounds bounds = CalculatePolygonColliderBounds(collider);

        // 最大尝试次数以防止无限循环
        int maxAttempts = 1000;
        int attempts = 0;

        do
        {
            randomPoint = GenerateRandomPointWithinBounds(bounds);
            attempts++;
            if (attempts > maxAttempts)
            {
                Debug.LogError("Failed to find a valid point within the polygon after maximum attempts.");
                break;
            }
        } while (!PointInPolygon(collider, randomPoint) || IsTooCloseToExistingPoints(randomPoint, existingPositions));

        Debug.Log($"Found random point {randomPoint} within polygon after {attempts} attempts.");
        return randomPoint;
    }

    private Bounds CalculatePolygonColliderBounds(PolygonCollider2D collider)
    {
        Vector2[] points = collider.points;
        Vector2 min = points[0];
        Vector2 max = points[0];

        for (int i = 1; i < points.Length; i++)
        {
            Vector2 transformedPoint = collider.transform.TransformPoint(points[i]);
            min = Vector2.Min(min, transformedPoint);
            max = Vector2.Max(max, transformedPoint);
        }

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    private Vector2 GenerateRandomPointWithinBounds(Bounds bounds)
    {
        // 使用collider的bounds来生成随机点
        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        Debug.Log("min x" + bounds.min.x + "max x"+ bounds.max.x);
        Debug.Log("min y" + bounds.min.y + "max y"+ bounds.max.y);
        return new Vector2(randomX, randomY);
    }


    private bool PointInPolygon(PolygonCollider2D collider, Vector2 point)
    {
        int numPoints = collider.GetTotalPointCount();
        int j = numPoints - 1;
        bool inside = false;
        Vector2[] points = new Vector2[numPoints];

        // 将本地坐标转换为世界坐标
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = collider.transform.TransformPoint(collider.points[i]);
        }

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

    private bool IsTooCloseToExistingPoints(Vector2 point, List<Vector2> existingPoints)
    {
        foreach (var existingPoint in existingPoints)
        {
            if (Vector2.Distance(point, existingPoint) < minSpawnDistance)
            {
                return true;
            }
        }
        return false;
    }


}
