using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFdjf_5 : CfloorBase_2
{

}

public class SLFdjf_5: SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CLFdjf_5 card = _card as CLFdjf_5;

        GiveNextCsm(card);
    }
}