using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(LineRenderer))]
public class RadarChart : MonoBehaviour
{
    public Vector3[] points; // 存储雷达图的各顶点位置
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupRadarChart();
    }

    void SetupRadarChart()
    {
        float[] values = { 422, 417, 186, 178, 376, 521 };  // 各顶点的值，应与属性值对应
        float maxValue = 550; // 值的最大范围，用于归一化
        float radius = 250; // 雷达图的半径
        int numPoints = values.Length;
        lineRenderer.positionCount = numPoints + 1; // 设置顶点数，+1是为了回到起点闭合形状

        for (int i = 0; i < numPoints; i++)
        {
            float normalizedValue = values[i] / maxValue; // 归一化值
            float angle = i * 2 * Mathf.PI / numPoints;
            float x = Mathf.Cos(angle) * normalizedValue * radius;
            float y = Mathf.Sin(angle) * normalizedValue * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

        // 闭合形状，将最后一个点设置为第一个点
        lineRenderer.SetPosition(numPoints, lineRenderer.GetPosition(0));

        // 配置LineRenderer
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.useWorldSpace = false; // 使用局部坐标，确保跟随GameObject移动
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.gray;
    }
}