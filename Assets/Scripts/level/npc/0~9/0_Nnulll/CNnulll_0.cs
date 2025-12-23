using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CNnulll_0 : CardBase
{

}

public class SNnulll_0 : SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.BeInteract, BeInteract);
    }
    void BeInteract(CardBase _card, MsgBase _msg)
    {
        CNnulll_0 card = _card as CNnulll_0;
        MsgBeInteract msg = _msg as MsgBeInteract;

        Debug.Log(msg.live);
    }
}