using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Unity.VisualScripting;

public class CGCjtqh_10 : CardBase
{
    public float force;
    public float time;
}
public class SGCjtqh_10 : SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    
    void Collision(CardBase _card, MsgBase _msg)
    {
        CGCjtqh_10 card = _card as CGCjtqh_10;
        MsgCollision msg = _msg as MsgCollision;

        Vector3 dir = Vector3.zero;
        float exPow = Sbullet_10.GetBulletPow(card);
        if (TryGetCobj(card, out var cObj,true))
        {
            dir = cObj.obj.rbody.velocity.normalized;
        }

        if (msg.robj == null) return;
        if (msg.other != null && msg.hit == false) return;

        MyPhysics.GiveImpulseForce(msg.robj, dir, card.force * exPow, msg.hitPos);
    }
}