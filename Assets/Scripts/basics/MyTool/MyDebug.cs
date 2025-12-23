using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyDebug 
{
    public static void DrawLine(Vector3 start, Vector3 end, float time=1)
    {
        GameObject lineObj = new GameObject("Line");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // 配置 LineRenderer 的属性
        lineRenderer.startWidth = 0.05f; // 起点宽度
        lineRenderer.endWidth = 0.05f;   // 终点宽度
        lineRenderer.positionCount = 2; // 两个顶点：起点和终点
        lineRenderer.useWorldSpace = true; // 使用世界坐标
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 设置默认材质
        lineRenderer.startColor = Color.white; // 起点颜色
        lineRenderer.endColor = Color.white;   // 终点颜色

        // 设置线段的起点和终点
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        Object.Destroy(lineObj, time);
    }
    public static void LogWarning(string msg)
    {
        Debug.LogWarning(msg);
    }
}
