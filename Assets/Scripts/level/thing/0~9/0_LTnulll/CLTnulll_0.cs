using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLTnulll_0 : CObj_2
{

}

public class SLTnulll_0 : SObj_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CLTnulll_0 card = _card as CLTnulll_0;
        MsgCollision msg = _msg as MsgCollision;

    }
}