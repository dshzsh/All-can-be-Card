using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGfzq_22 : Citem_33
{
    [NoActiveCard]
    public CardBase item;
}
public class DGfzq_22 : DataBase
{
    public int copyCnt;
}
public class SGfzq_22 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.InteractItem, InteractItem);
        AddHandle(MsgType.SelfContainerGet, MyContainerGet);
        AddHandle(MsgType.SelfContainerJudge, MyContainerJudge);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGfzq_22 card = _card as CGfzq_22;

        card.item = ContainedCnull(card);
    }
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        CGfzq_22 card = _card as CGfzq_22;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (msg.gmsg.op == 1)
        {
            if(!(msg.gmsg.item is Citem_33 item))
            {
                msg.ok = false;
                msg.AddMsg("只能复制物品");
                return;
            }
            if(item.pow>card.pow)
            {
                msg.ok = false;
                msg.AddMsg("复制物品威力超过此道具");
                return;
            }
            msg.ok = true;
        }
        else
        {
            msg.ok = card.item == msg.gmsg.item;
            msg.gmsg.pos = 0;
        }

    }
    void MyContainerGet(CardBase _card, MsgBase _msg)
    {
        CGfzq_22 card = _card as CGfzq_22;
        MsgGetItem msg = _msg as MsgGetItem;

        if (msg.op == 1)
        {
            msg.pos = 0;
            msg.item.container = card;
            card.item = msg.item;
        }
        else
        {
            if (msg.item == card.item)
            {
                msg.pos = 0;
                msg.item.container = null;
                card.item = new CNull_0();
                card.item.container = card;
            }
            else msg.pos = -1;
        }
    }
    void InteractItem(CardBase _card, MsgBase _msg)
    {
        CGfzq_22 card = _card as CGfzq_22;
        MsgInteractItem msg = _msg as MsgInteractItem;
        DGfzq_22 config = basicConfig as DGfzq_22;

        if(card.item.id==0)
        {
            UIBasic.GiveErrorText("不复制空的物体！");
            return;
        }
        for (int i = 0; i < config.copyCnt; i++)
            Sbag_40.LiveGetItem(card, NewCopy(card.item));
        DestroyCard(card);
    }
}