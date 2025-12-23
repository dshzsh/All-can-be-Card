using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MItemCntUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    
    private float displayedCoin = 0f;
    private float changeAmount = 0f;
    private Coroutine displayCoroutine;
    private string GetCoinStr(float coin)
    {
        string ans = MyTool.SuperNumText(coin);
        if (ans.Contains('.'))
        {
            ans = ans.TrimEnd('0');
            ans = ans.TrimEnd('.');
        }
        return ans;
    }
    public void UpdateSet(float num)
    {
        float coin = num;

        // 检测coin值是否变化
        if (Mathf.Abs(coin - displayedCoin) > Mathf.Epsilon)
        {
            changeAmount = coin - displayedCoin;

            // 如果已经有显示协程在运行，先停止它
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }

            // 启动新的显示协程
            displayCoroutine = StartCoroutine(DisplayChange());

            displayedCoin = coin;
        }
    }

    private IEnumerator DisplayChange()
    {
        float duration = 1.5f; // 显示变化的时间长度
        float elapsed = 0f;
        Color color;

        // 格式化变化量，保留符号
        string changeText;
        if (changeAmount > 0)
        {
            color = Color.green;
            changeText = "+" + GetCoinStr(changeAmount);
        }
        else
        {
            color = Color.red;
            changeText = GetCoinStr(changeAmount);
        }

        while (elapsed < duration)
        {
            // 透明度
            if (elapsed > 1f)
                color.a = duration - elapsed;

            // 更新显示文本
            text.text = $"{GetCoinStr(displayedCoin)} <color=#{ColorUtility.ToHtmlStringRGBA(color)}>{changeText}</color>";

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 时间结束后恢复普通显示
        text.text = $"{GetCoinStr(displayedCoin)}";
        displayCoroutine = null;
    }
}
