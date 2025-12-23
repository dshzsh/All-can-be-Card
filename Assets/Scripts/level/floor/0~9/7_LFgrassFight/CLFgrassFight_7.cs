using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFgrassFight_7 : CLFfightBase_6
{

}

public class SLFgrassFight_7: SLFfightBase_6
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLFgrassFight_7 card = _card as CLFgrassFight_7;
        MsgOnItem msg = _msg as MsgOnItem;


    }
}