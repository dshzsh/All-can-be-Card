using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBzlb_10 : Cbullet_10
{
    public CMzlb_17 magic;
}

public class SBzlb_10 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
        AddHandle(MsgType.BulletStart, BulletStart);
    }
    public void BulletStart(CardBase _card, MsgBase _msg)
    {
        CBzlb_10 card = _card as CBzlb_10;
        MsgBulletCreate msg = _msg as MsgBulletCreate;

        if (msg.msg?.fromMagicUse?.magic is CMzlb_17 magic)
            card.magic = magic;
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CBzlb_10 card = _card as CBzlb_10;
        MsgCollision msg = _msg as MsgCollision;

        if(msg.hit)
        {
            if(CardValid(card.magic) && msg.robj != null)
            {
                SMzlb_17.AddLink(card.magic, new SMzlb_17.ConObj() { rbody = msg.robj, cobj = msg.other });
                Smagic_14.RecoverMagicCd(card.magic);
            }
        }
    }
}