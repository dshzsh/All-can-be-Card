using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFjlp_21 : Cbuffbase_36
{
    public float damageRate;
    public float delay = 0;
}
public class DFjlp_21 : DataBase
{

}
public class SFjlp_21 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        CFjlp_21 card = _card as CFjlp_21;
        MsgCollision msg = _msg as MsgCollision;

        if (!msg.hit || msg.other == null) return;

        Cbullet_10 bullet = GetTop(card) as Cbullet_10;
        if (bullet == null) return;

        MsgBullet bmsg = new MsgBullet(bullet, true, id);
        bmsg.AddWx(SGRwx_4.WxTag.jin);

        bmsg.initPos = msg.other.obj.Center;
        bmsg.damage *= card.damageRate;
        bmsg.dir = MyMath.V3zeroYNor(bmsg.dir);

        Sbullet_10.GiveBullet(SBlj_2.bid, bmsg, card.delay);
    }
}