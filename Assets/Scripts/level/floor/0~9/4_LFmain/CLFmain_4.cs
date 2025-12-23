using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFmain_4 : CfloorBase_2
{

}

public class SLFmain_4: SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLFmain_4 card = _card as CLFmain_4;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op == 1)
        {
            GiveNextCsm(card);
        }
    }
}