using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGjdyz_51 : Citem_33
{
    public BasicAtt added = new BasicAtt();
}
public class DGjdyz_51 : DataBase
{
    public float damage, upRate;
}
public class SGjdyz_51 : Sitem_33
{
    public int bid;
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamageAfter, GiveDamageAfter);
        bid = GetTypeId(typeof(CFjdyz_20));
    }
    void GiveDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGjdyz_51 card = _card as CGjdyz_51;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGjdyz_51 config = basicConfig as DGjdyz_51;

        if (Sbuff_35.GetBuff(msg.to, bid) != null)
            return;
        Sbuff_35.GiveBuff(msg.from, msg.to, new MsgBeBuff(CreateCard<CFjdyz_20>(), Sbuff_35.BuffSpeTime.Inf));

        float damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.healthMax) * card.pow;
        MsgBeDamage bmsg = new MsgBeDamage(damage, SGRwx_4.WxTag.mu);
        Shealth_4.GiveDamage(msg.from, msg.to, bmsg);
        float healthUp = bmsg.damage * config.upRate;
        SGby_50.AddPermanentAtt(card, new AttAndRevise(BasicAttID.healthMax, new BasicAtt(healthUp)));
        card.added.BeOnAtt(new BasicAtt(healthUp), 1);
    }
}