using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Ccoldam_5 : CardBase
{
    public float damage;
}
public class MsgKillLive : MsgBase
{
    public CardBase live;
    public MsgKillLive() { }
    public MsgKillLive(CardBase live)
    {
        this.live = live;
    }
}
public class Scoldam_5 : SystemBase
{
    public static void GiveBulletDamage(CardBase card, MsgCollision msg, MsgBeDamage dmsg, bool useGiveDamage = true)
    {
        if (msg.other == null) return;

        if (TryGetCobj(card, out var obj1) && TryGetCobj(msg.other, out var obj2))
        {
            if (Slive_19.TeamSatisfy(obj1.team, obj2.team, Slive_19.FindLiveMode.friend)) return;
        }

        dmsg.damagePos = msg.hitPos;
        dmsg.from = Sysw_26.GetYsw(card);
        dmsg.to = msg.other;

        // 给子弹使用的伤害信息
        if (useGiveDamage)
            SendMsg(msg.cobj, MsgType.GiveDamageBefore, dmsg);

        Shealth_4.GiveDamage(dmsg.from, msg.other, dmsg);

        if (useGiveDamage)
            SendMsg(msg.cobj, MsgType.GiveDamageAfter, dmsg);

        if (msg.other != null) { msg.hit = true; }
        if (dmsg.isKill)
        {
            SendMsg(msg.cobj, MsgType.KillLive, new MsgKillLive(msg.other));
            SendMsg(dmsg.from, MsgType.KillLive, new MsgKillLive(msg.other));
        }
    }
    public override void Init()
    {
        AddHandle(MsgType.Collision, Collision);
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        Ccoldam_5 card = _card as Ccoldam_5;
        MsgCollision msg = _msg as MsgCollision;

        float pow = Sbullet_10.GetBulletPow(card);
        MsgBeDamage dmsg = new MsgBeDamage(card.damage * pow);
        GiveBulletDamage(card, msg, dmsg);
    }
}