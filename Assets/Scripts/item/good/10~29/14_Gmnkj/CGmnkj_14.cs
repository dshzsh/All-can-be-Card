using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGmnkj_14 : Citem_33
{

}
public class DGmnkj_14 : DataBase
{
    public float defRate;
}
public class SGmnkj_14 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeDamage, BeDamage, HandlerPriority.Before);
    }
    void BeDamage(CardBase _card, MsgBase _msg)
    {
        CGmnkj_14 card = _card as CGmnkj_14;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGmnkj_14 config = basicConfig as DGmnkj_14;

        float reduce = msg.damage * config.defRate * card.pow;
        MsgCostMana cmsg = new MsgCostMana(reduce);
        SendMsg(GetTop(card), MsgType.CostMana, cmsg);
        if(cmsg.ok)
        {
            msg.damage -= reduce;
        }
    }
}