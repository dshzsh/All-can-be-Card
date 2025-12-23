using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGAcrit_11 : CGAattbase_1
{

}
public class SGAcrit_11 : SGAattbase_1
{
    public static int cirtTag;
    public override void Init()
    {
        cirtTag = id;
        AddHandle(MsgType.GiveDamageBefore, GiveDamageBefore);
    }
    public static void GiveCrit(CardBase card, MsgBeDamage msg, float critDam = -1)
    {
        msg.AddTag(CreateCard<CTTcrit_1>());
        msg.AddStr("C");
        if (critDam == -1) critDam = Shealth_4.GetAttf(card, BasicAttID.critDam);
        msg.damage *= 1 + critDam;
    }
    void GiveDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGAcrit_11 card = _card as CGAcrit_11;
        MsgBeDamage msg = _msg as MsgBeDamage;

        // 产生暴击
        if (MyRandom.RandPer(card.bvalue.GetValue()))
        {
            GiveCrit(card, msg);
        }
    }
}