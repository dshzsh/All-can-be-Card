using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGbox_26 : Citem_33
{
    [NoActiveCard]
    public List<CardBase> items = new();
}
public class DGbox_26 : DataBase
{

}
public class SGbox_26 : Sitem_33
{
    public static CGbox_26 BoxWithSlot(int num)
    {
        CGbox_26 box = CreateCard<CGbox_26>();
        for(int i=0;i<num; i++)
        {
            box.items.Add(ContainedCnull(box));
        }
        return box;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyContainerGet, MyContainerGet);
        AddHandle(MsgType.MyContainerJudge, MyContainerJudge);
        AddHandle(MsgType.MyContainerAllItem, MyContainerAllItem);
    }
    private bool IsMbContainer(CardBase card, CardBase tarContainer)
    {
        if (card == tarContainer) return true;
        if (card.parent != null && card.parent == tarContainer) return true;
        return false;
    }
    
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        CGbox_26 card = _card as CGbox_26;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (!IsMbContainer(card, msg.gmsg.container))
            return;

        // 给父亲节点转发内容
        if (card.parent != null)
            SendMsg(card.parent, MsgType.SelfContainerJudge, msg);

        if (msg.ok == false) return;

        if (msg.gmsg.op == 1)
        {
            if (msg.gmsg.pos >= card.items.Count || msg.gmsg.pos < 0)
            {
                msg.ok = false;
                msg.AddMsg("不合理的位置！");
                return;
            }
        }
        else
        {
            msg.gmsg.pos = card.items.IndexOf(msg.gmsg.item);
            msg.ok = msg.gmsg.pos != -1;
        }

    }
    void MyContainerGet(CardBase _card, MsgBase _msg)
    {
        CGbox_26 card = _card as CGbox_26;
        MsgGetItem msg = _msg as MsgGetItem;

        if (!IsMbContainer(card, msg.container))
            return;

        if (msg.op == 1)
        {
            if (msg.item == null) return;

            card.items[msg.pos] = msg.item;
            msg.item.container = card;
        }
        else
        {
            msg.pos = card.items.IndexOf(msg.item);
            if (msg.pos == -1) return;

            msg.item.container = null;
            card.items[msg.pos] = ContainedCnull(card);
        }

        // 给父亲节点转发内容
        if (card.parent != null)
            SendMsg(card.parent, MsgType.SelfContainerGet, msg);
    }
    void MyContainerAllItem(CardBase _card, MsgBase _msg)
    {
        CGbox_26 card = _card as CGbox_26;
        MsgContainerAllItem msg = _msg as MsgContainerAllItem;

        if (!IsMbContainer(card, msg.container))
            return;

        msg.items.AddRange(card.items);
    }
}