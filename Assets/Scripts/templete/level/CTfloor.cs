using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTfloor : CfloorBase_2
{

}

public class STfloor : SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CTfloor card = _card as CTfloor;
        MsgFloorStart msg = _msg as MsgFloorStart;


    }
}