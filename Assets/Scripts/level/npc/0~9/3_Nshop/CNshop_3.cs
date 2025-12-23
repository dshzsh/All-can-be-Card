using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CNshop_3 : CObj_2
{
    public CGbox_26 box;

    public bool used = false;
    public int initCost = 0;
    public int cost = 0;
}

public class SNshop_3 : SObj_2
{
    public static int mTShopStart = MsgType.ParseMsgType(CardField.npc, 3, 0);
    public class MsgShopStart:MsgBase
    {
        public CNshop_3 shop;
        public MsgShopStart() { }
        public MsgShopStart(CNshop_3 shop)
        {
            this.shop = shop;
        }
    }

    private static float GetCost(CardBase item)
    {
        return SGqb_24.ItemValue(item) * Mathf.Max(1, Mathf.Log10(1 + SCmap_45.mainMap.mapCnt));
    }
    public static void SetShop(CNshop_3 card, CardBase item, int cost = -1)
    {
        if (cost == -1) cost = ((int)GetCost(item));

        SendMsg(card.box, MsgType.MyContainerGet, new MsgGetItem(item, 1, 0, card.box));
        card.cost = cost;
        card.initCost = cost;

        SendMsgToPlayer(mTShopStart, new MsgShopStart(card));

        ShowShop(card);
    }
    public static void ShowShop(CNshop_3 card)
    {
        if (!InWorld(card)) return;

        if(card.used)
        {
            card.obj.gameObject.GetComponent<MNshop_3_obj>().SetClear();
            return;
        }
        string costText;

        if (card.initCost == card.cost)
            costText = card.cost.ToString() + "￥";
        else
            costText = $"<color=#927500><s>{card.initCost}￥</s></color>\n<color=#47FF00>{card.cost}￥</color>";

        card.obj.gameObject.GetComponent<MNshop_3_obj>().Set(card.box.items[0], costText);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.BeInteract, BeInteract);
        AddHandle(MsgType.MyInteractItem, MyInteractItem);

        AddHandle(MsgType.SelfContainerJudge, SelfContainerJudge);
    }
    public override void Create(CardBase _card)
    {
        CNshop_3 card = _card as CNshop_3;
        card.box = SGbox_26.BoxWithSlot(1);
        ActiveComponent(card, card.box);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CNshop_3 card = _card as CNshop_3;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op == 1)
        {
            ShowShop(card);
        }
    }
    void MyInteractItem(CardBase _card, MsgBase _msg)
    {
        CNshop_3 card = _card as CNshop_3;
        MsgInteractItem msg = _msg as MsgInteractItem;

        if (card.used) return;

        if(!SGqb_24.CostCoin(msg.live, card.cost))
        {
            UIBasic.GiveErrorText("金币不足！");
            msg.used = false;
            return;
        }

        for(int i=0;i<card.box.items.Count;i++)
        {
            CardBase item = card.box.items[i];
            SendMsg(card.box, MsgType.MyContainerGet, new MsgGetItem(item, -1, -1, card.box));
            Sbag_40.LiveGetItem(msg.live, item);
        }

        ShowShop(card);
        card.used = true;
    }
    void SelfContainerJudge(CardBase _card, MsgBase _msg)
    {
        CMOdzt_5 card = _card as CMOdzt_5;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        msg.ok = false;
        msg.AddMsg("不能这么做");
    }

    
    public void BeInteract(CardBase _card, MsgBase _msg)
    {
        CNshop_3 card = _card as CNshop_3;
        MsgBeInteract msg = _msg as MsgBeInteract;

        if (card.used) return;

        UIBasic.GiveUI(DMOdzt_5.forgeUIPrefab).GetComponent<MInteractBoxUI>()
            .SetWithUse(msg.live, card,
            $"耗费{card.cost}金币购买物品",
            card.used, "已购买", "购买");
    }
}