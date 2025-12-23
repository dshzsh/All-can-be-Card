using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Ccolheal_39 : CardBase
{
    public float heal;
}

public class Scolheal_39: SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Collision, Collision);
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        Ccolheal_39 card = _card as Ccolheal_39;
        MsgCollision msg = _msg as MsgCollision;

        if (msg.other == null) return;

        if (TryGetCobj(card, out var obj1) && TryGetCobj(msg.other, out var obj2))
        {
            if (obj1.team != obj2.team) return;
        }
        float pow = 1f;
        if(obj1 is Cbullet_10 cbullet)
        {
            pow = cbullet.bulletPow;
        }

        MsgBeHeal dmsg = new MsgBeHeal(card.heal * pow);//Debug.Log(dmsg.heal);
        dmsg.healPos = msg.hitPos;
        Shealth_4.GiveHeal(Sysw_26.GetYsw(card), msg.other, dmsg);
        if (msg.other != null) { msg.hit = true; }
    }
}