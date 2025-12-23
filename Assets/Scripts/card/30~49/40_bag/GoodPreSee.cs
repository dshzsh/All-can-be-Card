using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GoodPreSee : MonoBehaviour,IEscClose
{
    public static GameObject goodPreSee;
    public GoodSelectUI goodSelectUI;

    private void OnEnable()
    {
        SControl_3.AddEscClose(this);
    }
    private void OnDisable()
    {
        SControl_3.RemoveEscClose(this);
    }
    public void Close()
    {
        SControl_3.RemoveEscClose(this);
        Destroy(gameObject);
    }
    public static RectTransform SetPreSee(CardBase card)
    {
        GameObject obj = UIBasic.GiveUI(goodPreSee);
        obj.GetComponent<GoodPreSee>().goodSelectUI.SetCard(card);
        RectTransform rect = obj.GetComponent<RectTransform>();
        SetPreSeePos(rect);
        return rect;
    }

    private static Vector2 offset = new Vector2(250, 80);//默认为右下角
    //输入的内容中心点位置：(0.5,1)
    public static void SetPreSeePos(RectTransform panelRect)
    {
        Vector2 originalPosition = UIBasic.GetUIlocalPosition(Input.mousePosition);

        RectTransform canvasRect = UIBasic.canvas;
        Vector2 canvasSize = canvasRect.sizeDelta;

        Vector2 tooltipSize = panelRect.sizeDelta;

        // 计算默认位置（物品右侧）
        Vector2 position = originalPosition + offset;

        // 检查是否会超出屏幕右侧
        if (position.x + tooltipSize.x/2 > canvasSize.x/2)
        {
            // 尝试显示在物品左侧
            position.x = originalPosition.x - offset.x;
        }

        // 确保上下在屏幕内
        position.y = Mathf.Clamp(position.y, -canvasSize.y / 2 + tooltipSize.y, +canvasSize.y / 2);

        panelRect.anchoredPosition = position;
    }
}
