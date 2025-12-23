using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGyg_46 : Citem_33
{
    public float recordDamage = 0;
    public int time = 0;
}
public class DGyg_46 : DataBase
{
    public int interval;
    public float healRate;
}
public class SGyg_46 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeDamageAfter, BeDamageAfter);
        AddHandle(MsgType.UpdateSec, UpdateSec);
    }
    void BeDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGyg_46 card = _card as CGyg_46;
        MsgBeDamage msg = _msg as MsgBeDamage;

        card.recordDamage += msg.damage;
    }
    void UpdateSec(CardBase _card, MsgBase _msg)
    {
        CGyg_46 card = _card as CGyg_46;
        MsgUpdate msg = _msg as MsgUpdate;
        DGyg_46 config = basicConfig as DGyg_46;
        
        if(MyTool.IntervalTimeSec(config.interval, ref card.time))
        {
            CardBase live = GetTop(card);
            Shealth_4.GiveHeal(live, live, new MsgBeHeal(card.recordDamage * config.healRate * card.pow));
            card.recordDamage = 0;
        }
    }
}