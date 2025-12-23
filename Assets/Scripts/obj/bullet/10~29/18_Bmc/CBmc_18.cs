using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBmc_18 : Cbullet_10
{

}

public class SBmc_18 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CBmc_18 card = _card as CBmc_18;
        MsgCollision msg = _msg as MsgCollision;

    }
}