using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLELEnulll_0 : Citem_33
{

}
public class DLELEnulll_0 : DataBase
{

}

public class SLELEnulll_0 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLELEnulll_0 card = _card as CLELEnulll_0;
        MsgOnItem msg = _msg as MsgOnItem;
        DLELEnulll_0 config = basicConfig as DLELEnulll_0;

        Debug.Log(msg.op);
    }
}