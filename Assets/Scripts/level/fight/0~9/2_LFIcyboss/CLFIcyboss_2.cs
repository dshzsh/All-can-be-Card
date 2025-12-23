using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLFIcyboss_2 : CLFfightBase_6
{

}

public class SLFIcyboss_2 : SLFfightBase_6
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLFIcyboss_2 card = _card as CLFIcyboss_2;
        MsgOnItem msg = _msg as MsgOnItem;


    }
}
