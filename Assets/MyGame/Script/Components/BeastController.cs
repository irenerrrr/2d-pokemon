using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastController : MonoBehaviour
{
    private Animator animator;
    public float speed = 0.2f; // 行走速度
    public float minWalkTime = 3.0f; // 最短行走时间
    public float maxWalkTime = 5.0f; // 最长行走时间
    public float minIdleTime = 1.0f; // 最短站立时间
    public float maxIdleTime = 3.0f; // 最长站立时间

    private Vector3 direction;

    public PolygonCollider2D spawnArea; // 生成区域
    private bool isWalking;

    void Start()
    {
        animator = GetComponent<Animator>();
        direction = GetRandomDirection(); // 获取随机方向
        StartCoroutine(WalkAndIdleRoutine());
    }

    void Update()
    {
        if (isWalking)
        {
            Move();
            CheckBounds();
        }
    }

    void Move()
    {
        // 移动 beast
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void CheckBounds()
    {
        // 检查是否超出范围
        if (!PointInPolygon(spawnArea, transform.position))
        {
            direction = GetRandomDirection(); // 获取新的随机方向
        }
    }

    Vector3 GetRandomDirection()
    {
        // 获取一个随机方向，涵盖所有可能方向
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        return new Vector3(x, y, 0).normalized;
    }

    private bool PointInPolygon(PolygonCollider2D collider, Vector2 point)
    {
        int numPoints = collider.GetTotalPointCount();
        int j = numPoints - 1;
        bool inside = false;
        Vector2[] points = collider.points;

        for (int i = 0; i < numPoints; j = i++)
        {
            Vector2 p1 = collider.transform.TransformPoint(points[i]);
            Vector2 p2 = collider.transform.TransformPoint(points[j]);

            if (((p1.y > point.y) != (p2.y > point.y)) &&
                (point.x < (p2.x - p1.x) * (point.y - p1.y) / (p2.y - p1.y) + p1.x))
            {
                inside = !inside;
            }
        }
        return inside;
    }

    IEnumerator WalkAndIdleRoutine()
    {
        while (true)
        {
            // 50-50 概率决定是否行走
            isWalking = Random.value > 0.5f;

            if (isWalking)
            {
                animator.SetBool("isWalk", true);
                direction = GetRandomDirection(); // 获取新的随机方向

                // 随机行走时间
                float walkTime = Random.Range(minWalkTime, maxWalkTime);
                yield return new WaitForSeconds(walkTime);
            }
            else
            {
                animator.SetBool("isWalk", false);

                // 随机站立时间
                float idleTime = Random.Range(minIdleTime, maxIdleTime);
                yield return new WaitForSeconds(idleTime);
            }
        }
    }
}
