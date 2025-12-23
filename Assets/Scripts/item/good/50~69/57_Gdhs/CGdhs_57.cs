using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGdhs_57 : Citem_33
{
    public float time;
}
public class DGdhs_57 : DataBase
{
    public float interval, fireTime, fireDamage;
}
public class SGdhs_57 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.GiveDamageAfter, GiveDamageAfter);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGdhs_57 card = _card as CGdhs_57;
        MsgUpdate msg = _msg as MsgUpdate;
        DGdhs_57 config = basicConfig as DGdhs_57;

        card.time += msg.time;
        if (card.time > config.interval) card.time = config.interval;
    }
    void GiveDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGdhs_57 card = _card as CGdhs_57;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGdhs_57 config = basicConfig as DGdhs_57;

        if (card.time + MyMath.SmallFloat < config.interval) return;
        card.time = 0;

        CFzs_1 buff = CreateCard<CFzs_1>();
        buff.pow = card.pow * config.fireDamage * Shealth_4.GetAttf(card, BasicAttID.atk);
        Sbuff_35.GiveBuff(msg.from, msg.to, new MsgBeBuff(buff, config.fireTime, 0, Sbuff_35.BeBuffMode.coverByBig));
    }
}