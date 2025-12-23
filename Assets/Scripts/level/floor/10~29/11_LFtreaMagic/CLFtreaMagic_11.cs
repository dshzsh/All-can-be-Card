using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFtreaMagic_11 : CfloorBase_2
{

}

public class SLFtreaMagic_11 : SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CLFtreaMagic_11 card = _card as CLFtreaMagic_11;
        MsgFloorStart msg = _msg as MsgFloorStart;

        GiveNextCsm(card);
    }
}