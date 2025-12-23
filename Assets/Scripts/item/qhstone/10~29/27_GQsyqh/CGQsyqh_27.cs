using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQsyqh_27 : CGqhsbase_11
{
    [NoActiveCard]
    public CardBase magic;
}
public class DGQsyqh_27 : DataBase
{
    public float exPow;
}
public class SGQsyqh_27 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
        AddHandle(MsgType.SelfContainerGet, MyContainerGet);
        AddHandle(MsgType.SelfContainerJudge, MyContainerJudge);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGQsyqh_27 card = _card as CGQsyqh_27;
        card.magic = ContainedCnull(card);
    }
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        CGQsyqh_27 card = _card as CGQsyqh_27;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (msg.gmsg.op == 1)
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
        CGQsyqh_27 card = _card as CGQsyqh_27;
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
                card.magic = ContainedCnull(card);
            }
            else msg.pos = -1;
        }
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQsyqh_27 card = _card as CGQsyqh_27;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQsyqh_27 config = basicConfig as DGQsyqh_27;

        MsgMagicUse nmsg = new MsgMagicUse(msg.live, NewCopy(card.magic), msg.pos, msg.mks);
        nmsg.key = Smagic_14.TempKey;
        nmsg.pow *= msg.pow * config.exPow;
        nmsg.mdata.windUp = nmsg.mdata.windDown = 0f;
        if (msg.isNoCost) nmsg.ToNoCost();

        Smagic_14.UseMagicEntirely(nmsg);
    }
}