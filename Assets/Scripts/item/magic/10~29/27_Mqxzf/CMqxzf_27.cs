using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Unity.VisualScripting.FullSerializer;

public class CMqxzf_27 : Cmagicbase_17
{
    public CardBase good;
}
public class DMqxzf_27 : DataBase
{
    public BasicAtt exPow;
}
public class SMqxzf_27 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem, HandlerPriority.Before);
        AddHandle(MsgType.SelfContainerGet, MyContainerGet);
        AddHandle(MsgType.SelfContainerJudge, MyContainerJudge);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CMqxzf_27 card = _card as CMqxzf_27;
        card.good = new CNull_0();
        card.good.container = card;
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CMqxzf_27 card = _card as CMqxzf_27;
        MsgOnItem msg = _msg as MsgOnItem;
        DMqxzf_27 config = basicConfig as DMqxzf_27;

        if (card.good is Citem_33 item)
            item.pow = config.exPow.WithPow(card.pow).UseAttTo(item.pow, msg.op);
        
    }
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        CMqxzf_27 card = _card as CMqxzf_27;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (card != msg.gmsg.container) return;

        if (msg.gmsg.op == 1)
        {
            if (Sitem_33.IsGood(msg.gmsg.item))
            {
                msg.ok = true;
                return;
            }
            msg.ok = false;
            msg.AddMsg("只能装备道具");
        }
        else
        {
            msg.ok = card.good == msg.gmsg.item;
            msg.gmsg.pos = 0;
        }

    }
    void MyContainerGet(CardBase _card, MsgBase _msg)
    {
        CMqxzf_27 card = _card as CMqxzf_27;
        MsgGetItem msg = _msg as MsgGetItem;
        DMqxzf_27 config = basicConfig as DMqxzf_27;

        if (card != msg.container) return;

        if (msg.op == 1)
        {
            msg.pos = 0;
            msg.item.container = card;
            card.good = msg.item;

            if (InWorld(card) && card.good is Citem_33 item)
                item.pow = config.exPow.WithPow(card.pow).UseAttTo(item.pow, 1);

            ActiveComponent(card, card.good);
        }
        else
        {
            if (msg.item != card.good)
            {
                msg.pos = -1;
                return;
            }

            InactiveComponent(card, card.good);

            if (InWorld(card) && card.good is Citem_33 item)
                item.pow = config.exPow.WithPow(card.pow).UseAttTo(item.pow, -1);

            msg.pos = 0;
            msg.item.container = null;
            card.good = new CNull_0();
            card.good.container = card;
        }
    }
}