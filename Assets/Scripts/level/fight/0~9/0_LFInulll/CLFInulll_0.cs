using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLFInulll_0 : CLFfightBase_6
{

}

public class SLFInulll_0 : SLFfightBase_6
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLFInulll_0 card = _card as CLFInulll_0;
        MsgOnItem msg = _msg as MsgOnItem;


    }
}
