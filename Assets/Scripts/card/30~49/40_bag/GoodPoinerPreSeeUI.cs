using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoodPoinerPreSeeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    public static int inDragCnt = 0;
    GoodUI goodUI;
    private RectTransform selectPreSeeRect;
    private RectTransform pointPreSeeRect;

    private void Awake()
    {
        goodUI = GetComponent<GoodUI>();
    }
    private void Update()
    {
        if (!CanPointerMovePreSee() || pointPreSeeRect == null)
        {
            EndPointer();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (selectPreSeeRect != null)
            GameObject.Destroy(selectPreSeeRect.gameObject);
        EndPointer();
        selectPreSeeRect = GoodPreSee.SetPreSee(goodUI.good);
    }

    public bool CanPointerMovePreSee()
    {
        if (inDragCnt > 0) return false;
        if (selectPreSeeRect != null) return false;
        if (goodUI.good == null || goodUI.good.id == 0) return false;
        return true;
    }

    private void EndPointer()
    {
        if (pointPreSeeRect != null)
            Destroy(pointPreSeeRect.gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanPointerMovePreSee()) return;
        pointPreSeeRect = GoodPreSee.SetPreSee(goodUI.good);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if (!CanPointerMovePreSee() || pointPreSeeRect == null)
        {
            EndPointer();
            return;
        }
        GoodPreSee.SetPreSeePos(pointPreSeeRect);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanPointerMovePreSee() || pointPreSeeRect == null)
        {
            EndPointer();
            return;
        }
        Destroy(pointPreSeeRect.gameObject);
    }
    private void OnDisable()
    {
        EndPointer();
    }
    private void OnDestroy()
    {
        if (selectPreSeeRect != null)
            Destroy(selectPreSeeRect.gameObject);
    }
}
