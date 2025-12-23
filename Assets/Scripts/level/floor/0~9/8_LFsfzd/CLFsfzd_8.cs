using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFsfzd_8 : CfloorBase_2
{

}

public class SLFsfzd_8: SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CLFsfzd_8 card = _card as CLFsfzd_8;

        GiveNextCsm(card);
    }
}