using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BeastSpawner : MonoBehaviour
{
    public PolygonCollider2D spawnArea; // 使用 PolygonCollider2D
    public GameObject BeastPrefab;

    public int spawnCount = 10; // 每次生成的Beast数量
    private BeastGenerator beastGenerator;
    private SpiritBagManager spiritbagManager; // 添加 SlotManager 的引用
   

    void Start()
    {
        beastGenerator = GetComponent<BeastGenerator>();
        spiritbagManager = FindObjectOfType<SpiritBagManager>();
        GenerateBeastsInArea(spawnCount);
    }

    void GenerateBeastsInArea(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 随机选择生成类型
            GameObject beastPrefab = BeastPrefab;

            // 随机位置生成
            Vector2 randomPosition = GetRandomPositionWithinPolygonCollider(spawnArea);
            GameObject beastInstance = Instantiate(beastPrefab, randomPosition, Quaternion.identity);

            // 设置生成的兽的属性
            SpiritualBeast beast = beastGenerator.GenerateBeast();

            BeastComponent beastComponent = beastInstance.GetComponent<BeastComponent>();
            beastComponent.beast = beast;

            SpriteRenderer spriteRenderer = beastInstance.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = beast.image;

            // 如果生成的是 SpiritualBeast，将其添加到背包
            if (beast.type == "SpiritualBeast")
            {
                Debug.Log($"Generated Beast: Name={beast.name}, Level={beast.level}, Gender={beast.gender}, HP={beast.hp}, Attack={beast.attack}, Armor={beast.armor}, AP={beast.ap}, MR={beast.mr}, Speed={beast.speed}");
                spiritbagManager.AddBeast(beast); 
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
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
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
