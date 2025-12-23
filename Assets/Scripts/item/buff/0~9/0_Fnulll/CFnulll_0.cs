using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFnulll_0 : Citem_33
{

}
public class DFnulll_0 : DataBase
{

}
public class SFnulll_0 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFnulll_0 card = _card as CFnulll_0;
        MsgOnItem msg = _msg as MsgOnItem;

        Debug.Log(msg.op);
    }
}