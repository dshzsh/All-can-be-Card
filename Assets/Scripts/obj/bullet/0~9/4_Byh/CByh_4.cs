using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CByh_4 : Cbullet_10
{

}

public class SByh_4 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CByh_4 card = _card as CByh_4;
        MsgCollision msg = _msg as MsgCollision;

    }
}