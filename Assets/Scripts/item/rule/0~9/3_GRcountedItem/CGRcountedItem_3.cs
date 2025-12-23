using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SGqb_24;
using static SystemManager;

public class CGRcountedItem_3 : CGRbase_1
{
    public int itemId = 0;
    public float coin = 0;
}
public class DGRcountedItem_3 : DataBase
{

}
public class SGRcountedItem_3 : SGRbase_1
{
    public static float GetItemCnt(CardBase card, int itemId)
    {
        if (!TryGetCobj(card, out var live)) return 0f;
        MsgGetItemCnt msg = new(itemId);
        SendMsg(live, mTGetItemCnt, msg);
        return msg.coin;
    }
    public static bool CostItem(CardBase card, int itemId, float coinCost)
    {
        if (!TryGetCobj(card, out var live)) return false;
        MsgCostItem msg = new(itemId, coinCost);
        SendMsg(live, mTCostItem, msg);
        return msg.ok;
    }
    public static void GetItem(CardBase card, int itemId, float coinAdd)
    {
        if (!TryGetCobj(card, out var live)) return;
        MsgGetItem msg = new(itemId, coinAdd);
        SendMsg(live, mTGetItem, msg);
        if (msg.ok == false)
        {
            return;
        }
    }
    public static int mTGetItemCnt = MsgType.ParseMsgType(CardField.rule, 3, 0);
    public class MsgGetItemCnt : MsgBase
    {
        public int itemID = 0;
        public float coin = 0;
        public MsgGetItemCnt(int itemID)
        {
            this.itemID = itemID;
        }
    }
    public static int mTCostItem = MsgType.ParseMsgType(CardField.rule, 3, 1);
    public class MsgCostItem : MsgBase
    {
        public int itemID = 0;
        public float coin = 0;
        public bool ok = false;
        public MsgCostItem(int itemID, float coin)
        {
            this.itemID = itemID;
            this.coin = coin;
        }
    }
    public static int mTGetItem = MsgType.ParseMsgType(CardField.rule, 3, 2);
    public class MsgGetItem : MsgBase
    {
        public int itemID = 0;
        public float coinAdd = 0;
        public bool ok = false;
        public MsgGetItem(int itemID, float coinAdd)
        {
            this.itemID = itemID;
            this.coinAdd = coinAdd;
        }
    }
    public override void Init()
    {
        base.Init();
        AddHandle(mTGetItemCnt, GetCoinCnt);
        AddHandle(mTCostItem, CostCoin);
        AddHandle(mTGetItem, GetCoin);
    }
    void GetCoinCnt(CardBase _card, MsgBase _msg)
    {
        CGRcountedItem_3 card = _card as CGRcountedItem_3;
        MsgGetItemCnt msg = _msg as MsgGetItemCnt;

        if (msg.itemID != card.itemId) return;

        msg.coin = card.coin;
    }
    void CostCoin(CardBase _card, MsgBase _msg)
    {
        CGRcountedItem_3 card = _card as CGRcountedItem_3;
        MsgCostItem msg = _msg as MsgCostItem;

        if (msg.itemID != card.itemId) return;

        if (card.coin < msg.coin)
        {
            msg.ok = false;
            return;
        }
        card.coin -= msg.coin;
        msg.ok = true;
    }
    void GetCoin(CardBase _card, MsgBase _msg)
    {
        CGRcountedItem_3 card = _card as CGRcountedItem_3;
        MsgGetItem msg = _msg as MsgGetItem;

        if (msg.itemID != card.itemId) return;

        card.coin += msg.coinAdd;
        msg.ok = true;
    }
}