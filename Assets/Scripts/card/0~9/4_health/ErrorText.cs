using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorText : MonoBehaviour
{
    private const float moveDuration = 1f;
    private const float destroyDelay = 1.4f;
    private float timer = 0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        // 初始化起始位置和目标位置
        startPosition = transform.position;
        float moveDistance = 40f * UIBasic.canvasScaler.scaleFactor;
        targetPosition = startPosition + Vector3.up * moveDistance;

        isMoving = true;
    }
    // OutQuad缓动函数：开始快，结束时慢
    private float EaseOutQuad(float t)
    {
        return 1 - (1 - t) * (1 - t);
    }

    void Update()
    {
        if (isMoving)
        {
            // 更新计时器
            timer += Time.unscaledDeltaTime;

            // 移动物体
            if (timer <= moveDuration)
            {
                float progress = timer / moveDuration;
                float easedProgress = EaseOutQuad(progress);
                transform.position = Vector3.Lerp(startPosition, targetPosition, easedProgress);
            }

            // 检查是否到达销毁时间
            if (timer >= destroyDelay)
            {
                Destroy(gameObject);
            }
        }
    }
}
