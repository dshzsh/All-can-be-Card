using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGtxtz_31 : Citem_33
{
    public float record = 0;
    public float healRecordPerSec = 0f;

    public float time;
}
public class DGtxtz_31 : DataBase
{
    public float recordRate;
    public float timeHeal;
    public float interval;
}
public class SGtxtz_31 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.BeDamageAfter, BeDamageAfter);
    }
    void BeDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGtxtz_31 card = _card as CGtxtz_31;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGtxtz_31 config = basicConfig as DGtxtz_31;

        card.record += msg.damage * config.recordRate;
        card.healRecordPerSec = card.record / config.timeHeal;
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGtxtz_31 card = _card as CGtxtz_31;
        MsgUpdate msg = _msg as MsgUpdate;
        DGtxtz_31 config = basicConfig as DGtxtz_31;

        if (!MyTool.IntervalTime(config.interval, ref card.time, msg.time)) return;
        if (card.record <= 0) return;

        float heal = card.healRecordPerSec * config.interval;
        Shealth_4.GiveHeal(GetTop(card), GetTop(card), new MsgBeHeal(heal));
        card.record -= heal;
    }
}