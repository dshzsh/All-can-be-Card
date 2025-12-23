using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCycbp_57 : Cbuffbase_36
{
    public float radius;
}
public class DGQCycbp_57 : DataBase
{

}
public class SGQCycbp_57 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.TrueDie, TrueDie);
        AddHandle(MsgType.Collision, Collision, HandlerPriority.Highest);
    }
    void TrueDie(CardBase _card, MsgBase _msg)
    {
        CGQCycbp_57 card = _card as CGQCycbp_57;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj is not Cbullet_10 cbullet) return;

        card.buffInfo.time = -1f;

        MsgBullet bmsg = new MsgBullet(cbullet);
        bmsg.baseScale *= card.radius * 2;
        Sbullet_10.GiveBullet(SBbz_24.bid, bmsg);
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        CGQCycbp_57 card = _card as CGQCycbp_57;
        MsgCollision msg = _msg as MsgCollision;

        msg.valid = false;
    }
}