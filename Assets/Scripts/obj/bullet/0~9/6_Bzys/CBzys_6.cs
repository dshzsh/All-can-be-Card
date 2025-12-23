using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBzys_6 : Cbullet_10
{

}

public class SBzys_6 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CBzys_6 card = _card as CBzys_6;
        MsgCollision msg = _msg as MsgCollision;

    }
}