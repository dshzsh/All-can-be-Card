using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cqhc_38 : CardBase
{
    public List<CardBase> qhstones = new List<CardBase>();
}

public class Sqhc_38: SystemBase
{
    //better:加入装配强化石和卸下强化石的事件和响应
    //找到管理的最上层的物体，也就是被container包含的物体
    public static CardBase GetTopItem(CardBase card)
    {
        if (card == null) return card;

        do
        {
            if (card.container != null) return card;
            card = card.parent;
        }
        while (card != null && card.parent != null);

        return card;
    }
    public static Cqhc_38 QhcWithSlot(int slotCnt, params int[] vids)
    {
        Cqhc_38 qhc = CreateCard<Cqhc_38>();
        for (int i = 0; i < slotCnt; i++) 
        {
            CardBase cnull;
            if (i < vids.Length)
            {
                cnull = CreateCard(DataManager.VidToPid(vids[i], CardField.qhstone));
                AddComponent(cnull, CreateCard<CGQwfxx_15>());
            }
            else cnull = new CNull_0(); 
            qhc.qhstones.Add(cnull);
            cnull.container = qhc;
            ActiveComponent(qhc, cnull); 
        }
        return qhc;
    }
    public static Cqhc_38 QhcWithSlotCard(int slotCnt, params CardBase[] qhss)
    {
        Cqhc_38 qhc = CreateCard<Cqhc_38>();
        for (int i = 0; i < slotCnt; i++)
        {
            CardBase cnull;
            if (i < qhss.Length)
            {
                cnull = qhss[i];
            }
            else cnull = new CNull_0();
            qhc.qhstones.Add(cnull);
            cnull.container = qhc;
            ActiveComponent(qhc, cnull);
        }
        return qhc;
    }
    public static CardBase GetQhMagic(CardBase qhStone)
    {
        return qhStone.parent?.parent;
    }
    public static bool IsQhMagic(CardBase qhStone, CardBase magic)
    {
        return CardManager.IsParentCard(qhStone, magic);
    }
    public static bool IsQhStone(CardBase card)
    {
        return card is CGqhsbase_11;
    }
    public override void Init()
    {
        AddHandle(MsgType.ParseDescribe, ParseDescribe);
        AddHandle(MsgType.SelfContainerGet, MyContainerGet);
        AddHandle(MsgType.SelfContainerJudge, MyContainerJudge);
    }

    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        Cqhc_38 card = _card as Cqhc_38;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (card != msg.gmsg.container) return;

        if(msg.gmsg.op==1)
        {
            if (IsQhStone(msg.gmsg.item))
            {
                msg.ok = true;
                return;
            }
            msg.ok = false;
            msg.AddMsg("只能装备强化石");
        }
        else
        {
            msg.gmsg.pos = card.qhstones.IndexOf(msg.gmsg.item);
            msg.ok = msg.gmsg.pos != -1;
        }
        
    }
    void MyContainerGet(CardBase _card, MsgBase _msg)
    {
        Cqhc_38 card = _card as Cqhc_38;
        MsgGetItem msg = _msg as MsgGetItem;

        //Debug.Log(msg.op + " " + msg.pos + " " + msg.item);

        //刷新顶层物体的装配状态，只有在inworld的时候需要这么做
        CardBase topCard = null;
        CardBase topCardParent = null;
        bool needRefresh = false;
        if (msg.item.id != 0 && InWorld(card)) topCard = GetTopItem(card);
        if (topCard != null && topCard.parent != null) needRefresh = true;
        if (needRefresh)
        {
            topCardParent = topCard.parent;
            InactiveComponent(topCardParent, topCard);
        }

        if (msg.op == 1)
        {
            if (!(card.qhstones.Count > msg.pos && card.qhstones[msg.pos].id == 0))
            {
                msg.pos = -1;
            }

            if(msg.pos!=-1)
            {
                msg.item.container = card;
                card.qhstones[msg.pos] = msg.item;
                CardManager.ActiveComponent(card, msg.item, -msg.pos);
            }
        }
        else
        {
            msg.pos = card.qhstones.IndexOf(msg.item);
            //Debug.Log(msg.pos);
            if (msg.pos == -1) return;
            msg.item.container = null;
            CardManager.InactiveComponent(card, msg.item);
            CardBase cnull = new CNull_0();cnull.container = card;
            card.qhstones[msg.pos] = cnull;
        }

        if (needRefresh)
        {
            ActiveComponent(topCardParent, topCard, topCard.exPriority);
        }
    }
    void ParseDescribe(CardBase _card, MsgBase _msg)
    {
        Cqhc_38 card = _card as Cqhc_38;
        MsgParseDescribe msg = _msg as MsgParseDescribe;

        if (msg.card != card.parent) return;

        string qhcs = "强化石上限：" + card.qhstones.Count;//当前有几个槽就是上限
        foreach(CardBase com in card.qhstones)
        {
            qhcs += Cstr(com, true, true) + "";
        }
        msg.text = Sitem_33.InsertField(msg.text, "强化槽", Sitem_33.MaskColor.MK, qhcs);
    }
}