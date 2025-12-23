using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCjs_30 : CGqhsbase_11
{
    public float damageRate;
    public float range;
}
public class DGQCjs_30 : DataBase
{

}
public class SGQCjs_30 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        CGQCjs_30 card = _card as CGQCjs_30;
        MsgCollision msg = _msg as MsgCollision;

        if (!msg.hit || msg.other == null) return;

        Cbullet_10 bullet = GetTop(card) as Cbullet_10;
        if (bullet == null) return;

        MsgBullet bmsg = new MsgBullet(bullet, false);
        bmsg.initPos = msg.other.obj.Center;
        //Debug.Log(msg.other.obj.Center);
        bmsg.bulletPow *= card.damageRate;
        bmsg.dir = MyMath.V3zeroYNor(bmsg.dir);
        bmsg.baseScale *= card.range * 2;
        
        Sbullet_10.GiveBullet(SBjs_19.bid, bmsg);
    }
}