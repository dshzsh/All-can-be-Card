using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFqs_9 : CfloorBase_2
{

}

public class SLFqs_9: SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CfloorBase_2 card = _card as CfloorBase_2;

        GiveNextCsm(card);
    }
}