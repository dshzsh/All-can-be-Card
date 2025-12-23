using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.UIElements;

public class UIBasic : MonoBehaviour
{
    public static RectTransform canvas;
    public static CanvasScaler canvasScaler;
    public static GraphicRaycaster graphicRaycaster;
    public static EventSystem eventSystem;
    public static GameObject textObj;
    public static GameObject textUI;
    public static GameObject errorTextObj;
    public static GameObject sceneShow;// 场景中的一些需要放在主UI上的内容，需要被遮盖
    public static GameObject leftUpShow;

    private void Awake()
    {
        canvas = gameObject.GetComponent<RectTransform>();
        canvasScaler = gameObject.GetComponent<CanvasScaler>();
        graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        sceneShow = gameObject.transform.GetChild(0).gameObject;
        leftUpShow = gameObject.transform.GetChild(1).gameObject;
        eventSystem = FindObjectOfType<EventSystem>();
    }
    public static GameObject GiveUI(GameObject prefab, bool setPos = false)
    {
        GameObject obj = GameObject.Instantiate(prefab, canvas.transform);
        if(setPos)
            SetPerfectPos(obj);
        return obj;
    }
    public static void GiveErrorText(string text)
    {
        GameObject obj = GiveUI(errorTextObj);
        obj.transform.position = Input.mousePosition;
        
        obj.GetComponent<TextMeshProUGUI>().text = text;
    }
    
    public static List<RectTransform> leftUpUIs = new();
    public static List<float> leftUpUIHeights = new();
    public static float lastHeight = 0;
    /// <summary>
    /// 左上角为一堆状态条，会根据已有的条来调整新的内容的位置
    /// 加入新的左上角UI请让它初始贴紧上左的边
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject GiveLeftUpUI(GameObject prefab)
    {
        GameObject uiObj = GameObject.Instantiate(prefab, leftUpShow.transform);
        RectTransform rect= uiObj.GetComponent<RectTransform>();
        rect.anchoredPosition += new Vector2(0, -lastHeight);
        lastHeight += rect.rect.height;
        leftUpUIs.Add(rect);
        leftUpUIHeights.Add(rect.rect.height);
        return uiObj;
    }
    public static void UpdateLeftUI()
    {
        //当一个左上条不见后，修改后续的条的位置以及高度
        for (int i = 0; i < leftUpUIs.Count; i++)
        {
            if (leftUpUIs[i] == null)
            {
                float height = leftUpUIHeights[i];
                for (int j = i + 1; j < leftUpUIs.Count; j++)
                {
                    if (leftUpUIs[j] != null)
                    {
                        leftUpUIs[j].anchoredPosition += new Vector2(0, height);//向上移动
                    }
                }
                leftUpUIs.RemoveAt(i);
                leftUpUIHeights.RemoveAt(i);
                lastHeight -= height;
                i--;
            }
        }
    }

    public static GameObject liveUpUIPrefab;
    public static float liveUpUIHeight = 0.12f;
    public static GameObject GiveLiveUpUI(GameObject prefab, CObj_2 live)
    {
        if(live.liveUpUI== null)
        {
            live.liveUpUI = GameObject.Instantiate(liveUpUIPrefab, live.obj.transform).GetComponent<LiveUpUI>();
            live.liveUpUI.transform.position += Vector3.up * (liveUpUIHeight + live.height);
        }
        return live.liveUpUI.GiveUpUI(prefab);
    }
    public static void GiveText(string text, Vector3 pos, Color color, float size=1f, float time=0.4f)
    {
        GameObject tt = GameObject.Instantiate(textObj, pos, Quaternion.identity);
        tt.GetComponent<GiveTextInUI>().Set(text, color, size);
        Destroy(tt.gameObject, time);
    }
    private void Update()
    {
        UpdateLeftUI();
    }
    public static Vector3 WorldToUIPosition(Vector3 worldPos)
    {
        //Debug.Log(worldPos);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        //Debug.Log(screenPos);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas,
            screenPos,
            null,
            out Vector3 uiPos
        );
        //Debug.Log(uiPos);
        return uiPos;
    }
    public static Vector2 GetUIlocalPosition(Vector3 pos)
    {
        Vector2 ans;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas, // 父Canvas的RectTransform
            pos, // 鼠标的屏幕坐标
            null, // Canvas的相机（如果是Screen Space - Overlay模式，可以为null）
            out ans // 输出的局部坐标
        );
        return ans;
    }
    /// <summary>
    /// 需要是中心为0.5,1的UI位置
    /// </summary>
    public static void SetPerfectPos(GameObject obj)
    {
        RectTransform panelRect = obj.GetComponent<RectTransform>();
        Vector2 originalPosition = UIBasic.GetUIlocalPosition(Input.mousePosition);

        RectTransform canvasRect = UIBasic.canvas;
        Vector2 canvasSize = canvasRect.sizeDelta;
        Vector2 tooltipSize = panelRect.sizeDelta;
        Vector2 offset = new Vector2(tooltipSize.x / 2 + 20, tooltipSize.y / 8);


        // 计算默认位置（物品右侧）
        Vector2 position = originalPosition + offset;

        // 检查是否会超出屏幕右侧
        if (position.x + tooltipSize.x / 2 > canvasSize.x / 2)
        {
            // 尝试显示在物品左侧
            position.x = originalPosition.x - offset.x;
        }

        // 确保上下在屏幕内
        position.y = Mathf.Clamp(position.y, -canvasSize.y / 2 + tooltipSize.y, +canvasSize.y / 2);

        panelRect.anchoredPosition = position;
    }
}