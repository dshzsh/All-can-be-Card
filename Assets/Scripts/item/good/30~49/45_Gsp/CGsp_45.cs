using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGsp_45 : Citem_33
{

}
public class DGsp_45 : DataBase
{
    public float minDamage;
}
public class SGsp_45 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamage, GiveDamage, HandlerPriority.Lowest);
    }
    void GiveDamage(CardBase _card, MsgBase _msg)
    {
        CGsp_45 card = _card as CGsp_45;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGsp_45 config = basicConfig as DGsp_45;

        float minDamage = config.minDamage * Shealth_4.GetAttf(msg.from, BasicAttID.atk) * card.pow;
        if (msg.damage < minDamage)
            msg.damage = minDamage;

    }
}