using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLnulll_0 : CEbase_29
{

}
public class SLnulll_0 : SEbase_29
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLnulll_0 card = _card as CLnulll_0;
        MsgOnItem msg = _msg as MsgOnItem;

        Debug.Log(msg.op);
    }
}