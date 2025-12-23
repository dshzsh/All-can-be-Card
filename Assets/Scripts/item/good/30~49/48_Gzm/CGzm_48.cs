using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGzm_48 : Citem_33
{

}
public class DGzm_48 : DataBase
{
    public float damagePer;
}
public class SGzm_48 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeDamageBefore, BeDamageBefore);
    }
    void BeDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGzm_48 card = _card as CGzm_48;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGzm_48 config = basicConfig as DGzm_48;

        if(SGbc_47.IsBackstab(msg.from, msg.to) == -1)
        {
            msg.damage *= 1 - config.damagePer * card.pow;
        }
    }
}