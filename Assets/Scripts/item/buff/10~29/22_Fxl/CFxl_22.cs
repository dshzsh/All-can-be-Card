using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFxl_22 : Cbuffbase_36
{
    public float exDownSpeed;
    public float rangeUp, damageUp;
    public MsgBullet bmsg;
}
public class DFxl_22 : DataBase
{

}
public class SFxl_22 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Collision, Collision);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFxl_22 card = _card as CFxl_22;
        MsgOnItem msg = _msg as MsgOnItem;

        if (!TryGetClive(card, out var clive)) return;

        clive.mustMoveVelocity += msg.op * card.exDownSpeed * Vector3.down;

        if(msg.op==1 && clive.obj.OnGround())
        {
            MsgCollision cmsg = new MsgCollision();
            cmsg.hitPos = card.bmsg.initPos;
            Collision(_card, cmsg);
        }
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        CFxl_22 card = _card as CFxl_22;
        MsgCollision msg = _msg as MsgCollision;

        float height = card.bmsg.initPos.y - msg.hitPos.y;
        if (height < 0) height = 0;
        card.bmsg.damage *= 1 + height * card.damageUp;
        card.bmsg.AddExScale(height * card.rangeUp);
        card.bmsg.initPos = msg.hitPos;
        Sbullet_10.GiveBullet(card.bmsg);
        DestroyCard(card);
    }
}