using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CGcfqh_2 : CGqhsbase_11
{
    [NoActiveCard]
    public CardBase magic;
}
public class DGcfqh_2 : DataBase
{
    public float exPow;
}
public class SGcfqh_2 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyMagicEnd, MagicEnd);
        AddHandle(MsgType.SelfContainerGet, MyContainerGet);
        AddHandle(MsgType.SelfContainerJudge, MyContainerJudge);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGcfqh_2 card = _card as CGcfqh_2;
        card.magic = ContainedCnull(card);
    }
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        CGcfqh_2 card = _card as CGcfqh_2;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if(msg.gmsg.op==1)
        {
            if (Sitem_33.IsMagic(msg.gmsg.item))
            {
                msg.ok = true;
                return;
            }
            msg.ok = false;
            msg.AddMsg("只能装备魔法");
        }
        else
        {
            msg.ok = card.magic == msg.gmsg.item;
            msg.gmsg.pos = 0;
        }
        
    }
    void MyContainerGet(CardBase _card, MsgBase _msg)
    {
        CGcfqh_2 card = _card as CGcfqh_2;
        MsgGetItem msg = _msg as MsgGetItem;

        if (msg.op == 1)
        {
            msg.pos = 0;
            msg.item.container = card;
            card.magic = msg.item;
        }
        else
        {
            if (msg.item == card.magic)
            {
                msg.pos = 0;
                msg.item.container = null;
                card.magic = new CNull_0();
                card.magic.container = card;
            }
            else msg.pos = -1;
        }
    }
    void MagicEnd(CardBase _card, MsgBase _msg)
    {
        CGcfqh_2 card = _card as CGcfqh_2;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGcfqh_2 config = basicConfig as DGcfqh_2;

        if (msg.magic == null || msg.magic != Sqhc_38.GetQhMagic(card)) return;
        if (card.magic == null) return;

        MsgMagicUse nmsg = new MsgMagicUse(msg.live, NewCopy(card.magic), msg.pos);
        nmsg.costKey = msg.costKey;
        nmsg.costLive = msg.costLive;
        nmsg.pow *= card.pow * config.exPow;

        if (msg.isNoCost)
            nmsg.ToNoCost();

        Smagic_14.UseMagicEntirely(nmsg);
    }
}