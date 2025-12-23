using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveUpUI : MonoBehaviour
{
    public Canvas canvas;

    public List<RectTransform> upUIs = new();
    public List<float> upUIHeights = new();
    public float lastHeight = 0f;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    public GameObject GiveUpUI(GameObject prefab)
    {
        GameObject uiObj = GameObject.Instantiate(prefab, canvas.transform);
        RectTransform rect = uiObj.GetComponent<RectTransform>();
        rect.anchoredPosition += new Vector2(0, lastHeight + rect.rect.height / 2);
        lastHeight += rect.rect.height;
        upUIs.Add(rect);
        upUIHeights.Add(rect.rect.height);
        return uiObj;
    }
    void Update()
    {
        //当一个条不见后，修改后续的条的位置以及高度
        for (int i = 0; i < upUIs.Count; i++)
        {
            if (upUIs[i] == null)
            {
                float height = upUIHeights[i];
                for (int j = i + 1; j < upUIs.Count; j++)
                {
                    if (upUIs[j] != null)
                    {
                        upUIs[j].anchoredPosition -= new Vector2(0, height);//向下移动
                    }
                }
                upUIs.RemoveAt(i);
                upUIHeights.RemoveAt(i);
                lastHeight -= height;
                i--;
            }
        }
    }
}
