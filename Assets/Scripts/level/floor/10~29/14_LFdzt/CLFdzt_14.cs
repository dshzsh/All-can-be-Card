using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFdzt_14 : CfloorBase_2
{

}

public class SLFdzt_14 : SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CLFdzt_14 card = _card as CLFdzt_14;
        MsgFloorStart msg = _msg as MsgFloorStart;

        GiveNextCsm(card);
    }
}