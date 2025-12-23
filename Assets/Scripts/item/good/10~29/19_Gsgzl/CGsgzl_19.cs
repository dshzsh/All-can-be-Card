using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGsgzl_19 : Citem_33
{

}
public class DGsgzl_19 : DataBase
{
    public float critDamRate;
}
public class SGsgzl_19 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveHeal, GiveHeal);
    }
    void GiveHeal(CardBase _card, MsgBase _msg)
    {
        CGsgzl_19 card = _card as CGsgzl_19;
        MsgBeHeal msg = _msg as MsgBeHeal;
        DGsgzl_19 config = basicConfig as DGsgzl_19;

        if (MyRandom.RandPer(Shealth_4.GetAttf(card, BasicAttID.crit)))
        {
            float dam = Shealth_4.GetAttf(card, BasicAttID.critDam) * config.critDamRate;
            msg.AddStr("C");
            msg.heal *= 1 + dam;
        }
    }
}