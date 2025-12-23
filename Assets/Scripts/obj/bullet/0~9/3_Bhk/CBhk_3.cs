using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBhk_3 : Cbullet_10
{

}

public class SBhk_3 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    public void Update(CardBase _card, MsgBase _msg)
    {
        CBhk_3 card = _card as CBhk_3;
        //Debug.Log(cardAbandon.obj.transform.position);
        //Debug.Log("v:" + cardAbandon.obj.rbody.velocity);
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CBhk_3 card = _card as CBhk_3;
        MsgCollision msg = _msg as MsgCollision;

    }
}