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
   
    private int k = 0;
    private static Dictionary<string, SpiritualBeast> spawnedBeasts = new Dictionary<string, SpiritualBeast>();

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
            string beastId = "Beast_" + i;

            if (!spawnedBeasts.ContainsKey(beastId))
            {
                Vector2 randomPosition = GetRandomPositionWithinPolygonCollider(spawnArea);
                GameObject beastInstance = Instantiate(BeastPrefab, randomPosition, Quaternion.identity);

                SpiritualBeast beast = beastGenerator.GenerateBeast();
                BeastComponent beastComponent = beastInstance.GetComponent<BeastComponent>();
                beastComponent.beast = beast;

                SpriteRenderer spriteRenderer = beastInstance.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = beast.image;

                spawnedBeasts[beastId] = beast;

                if (beast.type == "SpiritualBeast" && k <= 2)
                {
                    Debug.Log($"Generated Beast: Name={beast.name}, Level={beast.level}, Gender={beast.gender}, MaxHP={beast.maxHp}, MaxAttack={beast.maxAttack}, MaxArmor={beast.maxArmor}, MaxAP={beast.maxAp}, MaxMR={beast.maxMr}, MaxSpeed={beast.maxSpeed}");
                    spiritbagManager.AddBeast(beast);
                }
                k += 1;
            }
            else
            {
                // 从已保存的状态中恢复敌人
                SpiritualBeast beast = spawnedBeasts[beastId];
                Vector2 randomPosition = GetRandomPositionWithinPolygonCollider(spawnArea);
                GameObject beastInstance = Instantiate(BeastPrefab, randomPosition, Quaternion.identity);

                BeastComponent beastComponent = beastInstance.GetComponent<BeastComponent>();
                beastComponent.beast = beast;

                SpriteRenderer spriteRenderer = beastInstance.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = beast.image;
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
