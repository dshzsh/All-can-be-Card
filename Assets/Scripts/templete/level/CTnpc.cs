using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTnpc : CardBase
{

}

public class STnpc : SystemBase
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeInteract, BeInteract);
    }
    void BeInteract(CardBase _card, MsgBase _msg)
    {
        CTnpc card = _card as CTnpc;
        MsgBeInteract msg = _msg as MsgBeInteract;

        Debug.Log(msg.live);
    }
}