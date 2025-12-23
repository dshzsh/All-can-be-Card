using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTfight : CLFfightBase_6
{

}

public class STfight : SLFfightBase_6
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTfight card = _card as CTfight;
        MsgOnItem msg = _msg as MsgOnItem;


    }
}
