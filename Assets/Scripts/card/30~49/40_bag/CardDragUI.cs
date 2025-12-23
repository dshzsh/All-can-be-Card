using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject goodUIPrefab;
    public static GameObject dragHintPrefab;
    public bool IsAbandon;
    public bool IsToBag;
    public static CardBase cardAbandon = new CardBase() { id = -1 };
    public static CardBase cardGoBag = new CardBase() { id = -2 };

    private GoodUI goodUI;
    private RectTransform myRectTransform; // UI元素的RectTransform
    private RectTransform dragHint;

    private RectTransform rectTransform; // UI元素的RectTransform
    private CanvasGroup canvasGroup; // 用于处理拖动时的透明度
    private Vector3 offset; // 鼠标点击位置与UI中心点的偏移量

    private void Awake()
    {
        myRectTransform = GetComponent<RectTransform>();
        // 添加CanvasGroup组件（如果不存在）
        goodUI= gameObject.GetComponent<GoodUI>();
        if (IsAbandon)
            goodUI.good = cardAbandon;
        if (IsToBag)
            goodUI.good = cardGoBag;
    }
    private bool CanDrag()
    {
        if (goodUI == null) return false;
        if (goodUI.good == null) return false;
        if (goodUI.good.id <= 0 || MyTag.HaveTag(goodUI.good.id, MyTag.CardTag.empty)) return false;//空物体不能拽
        if (goodUI.good.parent == null && goodUI.good.container == null)
            return false;//无根的物体不能拽
        return true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag())
        {
            return;
        }
        GoodPoinerPreSeeUI.inDragCnt++;
        GameObject tempGoodUI = UIBasic.GiveUI(goodUIPrefab);
        rectTransform = tempGoodUI.GetComponent<RectTransform>();
        rectTransform.position = myRectTransform.position;
        canvasGroup = tempGoodUI.AddComponent<CanvasGroup>();
        tempGoodUI.GetComponent<Image>().raycastTarget = false;
        tempGoodUI.GetComponent<GoodUI>().SetCard(goodUI.good);
        dragHint = UIBasic.GiveUI(dragHintPrefab).GetComponent<RectTransform>();
        dragHint.gameObject.SetActive(false);

        // 降低透明度以表示拖动状态
        canvasGroup.alpha = 0.3f;

        // 禁用射线检测，避免拖动时遮挡其他UI
        canvasGroup.blocksRaycasts = false;

        offset = rectTransform.position - (Vector3)eventData.position;
    }
    private CardDragUI lastDragUI;
    public void OnDrag(PointerEventData eventData)
    {
        if(!CanDrag())
        {
            return;
        }
        // 更新UI元素的位置
        rectTransform.position = (Vector3)eventData.position + offset;
        CardDragUI otherDrag = GetTopCardDragUI(eventData.position);
        if (otherDrag == lastDragUI) return;
        lastDragUI = otherDrag;

        if (otherDrag != null && otherDrag.goodUI.good != null)
        {
            //让提示目标框变到目标的drag位置，并且判断是否能拽
            GoodUI other = otherDrag.goodUI;
            bool ok = CanCardDrag(GetLive(goodUI), goodUI.good,
                GetLive(other), other.good, false);
            dragHint.gameObject.SetActive(true);
            dragHint.transform.position = other.transform.position;
            dragHint.transform.localScale = other.transform.lossyScale;
            dragHint.sizeDelta = other.GetComponent<RectTransform>().sizeDelta;
            Color color = ok ? Color.green : Color.red;color.a = 0.5f;
            dragHint.GetComponent<Image>().color = color;
        }
        else dragHint.gameObject.SetActive(false);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanDrag())
        {
            return;
        }
        GoodPoinerPreSeeUI.inDragCnt--;
        CardDragUI otherDrag = GetTopCardDragUI(eventData.position);
        if (otherDrag != null)
        {
            GoodUI other = otherDrag.goodUI;
            CardDrag(GetLive(goodUI), goodUI.good, 
                GetLive(other), other.good);
        }

        Destroy(rectTransform.gameObject);
        Destroy(dragHint.gameObject);
    }

    private CardBase GetLive(GoodUI goodUI)
    {
        CardBase top = CardManager.GetTop(goodUI.good);
        while (top != null && top.container != null)
            top = CardManager.GetTop(top.container);
        return top;
    }
    private static bool InBag(CardBase card)
    {
        return card.container != null && card.container is Cbag_40;
    }
    public CardDragUI GetTopCardDragUI(Vector3 pos)
    {
        PointerEventData pointerEventData = new PointerEventData(UIBasic.eventSystem);
        pointerEventData.position = pos; // 获取鼠标位置

        // 执行射线检测
        List<RaycastResult> results = new List<RaycastResult>();
        UIBasic.graphicRaycaster.Raycast(pointerEventData, results);

        // 返回最上层的符合条件的 UI 对象
        if (results.Count == 0) return null;

        CardDragUI cardUI = results[0].gameObject.GetComponent<CardDragUI>();
        if (cardUI != null)
        {
            return cardUI;
        }
        
        return null; // 没有找到
    }

    public static void AbandonCardFromPos(CardBase live, CardBase card)
    {
        if (card.id == 0) return;
        OffCard(live, card);
        Vector3 pos = Vector3.zero;
        Quaternion quaternion = Quaternion.identity;
        if (CardManager.TryGetCobj(live, out var cobj))
        {
            pos = cobj.obj.Center + cobj.obj.transform.forward;
            quaternion = cobj.obj.transform.rotation;
        }
        SMOitemObj_4.GiveItemObj(card, pos, quaternion);
    }
    public static void GoBagCardFromPos(CardBase live, CardBase card)
    {
        if (InBag(card)) return;//不再在背包中给背包
        if (card.id == 0) return;
        OffCard(live, card);
        if (CardManager.TryGetCobj(SystemManager.GetMainPlayer(), out var cobj))
            SystemManager.SendMsg(cobj.myBag, MsgType.SelfContainerGet, new MsgGetItem(card, 1) { container = cobj.myBag });
    }
    public static int OffCard(CardBase live, CardBase card)
    {
        int offPos = -1;
        //Debug.Log("off:" + cardAbandon + " " + cpos);
        // if (CanOffCard(live, card, true) == -1) return -1;
        //Debug.Log("off:" + cardAbandon + " " + cpos);
        {
            if (card.container == null) return -1;
            MsgGetItem gmsg = new MsgGetItem(card, -1); gmsg.container = card.container;
            //Debug.Log(live);
            SystemManager.SendMsg(card.container, MsgType.SelfContainerGet, gmsg);
            SystemManager.SendMsg(card.container, MsgType.MyContainerGet, gmsg);
            offPos = gmsg.pos;
        }
        return offPos;
    }
    public static bool OnCard(CardBase live, CardBase card, int onPos, CardBase toParent, bool needJudge)
    {
        //空物体无法被获取
        if (MyTag.HaveTag(card.id, MyTag.CardTag.empty)) return true;
        if(needJudge)
            if (!CanOnCard(live, card, onPos, toParent, true)) return false;
        {
            if (toParent == null) return false;
            MsgGetItem gmsg = new MsgGetItem(card, 1, onPos); gmsg.container = toParent;
            //Debug.Log(live);
            SystemManager.SendMsg(toParent, MsgType.SelfContainerGet, gmsg);
            SystemManager.SendMsg(toParent, MsgType.MyContainerGet, gmsg);
            return gmsg.pos != -1;
        }
    }
    public static int CanOffCard(CardBase live, CardBase card, bool giveMsg)
    {
        //Debug.Log(live + " " + cardAbandon + " " + cardAbandon.container + " " + cpos);
        {
            if (card.container == null) return -1;
            MsgGetItem gmsg = new MsgGetItem(card, -1); gmsg.container = card.container;
            MsgContainerJudge cmsg = new MsgContainerJudge() { gmsg = gmsg, giveMsg = giveMsg };
            SystemManager.SendMsg(card.container, MsgType.SelfContainerJudge, cmsg);
            SystemManager.SendMsg(card.container, MsgType.MyContainerJudge, cmsg);
            if (cmsg.ok == true)
                return cmsg.gmsg.pos;
            else
            {
                if (giveMsg)
                    UIBasic.GiveErrorText(cmsg.errorMsg);
                return -1;
            }
        }
    }
    public static bool CanOnCard(CardBase live, CardBase card, int onPos, CardBase toParent, bool giveMsg)
    {
        if (MyTag.HaveTag(card.id, MyTag.CardTag.empty)) return true;
        if (onPos == -1) return false;
        {
            if (toParent == null) return false;
            MsgGetItem gmsg = new MsgGetItem(card, 1, onPos); gmsg.container = toParent;
            MsgContainerJudge cmsg = new MsgContainerJudge() { gmsg = gmsg, giveMsg = giveMsg };
            SystemManager.SendMsg(toParent, MsgType.SelfContainerJudge, cmsg);
            SystemManager.SendMsg(toParent, MsgType.MyContainerJudge, cmsg);
            if (cmsg.ok == false)
            {
                if (giveMsg)
                    UIBasic.GiveErrorText(cmsg.errorMsg);
                return false;
            }
        }
        return true;
    }
    public static bool CanCardDrag(CardBase liveFrom, CardBase from, CardBase liveTo, CardBase to, bool giveMsg)
    {
        if (from == null || to == null) return false;
        if (from == to) return false;//相当于取消操作

        if (to == cardAbandon || to == cardGoBag)
        {
            return CanOffCard(liveFrom, from, giveMsg) != -1;
        }

        CardBase pf = from.container;
        CardBase pt = to.container;

        if (pt == null || pf == null) return false;
        if (pt == pf && InBag(from)) return true;

        int findex = CanOffCard(liveFrom, from, giveMsg);
        int tindex = CanOffCard(liveTo, to, giveMsg);
        if (findex == -1 || tindex == -1) return false;
        bool okf = CanOnCard(liveFrom, to, findex, pf, giveMsg);
        bool okt = CanOnCard(liveTo, from, tindex, pt, giveMsg);
        if (!okf || !okt) return false;
        return true;
    }
    public static void CardDrag(CardBase liveFrom ,CardBase from, CardBase liveTo, CardBase to)
    {
        if (from == null) return;
        if (from == to) return;//相当于取消操作

        if (!CanCardDrag(liveFrom, from, liveTo, to, true)) return;

        //Debug.Log("liveFrom: " + liveFrom + "from: " + from + " fromPos:" + fromPos +"\n"+
        //    "liveTo: " + liveTo + " to: " + to + " toPos:" + toPos);

        if (to == cardAbandon)
        {
            AbandonCardFromPos(liveFrom, from);
            GoodSelectUI.RefreshAll(); BoxUI.RefreshAll();
            return;
        }

        if (to == cardGoBag)
        {
            GoBagCardFromPos(liveFrom, from);
            GoodSelectUI.RefreshAll(); BoxUI.RefreshAll();
            return;
        }

        CardBase pf = from.container;
        CardBase pt = to.container;
        //在背包内交换时位置有问题，因为两个都卸下后有向前导致pos不准确
        bool bagSwap = InBag(from) && InBag(to) && liveFrom == liveTo;

        int findex = OffCard(liveFrom, from);
        int tindex = OffCard(liveTo, to);
        
        if (findex == -1 || tindex == -1)
        {
            Debug.LogError("OffCard产生错误");
            //出现问题，还原内容
            if (findex != -1) OnCard(liveFrom, from, findex, pf, false);
            if (tindex != -1) OnCard(liveTo, to, tindex, pt, false);
            return;
        }
        
        if (bagSwap)
        {
            //当from比to大的时候，to会先装备需要减少1，反之则加tindex
            if (findex > tindex) findex--;
            else tindex++;
        }
        bool okTo = OnCard(liveFrom, to, findex, pf, false);
        bool okFrom = OnCard(liveTo, from, tindex, pt, false);
        //产生问题，还原内容
        if (okFrom == false || okTo == false)
        {
            Debug.LogError("OnCard产生错误");
            if (okTo == true) OffCard(liveFrom, to);
            if (okFrom == true) OffCard(liveTo, from);
            OnCard(liveFrom, from, findex, pf, false);
            OnCard(liveTo, to, tindex, pt, false);
        }
        GoodSelectUI.RefreshAll();
        BoxUI.RefreshAll();
    }
}
