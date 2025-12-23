using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; // UI元素的RectTransform
    private RectTransform canvas;
    private CanvasGroup canvasGroup; // 用于处理拖动时的透明度
    private Vector2 offset; // 鼠标点击位置与UI中心点的偏移量

    private void Awake()
    {
        // 获取UI元素的RectTransform
        rectTransform = transform.parent.GetComponent<RectTransform>();

        // 获取父Canvas
        canvas = UIBasic.canvas.GetComponent<RectTransform>();

        // 添加CanvasGroup组件（如果不存在）
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = transform.parent.gameObject.AddComponent<CanvasGroup>();
        }
    }

    // 开始拖动时调用
    public void OnBeginDrag(PointerEventData eventData)
    {
        GoodPoinerPreSeeUI.inDragCnt++;
        // 降低透明度以表示拖动状态
        canvasGroup.alpha = 0.6f;

        // 禁用射线检测，避免拖动时遮挡其他UI
        canvasGroup.blocksRaycasts = false;

        // 计算鼠标点击位置与UI中心点的偏移量
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas, // 父Canvas的RectTransform
            eventData.position, // 鼠标的屏幕坐标
            null, // Canvas的相机（如果是Screen Space - Overlay模式，可以为null）
            out position // 输出的局部坐标
        );
        // 计算偏移量
        offset = rectTransform.anchoredPosition - position;
    }

    // 拖动过程中调用
    public void OnDrag(PointerEventData eventData)
    {
        // 将鼠标的屏幕坐标转换为UI的局部坐标
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas, // 父Canvas的RectTransform
            eventData.position, // 鼠标的屏幕坐标
            null, // Canvas的相机（如果是Screen Space - Overlay模式，可以为null）
            out position // 输出的局部坐标
        );

        // 更新UI元素的位置
        rectTransform.anchoredPosition = position + offset;

    }

    // 结束拖动时调用
    public void OnEndDrag(PointerEventData eventData)
    {
        GoodPoinerPreSeeUI.inDragCnt--;
        // 恢复透明度
        canvasGroup.alpha = 1f;

        // 启用射线检测
        canvasGroup.blocksRaycasts = true;
    }
}