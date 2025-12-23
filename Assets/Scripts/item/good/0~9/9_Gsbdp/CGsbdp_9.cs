using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGsbdp_9 : Citem_33
{

}
public class DGsbdp_9 : DataBase
{
    public float missRate;
    public float missMax;
}
public class SGsbdp_9 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeDamageBefore, BeDamageBefore);
    }
    void BeDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGsbdp_9 card = _card as CGsbdp_9;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGsbdp_9 config = basicConfig as DGsbdp_9;

        float miss = SGAluck_13.LuckedOdds(card, config.missRate) * card.pow;
        if (miss > config.missMax) miss = config.missMax;
        if (MyRandom.RandPer(miss))
        {
            msg.damage = 0;
            msg.AddStr("M");
        }
    }
}