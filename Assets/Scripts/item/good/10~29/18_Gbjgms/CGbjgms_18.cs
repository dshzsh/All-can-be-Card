using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGbjgms_18 : Citem_33
{

}
public class DGbjgms_18 : DataBase
{
    public float critDamRate;
}
public class SGbjgms_18 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamageBefore, GiveDamageBefore);
    }
    void GiveDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGbjgms_18 card = _card as CGbjgms_18;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGbjgms_18 config = basicConfig as DGbjgms_18;

        // 产生暴击
        if (MyRandom.RandPer(Shealth_4.GetAttf(card, BasicAttID.crit)))
        {
            SGAcrit_11.GiveCrit(card, msg, Shealth_4.GetAttf(card, BasicAttID.critDam) * config.critDamRate);
        }
    }
}